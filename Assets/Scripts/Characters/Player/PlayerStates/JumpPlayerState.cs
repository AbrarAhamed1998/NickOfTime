using NickOfTime.Characters.Player.PlayerStates;
using NickOfTime.Characters.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Characters.Player.PlayerStates
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
			player.CharacterMove();
			player.CharacterLook();
			player.CheckIfCharacterInAir();
		}

		public override void OnCharacterJump()
		{
			base.OnCharacterJump();
		}

		public override void OnCharacterUseWeapon()
		{
			base.OnCharacterUseWeapon();
			player.CharacterUseWeapon();
		}

		public override void OnStateExit()
		{
			base.OnStateExit();
		}
	}
}

