using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Characters.Player.PlayerStates
{
    public class DeathPlayerState : PlayerStateBase
    {
		public DeathPlayerState(Player player) : base(player)
		{
		}

		public override void OnStateEnter()
		{
			base.OnStateEnter();
			/// Fall to the ground if in air
			/// on hit ground splatter
			/// Should be in lie down pose and be inactive 
			/// Maybe turn off collider and set sort layer to back and freeze pos
		}

	}
}

