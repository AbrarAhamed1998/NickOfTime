using DG.Tweening;
using NickOfTime.Characters;
using NickOfTime.Characters.CharacterStates;
using NickOfTime.Characters.Enemy.EnemyStates;
using NickOfTime.Characters.Player;
using NickOfTime.Helper.Constants;
using NickOfTime.Managers;
using NickOfTime.ScriptableObjects.Enemy;
using NickOfTime.Utilities.PoolingSystem;
using NickOfTime.Weapons;
using Pathfinding;
using System.Collections;
using UnityEngine;

namespace NickOfTime.Enemy
{
	public class EnemyAI : CharacterBase
    {
        [SerializeField] private Transform lookTarget;

		[SerializeField] private EnemyConfigSO _enemyConfig => (EnemyConfigSO)_characterConfig;



		public bool CanJump;
		public bool CanUseWeapon;
		public bool CanCheckForPlayer;

        private Path aiPath;
        private int currentWaypoint = 0;
        bool reachedEndOfPath;

		[SerializeField] private Seeker seeker;
        [SerializeField] private Rigidbody2D myRigidbody;

		private EnemyStateBase _idleEnemyState, _moveEnemyState, _jumpEnemyState, _deathEnemyState;

		private Vector2 _waypointDirection;


		public EnemyStateBase CurrentEnemyState {
			get => (EnemyStateBase)CurrentCharacterState;
			set
			{
				CurrentCharacterState = value;
			}
		}

		#region PUBLIC METHODS

		public void TrackUIOnPlayer()
		{
			if(_characterHealthSlider != null)
				_characterHealthSlider.SetWordlPos(_uiRoot.position);
		}

		#endregion

		#region OVERRIDES

		protected override void Start()
		{
			CharacterHealthPoints = _characterConfig.DefaultHealthPoints;
			RegisterControlEvents();
			_idleEnemyState = new EnemyIdleState(this);
			_moveEnemyState = new EnemyMoveState(this);
			_jumpEnemyState = new EnemyJumpState(this);
			_deathEnemyState = new EnemyDeathState(this);
			CurrentEnemyState = _idleEnemyState;

			StartCoroutine(SetupCharacterUIRoutine());
			
			StartCoroutine(CalculatePathRoutine());
			StartCoroutine(CheckForPlayerInVicinityRoutine());
			StartCoroutine(CheckForUseWeaponOnPlayer());
		}

		protected override void FixedUpdate()
		{
			CurrentEnemyState?.OnStateFixedUpdate();
			TrackUIOnPlayer();
		}

		protected override void Update()
		{
			CurrentEnemyState?.OnStateUpdate();
		}

		protected override void OnDisable()
		{
			DeregisterControlEvents();
		}

		protected override void ChangeCharacterState(CharacterStateBase enemyState)
		{
			base.ChangeCharacterState(enemyState);
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
			

			 Vector2 waypointDirection = ((Vector2)aiPath.vectorPath[currentWaypoint] - myRigidbody.position).normalized;

			float distance = Vector2.Distance(myRigidbody.position, aiPath.vectorPath[currentWaypoint]);
			if (distance < _enemyConfig.PickNextWaypointDist)
				currentWaypoint++;
			
			return waypointDirection;
		}

		public void OnPathComplete(Path p)
		{
            if(!p.error)
			{
                aiPath = p;
                currentWaypoint = 0;
            }
        }

		public override void CheckIfCharacterInAir()
		{
			base.CheckIfCharacterInAir();
		}

		public void CheckIfEnemyMoving()
		{
			if (myRigidbody.velocity.x != 0f)
				ChangeCharacterState(_moveEnemyState);
			else
				ChangeCharacterState(_idleEnemyState);
		}

		public void CheckForJump()
		{
			if (_waypointDirection.y > _enemyConfig.YThresholdToTriggerJump && CanJump)
			{
				CanJump = false;
				StartCoroutine(JumpRoutine());
			}
		}

		public override void CharacterMove()
		{
			base.CharacterMove();
		}

		public override void CharacterLook()
		{
			base.CharacterLook();
		}

		public override void CharacterJump()
		{
			base.CharacterJump();
		}

		protected override void RegisterControlEvents()
		{
			moveAction = () => 
			{
				_waypointDirection = GetWayPointDirection();
				myRigidbody.AddForce(_waypointDirection * _enemyConfig.MovementSpeed * Time.deltaTime);
				JetControl(_waypointDirection);
			};
			lookAction = () =>
			{
				LookAtWorldPos(lookTarget);
			};
			jumpAction = () =>
			{
				myRigidbody.AddForce(Vector2.up * _enemyConfig.JumpForce, ForceMode2D.Impulse);
			};
			takeDamage = (damage, direction) =>
			{
				NegateDamageFromHealth(damage);
				ChangeToDamageSprite();
				DamageFlash();
				DamagePushBack(direction);
				CheckForCharacterDeath();
			};
			onCharacterDeath = () =>
			{
				OnDeath();
			};
		}

		protected override void LookAtWorldPos(Transform targetWorldTransform)
		{
			base.LookAtWorldPos(targetWorldTransform);
		}

