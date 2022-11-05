using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Player
{
	public class IdlePlayerState : PlayerStateBase
	{
		public override void OnStateEnter()
		{
			base.OnStateEnter();
		}

		public override void OnStateUpdate()
		{
			base.OnStateUpdate();
			player.MovePlayer();
		}

		public override void OnStateFixedUpdate()
		{
			base.OnStateFixedUpdate();
		}

		public override void OnStateExit()
		{
			base.OnStateExit();
		}
	}
}

