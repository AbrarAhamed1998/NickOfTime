using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.ScriptableObjects.Enemy
{
	[CreateAssetMenu(fileName = "EnemyConfigSO.asset", menuName = "Scriptable Objects/Enemy/EnemyConfig")]
    public class EnemyConfigSO : ScriptableObject
    {
		[SerializeField] private float _movementSpeed;
		[SerializeField] private float _lookSensitivity;
		[SerializeField] private float _jumpForce;
		[SerializeField] private Vector2 _groundCheckBoxSize;
		[SerializeField] private Color _groundCheckDebugColor;
		[SerializeField] private LayerMask _groundCheckLayerMask;
		[SerializeField] private float _pickNextWaypointDist;
		[SerializeField] private float _pathCalcInterval;
		[SerializeField] private float _yThresholdToTriggerJump;
		[SerializeField] private float _jumpInterval;

		public float MovementSpeed => _movementSpeed;
		public float LookSensitivity => _lookSensitivity;
		public float JumpForce => _jumpForce;
		public Vector2 GroundCheckBoxSize => _groundCheckBoxSize;
		public Color GroundCheckBoxColor => _groundCheckDebugColor;
		public LayerMask GroundCheckLayerMask => _groundCheckLayerMask;
		public float PickNextWaypointDist => _pickNextWaypointDist;
		public float PathCalcInterval => _pathCalcInterval;	
		public float YThresholdToTriggerJump => _yThresholdToTriggerJump;
		public float JumpInterval => _jumpInterval;
	}
}

