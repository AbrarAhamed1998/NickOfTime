using NickOfTime.Characters.CharacterStates;
using NickOfTime.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Characters.Enemy.EnemyStates
{
    public class EnemyStateBase : CharacterStateBase
    {
		protected EnemyAI enemy;
		public EnemyStateBase(EnemyAI enemy) : base(enemy)
		{
			this.enemy = enemy;
		}
	}
}

