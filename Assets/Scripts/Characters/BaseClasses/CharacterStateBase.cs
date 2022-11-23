using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Characters.CharacterStates
{
	public class CharacterStateBase
	{
		protected CharacterBase character;
		public CharacterStateBase(CharacterBase character)
		{
			this.character = character;
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

		public virtual void OnCharacterJump()
		{

		}

		public virtual void OnCharacterUseWeapon()
		{

		}
	}
}

