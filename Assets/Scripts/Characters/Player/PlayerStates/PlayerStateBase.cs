using NickOfTime.Characters.CharacterStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Characters.Player.PlayerStates
{
	public class PlayerStateBase : CharacterStateBase
	{
		protected Player player;
		public PlayerStateBase(Player player) : base(player)
		{
			this.player = player;
		}

	}
}

