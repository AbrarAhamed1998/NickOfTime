using NickOfTime.Characters.Player.PlayerStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Characters.Player.PlayerStates
{
	public class MovePlayerState : PlayerStateBase
	{
		public MovePlayerState(CharacterBase player) : base((Player)player)
		{

		}

		public override void OnStateEnter()
		{
			base.OnStateEnter();

		}

		public override void OnStateUpdate()
		{
			base.OnStateUpdate();
			player.CheckIfCharacterMoving();
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
			player.CharacterJump();
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

