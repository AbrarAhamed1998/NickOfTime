using NickOfTime.Characters.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Characters.CharacterStates
{
    public class BossStateBase
    {
        protected BossCharacter bossCharacter;

        public BossStateBase(BossCharacter bossCharacter)
		{
			this.bossCharacter = bossCharacter;
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
	}
}


