using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Player
{
	public class MovePlayerState : PlayerStateBase
	{
		public MovePlayerState(Player player) : base(player)
		{

		}

		public override void OnStateEnter()
		{
			base.OnStateEnter();

		}

		public override void OnStateUpdate()
		{
			base.OnStateUpdate();
			player.CheckIfPlayerMoving();
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
			player.PlayerJump();
		}

		public override void OnStateExit()
		{
			base.OnStateExit();
		}
	}
}

