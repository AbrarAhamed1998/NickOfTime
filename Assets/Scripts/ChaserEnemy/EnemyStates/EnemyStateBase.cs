using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Enemy
{
    public class EnemyStateBase
    {
		protected EnemyAI enemy;
		public EnemyStateBase(EnemyAI enemy)
		{
			this.enemy = enemy;
		}

		public void SetPlayer(EnemyAI enemyAI)
		{
			this.enemy = enemyAI;
		}
		public virtual void OnStateEnter()
		{

		}

		public virtual void OnStateUpdate()
		{

		}

		public virtual void OnStateFixedUpdate()
		{

		}

		public virtual void OnStateExit()
		{

		}

		public virtual void OnJump()
		{

		}
	}
}

