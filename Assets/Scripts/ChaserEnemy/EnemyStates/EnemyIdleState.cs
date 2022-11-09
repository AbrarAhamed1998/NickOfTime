using NickOfTime.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Enemy
{
	public class EnemyIdleState : EnemyStateBase
	{
		public EnemyIdleState(EnemyAI enemy) : base(enemy)
		{
		}

		public override void OnStateEnter()
		{
			base.OnStateEnter();
		}

		public override void OnStateFixedUpdate()
		{
			base.OnStateFixedUpdate();
			enemy.CheckIfEnemyInAir();
			enemy.EnemyMove();
			enemy.EnemyLook();
		}

		public override void OnStateUpdate()
		{
			base.OnStateUpdate();
			enemy.CheckIfEnemyMoving();
			enemy.CheckForJump();
		}

		public override void OnJump()
		{
			base.OnJump();
			enemy.EnemyJump();
		}

		public override void OnStateExit()
		{
			base.OnStateExit();
		}
	}
}
