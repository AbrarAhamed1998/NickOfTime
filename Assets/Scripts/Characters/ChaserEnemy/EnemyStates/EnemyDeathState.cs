using NickOfTime.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Characters.Enemy.EnemyStates
{
	public class EnemyDeathState : EnemyStateBase
	{
		public EnemyDeathState(EnemyAI enemy) : base(enemy)
		{
		}

		public override void OnStateEnter()
		{
			base.OnStateEnter();
			// Fall flat and die 
			// Take extra impulse from a hit maybe?
			// splatter
			enemy.CharacterDeath();
		}
	}

}
