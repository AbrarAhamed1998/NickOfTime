using NickOfTime.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Player
{
    public class JumpPlayerState : PlayerStateBase
    {
		public JumpPlayerState(Player player) : base(player)
		{
		}

		public override void OnStateEnter()
		{
			base.OnStateEnter();
		}

		public override void OnStateUpdate()
		{
			base.OnStateUpdate();
			
		}

		public override void OnStateFixedUpdate()
		{
			base.OnStateFixedUpdate();
			player.PlayerMove();
			player.PlayerLook();
			player.CheckIfPlayerInAir();
		}

		public override void OnPlayerJump()
		{
			base.OnPlayerJump();
		}

		public override void OnStateExit()
		{
			base.OnStateExit();
		}
	}
}

