using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Player
{
	public class PlayerStateBase
	{
		protected Player player;

		public void SetPlayer(Player player)
		{
			this.player = player;
		}
		public virtual void OnStateEnter()
		{

		}

		public virtual void OnStateUpdate()
		{

		}

		public virtual void OnStateFixedUpdate()
		{

		}

		public virtual void OnStateExit()
		{

		}

		public virtual void OnPlayerJump()
		{

		}
	}
}

