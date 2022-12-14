using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.ScriptableObjects.Characters
{
    public class CharacterBaseConfigSO : ScriptableObject
    {
		[Header("Player Action Speeds")]

		[SerializeField] protected float _movementSpeed;
		[SerializeField] protected float _lookSensitivity;
		[SerializeField] protected float _jumpForce;
		[SerializeField] protected float _jetPackRotSpeed;

		[Header("Character UI")]

		[SerializeField] protected float _defaultTotalHealthPoints;

		[Header("Character Damage")]

		[SerializeField] protected Color _damageFlashColor;
		[SerializeField] protected float _damageFlashTime;

		[Header("Ground Check Variables")]

		[SerializeField] protected Vector2 _groundCheckBoxSize;
		[SerializeField] protected Color _groundCheckDebugColor;
		[SerializeField] protected LayerMask _groundCheckLayerMask;

		[Header("Death Variables")]
		[SerializeField] protected float _deathBodyZRot;
		[SerializeField] protected float _deathHeadZRot;
		[SerializeField] protected float _deathEffectTime;

		/// <summary>
		/// These sprites are swapped based on damage taken by the player 
		/// with the highest index being the least damaged and 0 being the most damaged
		/// </summary>
		[Header("Death Sprites")]
		[SerializeField] protected Sprite[] _damageSprites;

		public float MovementSpeed => _movementSpeed;
		public float LookSensitivity => _lookSensitivity;
		public float JumpForce => _jumpForce;
		public float JetPackRotSpeed => _jetPackRotSpeed;
		public float DefaultHealthPoints => _defaultTotalHealthPoints;
		public float DamageFlashTime => _damageFlashTime;
		public Color DamageFlashColor => _damageFlashColor;
		public Vector2 GroundCheckBoxSize => _groundCheckBoxSize;
		public Color GroundCheckBoxColor => _groundCheckDebugColor;
		public LayerMask GroundCheckLayerMask => _groundCheckLayerMask;
		public float DeathBodyZRot => _deathBodyZRot;
		public float DeathHeadZRot => _deathHeadZRot;
		public Sprite[] DamageSprites => _damageSprites;
		public float DeathEffectTime => _deathEffectTime;
	}
}

