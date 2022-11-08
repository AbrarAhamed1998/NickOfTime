using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.ScriptableObjects.Player
{
	[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Scriptable Objects/Player/Player Config")]
	public class PlayerConfig : ScriptableObject
	{
		[SerializeField] private float _movementSpeed;
		[SerializeField] private float _lookSensitivity;
		[SerializeField] private float _jumpForce;
		[SerializeField] private Vector2 _groundCheckBoxSize;
		[SerializeField] private Color _groundCheckDebugColor;
		[SerializeField] private LayerMask _groundCheckLayerMask;

		public float MovementSpeed => _movementSpeed;
		public float LookSensitivity => _lookSensitivity;
		public float JumpForce => _jumpForce; 
		public Vector2 GroundCheckBoxSize => _groundCheckBoxSize;
		public Color GroundCheckBoxColor => _groundCheckDebugColor;
		public LayerMask GroundCheckLayerMask => _groundCheckLayerMask;
	}
}

