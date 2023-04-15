using NickOfTime.Characters.CharacterStates;
using NickOfTime.UI;
using NickOfTime.UI.DialogSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using NickOfTime.ScriptableObjects.Enemy;
using NickOfTime.Managers;
using Player = NickOfTime.Characters.Player.Player;
using DG.Tweening;
using System;

namespace NickOfTime.Characters.Enemy
{
    public class BossCharacter : CharacterBase
    {
		private BossStatsSO _bossConfig => (BossStatsSO)_characterConfig;

		[Header("Boss Tank Variables")]
        [SerializeField] private BossTank _bossTank;
        [SerializeField] private Seeker _bossTankSeeker;
        [SerializeField] private Rigidbody2D _tankRigidbody;

		[SerializeField] private Transform _lookTarget;

		public Seeker ActiveSeeker { get; set; }
		public Transform ActiveBossTransform { get; set; }

		public BossTank BossTank => _bossTank;

		private Vector2 _sphereCastOrigin;
		private Vector3 _characetrViewingOrigin;

        public bool CanUseWeapon;
        public bool CanCheckForPlayer;

        private Path aiPath;
        private int currentWaypoint = 0;
        bool reachedEndOfPath;

		protected CanvasGroup _bossHealthSliderGroup;

		public Action<Vector2> OnSphereCastOriginChanged;
		public Action<Vector3> OnCharacetrViewingOriginChanged;

        public BossStateBase CurrentBossState {
			get => (BossStateBase)CurrentCharacterState;
			set => CurrentCharacterState = value;
		}

        protected BossStateBase _tankState, _idleState, _moveState, _dialogState, _deathState;

		protected BossHealthSlider _bossHealthSlider {
			get => (BossHealthSlider)_characterHealthSlider;
			set => _characterHealthSlider = value;
		}

		public BossHealthSlider BossHealthSlider => _bossHealthSlider;

		protected override void Start()
		{
			CharacterHealthPoints = _characterConfig.DefaultHealthPoints;
			_tankState = new BossInTankState(this);
            ChangeBossState(_tankState);
			StartCoroutine(DetectGameStart());
			StartCoroutine(CalculatePathRoutine());
			StartCoroutine(CheckForPlayerInVicinityRoutine());
			StartCoroutine(CheckForUseWeaponOnPlayer());
		}

		protected override void Update()
		{
			base.Update();
		}

		protected override void RegisterControlEvents()
		{
			base.RegisterControlEvents();
			OnSphereCastOriginChanged += (origin) => _sphereCastOrigin = origin;
			OnCharacetrViewingOriginChanged += (viewingTransform) => _characetrViewingOrigin = viewingTransform;
			_bossTank.TakeDamageAction += (damage, direction) =>
			{
				NegateDamageFromHealth(damage);
			};
		}

		protected override void UseWeapon()
		{
			BossTank.TankAttack();
		}

		public override void TakeDamage(float damageValue, Vector2 direction)
		{
			base.TakeDamage(damageValue, direction);
			CurrentBossState?.OnCharacterTakeDamage();
		}

		public void ChangeBossState(BossStateBase toBossState)
		{
            CurrentBossState?.OnStateExit();
            CurrentBossState = toBossState;
            CurrentBossState?.OnStateEnter();
		}

		public void OnPathComplete(Path p)
		{
			if (!p.error)
			{
				aiPath = p;
				currentWaypoint = 0;
			}
		}

		public Vector2 TankWaypointDirection()
		{
			Vector2 waypoint = GetWayPointDirection();
			return new Vector2(waypoint.x, 0f);
		}

		private Vector2 GetWayPointDirection()
		{
			if (aiPath == null)
				return Vector2.zero;

			if (currentWaypoint >= aiPath.vectorPath.Count)
			{
				reachedEndOfPath = true;
				return Vector2.zero;
			}


			Vector2 waypointDirection = ((Vector2)aiPath.vectorPath[currentWaypoint] - _tankRigidbody.position).normalized;

			float distance = Vector2.Distance(_tankRigidbody.position, aiPath.vectorPath[currentWaypoint]);
			if (distance < _bossConfig.PickNextWaypointDist)
				currentWaypoint++;

			return waypointDirection;
		}

		

		private void CheckIfPlayerInSight()
		{
			Player.Player target = PersistentDataManager.instance.ActivePlayer;
			ContactFilter2D contactFilter = new ContactFilter2D();
			contactFilter.useLayerMask = true;
			contactFilter.SetLayerMask(_bossConfig.LineOfSightLayerMask);
			RaycastHit2D[] hits = new RaycastHit2D[1];
			Vector3 viewPosition = _characetrViewingOrigin;
			if (Physics2D.Raycast(viewPosition, target.transform.position - viewPosition, contactFilter, hits) > 0)
			{
				Player.Player hitPlayer = hits[0].collider.gameObject.GetComponent<Player.Player>();
				Debug.DrawRay(viewPosition, target.transform.position - viewPosition, Color.red, 1f);
				if (hitPlayer == null) return;
				// try using weapon on player
				UseWeapon();
			}
		}

		private void CheckIfPlayerInVicinity()
		{
			if (_lookTarget != null) return;
			ContactFilter2D contactFilter = new ContactFilter2D();
			contactFilter.useLayerMask = true;
			contactFilter.SetLayerMask(_bossConfig.PlayerCheckLayerMask);
			RaycastHit2D[] hits = new RaycastHit2D[1];
			Vector2 origin =_sphereCastOrigin;
			if (Physics2D.CircleCast(origin, 2f, Vector2.right, contactFilter, hits) > 0)
			{
				Player.Player player = hits[0].collider.gameObject.GetComponent<Player.Player>();
				if (player != null)
				{
					_lookTarget = player.transform;
					_bossTank.TankTarget = player.transform;
					_bossHealthSliderGroup.DOFade(1f, 0.5f);
				}
			}
		}

		private IEnumerator CalculatePathRoutine()
		{
			while (gameObject.activeSelf && CurrentCharacterState != _deathState)
			{
				if (_lookTarget != null)
					_bossTankSeeker.StartPath(_tankRigidbody.position, _lookTarget.position, OnPathComplete);
				yield return new WaitForSeconds(_bossConfig.PathCalcInterval);
			}
		}


		private IEnumerator CheckForUseWeaponOnPlayer()
		{
			while (this.gameObject.activeSelf && CurrentCharacterState != _deathState)
			{
				if (!CanUseWeapon)
				{
					yield return null;
					continue;
				}
				yield return new WaitForSeconds(_bossConfig.UseWeaponInterval); // you could randomize this interval
				CanUseWeapon = false;
				CheckIfPlayerInSight();
				CanUseWeapon = true;
			}
		}

		private IEnumerator CheckForPlayerInVicinityRoutine()
		{
			while (this.gameObject.activeSelf && CurrentCharacterState != _deathState)
			{
				if (!CanCheckForPlayer)
				{
					yield return null;
					continue;
				}
				yield return new WaitForSeconds(1f);
				CanCheckForPlayer = false;
				CheckIfPlayerInVicinity();
				CanCheckForPlayer = true;
			}
		}

		private IEnumerator DetectGameStart()
		{
			yield return new WaitUntil(() => PersistentDataManager.instance != null);
			yield return new WaitUntil(() => PersistentDataManager.instance.UIManager != null);
			_bossHealthSlider = PersistentDataManager.instance.UIManager.BossHealthBar;
			_bossHealthSlider.SetBossName(_bossConfig.BossName);
			_bossHealthSliderGroup = _bossHealthSlider.GetComponentInParent<CanvasGroup>();
		}
	}
}

