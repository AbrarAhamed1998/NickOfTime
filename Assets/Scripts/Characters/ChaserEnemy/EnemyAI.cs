using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using NickOfTime.ScriptableObjects.Enemy;
using NickOfTime.Characters;
using NickOfTime.Characters.CharacterStates;
using NickOfTime.Weapons;
using NickOfTime.Characters.Player;
using NickOfTime.Managers;

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

		private EnemyStateBase _idleEnemyState, _moveEnemyState, _jumpEnemyState;

		private Vector2 _waypointDirection; 

		public EnemyStateBase CurrentEnemyState {
			get => (EnemyStateBase)CurrentCharacterState;
			set
			{
				CurrentCharacterState = value;
			}
		}

		#region OVERRIDES

		protected override void Start()
		{
			RegisterControlEvents();
			_idleEnemyState = new EnemyIdleState(this);
			_moveEnemyState = new EnemyMoveState(this);
			_jumpEnemyState = new EnemyJumpState(this);

			CurrentEnemyState = _idleEnemyState;

			StartCoroutine(CalculatePathRoutine());
			StartCoroutine(CheckForPlayerInVicinityRoutine());
			StartCoroutine(CheckForUseWeaponOnPlayer());
		}

		protected override void FixedUpdate()
		{
			CurrentEnemyState?.OnStateFixedUpdate();
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
		}

		protected override void LookAtWorldPos(Transform targetWorldTransform)
		{
			base.LookAtWorldPos(targetWorldTransform);
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

		public override void EquipWeapon(WeaponBase weapon)
		{
			base.EquipWeapon(weapon);
			if(weapon.transform == lookTarget)
			{
				lookTarget = null;
			}
		}

		#endregion


		#region IENUMERATORS

		private IEnumerator CalculatePathRoutine()
		{
            while(gameObject.activeSelf)
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
			while (this.gameObject.activeSelf)
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
			while(this.gameObject.activeSelf)
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
				Debug.Log("Use weapon called");
				UseWeapon();
			}
		}

		#endregion
	}
}

