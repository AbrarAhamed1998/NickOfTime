using NickOfTime.ScriptableObjects.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.ScriptableObjects.Enemy
{
	[CreateAssetMenu(fileName = "EnemyConfigSO.asset", menuName = "Scriptable Objects/Enemy/EnemyConfig")]
    public class EnemyConfigSO : CharacterBaseConfigSO
    {
		[SerializeField] private float _pickNextWaypointDist;
		[SerializeField] private float _pathCalcInterval;
		[SerializeField] private float _yThresholdToTriggerJump;
		[SerializeField] private float _jumpInterval;

		public float PickNextWaypointDist => _pickNextWaypointDist;
		public float PathCalcInterval => _pathCalcInterval;	
		public float YThresholdToTriggerJump => _yThresholdToTriggerJump;
		public float JumpInterval => _jumpInterval;
	}
}

