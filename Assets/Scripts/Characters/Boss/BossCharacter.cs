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

		public BossTank BossTank => _bossTank;


        public bool CanUseWeapon;
        public bool CanCheckForPlayer;

        private Path aiPath;
        private int currentWaypoint = 0;
        bool reachedEndOfPath;

        public BossStateBase CurrentBossState {
			get => (BossStateBase)CurrentCharacterState;
			set => CurrentCharacterState = value;
		}

        protected BossStateBase _tankState, _idleState, _moveState, _dialogState, _deathState;


		protected override void Start()
		{
            _tankState = new BossInTankState(this);
            ChangeBossState(_tankState);

			StartCoroutine(CalculatePathRoutine());
			StartCoroutine(CheckForPlayerInVicinityRoutine());
			StartCoroutine(CheckForUseWeaponOnPlayer());
		}

		protected override void Update()
		{
			base.Update();
		}

		protected override void UseWeapon()
		{
			BossTank.TankAttack();
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
			if (Physics2D.Raycast(this.transform.position, target.transform.position - transform.position, contactFilter, hits) > 0)
			{
				Player.Player hitPlayer = hits[0].collider.gameObject.GetComponent<Player.Player>();
				Debug.DrawRay(this.transform.position, target.transform.position - transform.position, Color.red, 1f);
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
			if (Physics2D.CircleCast(this.transform.position, 2f, Vector2.right, contactFilter, hits) > 0)
			{
				Player.Player player = hits[0].collider.gameObject.GetComponent<Player.Player>();
				if (player != null)
				{
					_lookTarget = player.transform;
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
				yield return new WaitForSeconds(1f); // you could randomize this interval
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
	}
}

