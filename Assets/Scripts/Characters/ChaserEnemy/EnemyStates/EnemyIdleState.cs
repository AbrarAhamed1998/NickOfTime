using NickOfTime.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Characters.Enemy.EnemyStates
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
			enemy.CheckIfCharacterInAir();
			enemy.CharacterMove();
			enemy.CharacterLook();
		}

		public override void OnStateUpdate()
		{
			base.OnStateUpdate();
			enemy.CheckIfEnemyMoving();
			enemy.CheckForJump();
		}

		public override void OnCharacterJump()
		{
			base.OnCharacterJump();
			enemy.CharacterJump();
		}

		public override void OnStateExit()
		{
			base.OnStateExit();
		}
	}
}
