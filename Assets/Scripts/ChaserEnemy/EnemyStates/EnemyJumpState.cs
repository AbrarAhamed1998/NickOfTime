using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Enemy
{
	public class EnemyJumpState : EnemyStateBase
	{
		public EnemyJumpState(EnemyAI enemy) : base(enemy)
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
		}

		public override void OnJump()
		{
			base.OnJump();
		}

		public override void OnStateExit()
		{
			base.OnStateExit();
		}
	}
}

