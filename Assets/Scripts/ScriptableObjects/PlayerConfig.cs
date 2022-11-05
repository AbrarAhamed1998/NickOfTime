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

		public float MovementSpeed => _movementSpeed;
		public float LookSensitivity => _lookSensitivity;
		public float JumpForce => _jumpForce; 
	}
}

