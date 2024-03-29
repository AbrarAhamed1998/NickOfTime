using NickOfTime.ScriptableObjects.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.ScriptableObjects.Enemy
{
	[CreateAssetMenu(fileName = "EnemyConfigSO.asset", menuName = "Scriptable Objects/Enemy/EnemyConfig")]
    public class EnemyConfigSO : CharacterBaseConfigSO
    {
		[SerializeField] protected float _jetPackRotSpeed;

		[Header("Death Variables")]
		[SerializeField] protected float _deathBodyZRot;
		[SerializeField] protected float _deathHeadZRot;
		[SerializeField] protected float _deathEffectTime;

		[Header("Enemy AI Variables")]

		[SerializeField] private float _pickNextWaypointDist;
		[SerializeField] private float _pathCalcInterval;
		[SerializeField] private float _yThresholdToTriggerJump;
		[SerializeField] private float _jumpInterval;
		[SerializeField] private float _useWeaponInterval;
		[SerializeField] private LayerMask _playerCheckLayerMask;
		[SerializeField] private LayerMask _lineOfSightLayerMask;
		[SerializeField] private GameObject _healthSliderPrefab;

		public float JetPackRotSpeed => _jetPackRotSpeed;
		public float DeathBodyZRot => _deathBodyZRot;
		public float DeathHeadZRot => _deathHeadZRot;
		public float DeathEffectTime => _deathEffectTime;
		public float PickNextWaypointDist => _pickNextWaypointDist;
		public float PathCalcInterval => _pathCalcInterval;	
		public float YThresholdToTriggerJump => _yThresholdToTriggerJump;
		public float JumpInterval => _jumpInterval;
		public float UseWeaponInterval => _useWeaponInterval;
		public LayerMask PlayerCheckLayerMask => _playerCheckLayerMask;
		public LayerMask LineOfSightLayerMask => _lineOfSightLayerMask;
		public GameObject HealthSliderPrefab => _healthSliderPrefab;
	}
}