		protected override void OnDeath()
		{
			base.OnDeath();
			Vector3 targetBodyRot = new Vector3(transform.localEulerAngles.x,
				transform.localEulerAngles.y,
				_characterConfig.DeathBodyZRot);
			Vector3 targetHeadRot = new Vector3(_debugLookObjects[0].transform.localEulerAngles.x,
				_debugLookObjects[0].transform.localEulerAngles.y,
				_characterConfig.DeathHeadZRot);

			transform.DOLocalRotate(targetBodyRot, 0.25f).OnComplete(
				() =>
				{
					//lookAction = null;
					_debugLookObjects[0].transform.DOLocalRotate(targetHeadRot, 0.25f);
				});
			PoolObject bleedVFX = PersistentDataManager.instance.PoolManager
				.GetPoolObject(NickOfTimeStringConstants.EFFECT_BLOODPOOL_POOL_ID, _deathVFXTransform);
			StartCoroutine(HandleDeathVFX(bleedVFX));
			Destroy(_characterHealthSlider.gameObject);
		}

		protected override void CharacterInAir()
		{
			base.CharacterInAir();
			ChangeCharacterState(_jumpEnemyState);
		}

		protected override void CharacterOnGround()
		{
			base.CharacterOnGround();
			ChangeCharacterState(_idleEnemyState);
		}

		public override void CheckForCharacterDeath()
		{
			base.CheckForCharacterDeath();
			if(CharacterHealthPoints <= 0f)
				ChangeCharacterState(_deathEnemyState);
		}

		public override void EquipWeapon(WeaponBase weapon)
		{
			base.EquipWeapon(weapon);
			if(weapon.transform == lookTarget)
			{
				lookTarget = null;
			}
			weapon.SetProjectleLayer(false);
		}

		public override void TakeDamage(float damageValue, Vector2 direction)
		{
			base.TakeDamage(damageValue, direction);
			CurrentEnemyState?.OnCharacterTakeDamage();
		}

		#endregion


		#region IENUMERATORS

		private IEnumerator CalculatePathRoutine()
		{
            while(gameObject.activeSelf && CurrentCharacterState != _deathEnemyState)
			{
				if(lookTarget != null)
					seeker.StartPath(myRigidbody.position, lookTarget.position, OnPathComplete);
                yield return new WaitForSeconds(_enemyConfig.PathCalcInterval);
			}
		}

		private IEnumerator JumpRoutine()
		{
			CurrentEnemyState?.OnCharacterJump();
			yield return new WaitForSeconds(_enemyConfig.JumpInterval);
			CanJump = true;
		}

		private IEnumerator CheckForUseWeaponOnPlayer()
		{
			while (this.gameObject.activeSelf && CurrentCharacterState != _deathEnemyState)
			{
				if (!CanUseWeapon)
				{
					yield return null;
					continue;
				}
				yield return new WaitForSeconds(1f);
				CanUseWeapon = false;
				CheckIfPlayerInSight();
				CanUseWeapon = true;
			}
		}

		private IEnumerator CheckForPlayerInVicinityRoutine()
		{
			while(this.gameObject.activeSelf && CurrentCharacterState != _deathEnemyState)
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



		private void CheckIfPlayerInVicinity()
		{
			if (lookTarget != null) return;
			ContactFilter2D contactFilter = new ContactFilter2D();
			contactFilter.useLayerMask = true;
			contactFilter.SetLayerMask(_enemyConfig.PlayerCheckLayerMask);
			RaycastHit2D[] hits = new RaycastHit2D[1];
			if (Physics2D.CircleCast(this.transform.position, 2f, Vector2.right,contactFilter, hits) > 0)
			{
				Player player = hits[0].collider.gameObject.GetComponent<Player>();
				if (player != null)
				{
					lookTarget = player.transform;
				}
			}
		}

		private void CheckIfPlayerInSight()
		{
			Player target = PersistentDataManager.instance.ActivePlayer;
			ContactFilter2D contactFilter = new ContactFilter2D();
			contactFilter.useLayerMask = true;
			contactFilter.SetLayerMask(_enemyConfig.LineOfSightLayerMask);
			RaycastHit2D[] hits = new RaycastHit2D[1];
			if (Physics2D.Raycast(this.transform.position, target.transform.position - transform.position ,contactFilter, hits) > 0)
			{
				Player hitPlayer = hits[0].collider.gameObject.GetComponent<Player>();
				Debug.DrawRay(this.transform.position, target.transform.position - transform.position, Color.red, 1f);
				if (hitPlayer == null) return;
				// try using weapon on player
				UseWeapon();
			}
		}

		IEnumerator SetupCharacterUIRoutine()
		{
			yield return new WaitUntil(() => _uiManager != null);
			_characterHealthSlider = _uiManager
				.SpawnHealthbar(_enemyConfig.HealthSliderPrefab, _uiRoot.transform.position);
		}

		IEnumerator HandleDeathVFX(PoolObject poolObject)
		{
			yield return new WaitForSeconds(_characterConfig.DeathEffectTime);
			PersistentDataManager.instance.PoolManager.ReturnObjectToPool(poolObject);
		}

		#endregion
	}
}

