using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using NickOfTime.ScriptableObjects.Enemy;

namespace NickOfTime.Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private Transform lookTarget;

		[SerializeField] private EnemyConfigSO _enemyConfig;

		[SerializeField] private GameObject _debugLookObject;

		[SerializeField] private SpriteRenderer _enemySprite;

		[SerializeField] private GameObject _groundCheckBox;

		public bool IsGrounded;
		public bool CanJump;

        private Path aiPath;
        private int currentWaypoint = 0;
        bool reachedEndOfPath;

		[SerializeField] private Seeker seeker;
        [SerializeField] private Rigidbody2D myRigidbody;

		protected Action moveAction, jumpAction, lookAction;

		private EnemyStateBase _currentEnemyState, _idleEnemyState, _moveEnemyState, _jumpEnemyState;

		private Vector2 _waypointDirection; 

		public EnemyStateBase CurrentEnemyState {
			get => _currentEnemyState;
			set
			{
				_currentEnemyState = value;
			}
		}

		private void Start()
		{
			RegisterEnemyEvents();
			_idleEnemyState = new EnemyIdleState(this);
			_moveEnemyState = new EnemyMoveState(this);
			_jumpEnemyState = new EnemyJumpState(this);

			CurrentEnemyState = _idleEnemyState;

			StartCoroutine(CalculatePathRoutine());
		}

		private void FixedUpdate()
		{
			CurrentEnemyState?.OnStateFixedUpdate();
		}

		private void Update()
		{
			CurrentEnemyState?.OnStateUpdate();
		}

		private void ChangeEnemyState(EnemyStateBase enemyState)
		{
			CurrentEnemyState?.OnStateExit();
			CurrentEnemyState = enemyState;
			CurrentEnemyState?.OnStateEnter();
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

		public void CheckIfEnemyInAir()
		{
			Collider2D[] colliders = new Collider2D[1];
			int results = Physics2D.OverlapBoxNonAlloc(_groundCheckBox.transform.position,
				_enemyConfig.GroundCheckBoxSize,
				0f,
				colliders,
				_enemyConfig.GroundCheckLayerMask
				);
			if (results > 0)
			{
				//Debug.Log($"hit the ground {colliders[0].name}");
				IsGrounded = true;
				ChangeEnemyState(_idleEnemyState);
			}
			else
			{
				//Debug.Log("Off the Ground");
				IsGrounded = false;
				ChangeEnemyState(_jumpEnemyState);
			}
		}

		public void CheckIfEnemyMoving()
		{
			if (myRigidbody.velocity.x != 0f)
				ChangeEnemyState(_moveEnemyState);
			else
				ChangeEnemyState(_idleEnemyState);
		}

		public void CheckForJump()
		{
			Debug.Log($"_waypoint Dir : {_waypointDirection.y}");
			if (_waypointDirection.y > _enemyConfig.YThresholdToTriggerJump && CanJump)
			{
				CanJump = false;
				StartCoroutine(JumpRoutine());
			}
		}

		public void EnemyMove()
		{
			moveAction?.Invoke();
		}

		public void EnemyLook()
		{
			lookAction?.Invoke();
		}

		public void EnemyJump()
		{
			jumpAction?.Invoke();
		}

		public void RegisterEnemyEvents()
		{
			moveAction = () => 
			{
				_waypointDirection = GetWayPointDirection();
				myRigidbody.AddForce(_waypointDirection * _enemyConfig.MovementSpeed * Time.deltaTime);
			};
			lookAction = () =>
			{
				LookAtTarget();
			};
			jumpAction = () =>
			{
				myRigidbody.AddForce(Vector2.up * _enemyConfig.JumpForce, ForceMode2D.Impulse);
			};
		}

		private void LookAtTarget()
		{
			Vector2 worldPos = lookTarget.position;
			Transform target = _debugLookObject.transform;
			float y = target.position.y - worldPos.y;
			float x = target.position.x - worldPos.x;
			float targetAngle = (Mathf.Atan2(y, x) * Mathf.Rad2Deg) + 180f;
			target.localEulerAngles = new Vector3(0f, 0f, targetAngle);

			Vector2 mouseDirection = (worldPos - (Vector2)this.transform.position).normalized * Vector2.right;
			Vector2 playerdirection = Vector2.right * (_enemySprite.flipX ? -1f : 1f);
			float dotProduct = Vector2.Dot(mouseDirection, playerdirection);
			if (dotProduct < 0f)
				_enemySprite.flipX = !_enemySprite.flipX;
		}
        private IEnumerator CalculatePathRoutine()
		{
            while(gameObject.activeSelf)
			{
                seeker.StartPath(myRigidbody.position, lookTarget.position, OnPathComplete);

                yield return new WaitForSeconds(_enemyConfig.PathCalcInterval);
			}
		}
		private IEnumerator JumpRoutine()
		{
			CurrentEnemyState?.OnJump();
			yield return new WaitForSeconds(_enemyConfig.JumpInterval);
			CanJump = true;
		}
	}
}

