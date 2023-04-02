using NickOfTime.Characters.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Characters.CharacterStates
{
	public class TankStateBase : BossStateBase
	{
		public TankStateBase(BossCharacter bossCharacter) : base(bossCharacter)
		{
		}

		public override void OnStateEnter()
		{
			base.OnStateEnter();
		}

		public override void OnStateExit()
		{
			base.OnStateExit();
		}

		public override void OnStateUpdate()
		{
			base.OnStateUpdate();
		}

		public override void OnStateFixedUpdate()
		{
			base.OnStateFixedUpdate();
		}
	}
}

