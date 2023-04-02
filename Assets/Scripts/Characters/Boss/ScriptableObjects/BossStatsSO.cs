using NickOfTime.ScriptableObjects.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NickOfTime.ScriptableObjects.Enemy
{
    [CreateAssetMenu(fileName = "BossConfig", menuName = "Scriptable Objects/Boss/Boss Config")]
    public class BossStatsSO : CharacterBaseConfigSO
    {
		[Header("Boss AI Variables")]

		[SerializeField] private float _pickNextWaypointDist;
		[SerializeField] private float _pathCalcInterval;
		[SerializeField] private float _yThresholdToTriggerJump;
		[SerializeField] private float _jumpInterval;
		[SerializeField] private float _useWeaponInterval;
		[SerializeField] private LayerMask _playerCheckLayerMask;
		[SerializeField] private LayerMask _lineOfSightLayerMask;

		public float PickNextWaypointDist => _pickNextWaypointDist;
		public float PathCalcInterval => _pathCalcInterval;
		public float YThresholdToTriggerJump => _yThresholdToTriggerJump;
		public float JumpInterval => _jumpInterval;
		public float UseWeaponInterval => _useWeaponInterval;
		public LayerMask PlayerCheckLayerMask => _playerCheckLayerMask;
		public LayerMask LineOfSightLayerMask => _lineOfSightLayerMask;
	}
}


