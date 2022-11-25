using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.ScriptableObjects.Characters
{
    public class CharacterBaseConfigSO : ScriptableObject
    {
		[SerializeField] protected float _movementSpeed;
		[SerializeField] protected float _lookSensitivity;
		[SerializeField] protected float _jumpForce;
		[SerializeField] protected float _jetPackRotSpeed;
		[SerializeField] protected Vector2 _groundCheckBoxSize;
		[SerializeField] protected Color _groundCheckDebugColor;
		[SerializeField] protected LayerMask _groundCheckLayerMask;

		public float MovementSpeed => _movementSpeed;
		public float LookSensitivity => _lookSensitivity;
		public float JumpForce => _jumpForce;
		public float JetPackRotSpeed => _jetPackRotSpeed;
		public Vector2 GroundCheckBoxSize => _groundCheckBoxSize;
		public Color GroundCheckBoxColor => _groundCheckDebugColor;
		public LayerMask GroundCheckLayerMask => _groundCheckLayerMask;
	}
}

