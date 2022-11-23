using NickOfTime.Characters.CharacterStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Enemy
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

