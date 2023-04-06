using NickOfTime.Characters.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Characters.CharacterStates
{
	public class BossInTankState : BossStateBase
	{
		public BossInTankState(BossCharacter bossCharacter) : base(bossCharacter)
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
			bossCharacter.BossTank.TankLook();
		}

		public override void OnStateFixedUpdate()
		{
			base.OnStateFixedUpdate();
			bossCharacter.BossTank.WaypointDirection = bossCharacter.TankWaypointDirection();
			bossCharacter.BossTank.TankMove();
		}

		public override void OnCharacterTakeDamage()
		{
			base.OnCharacterTakeDamage();
			//bossCharacter.BossTank.TakeDamage();
		}
	}
}

