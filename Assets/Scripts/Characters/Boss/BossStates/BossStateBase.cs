using NickOfTime.Characters.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Characters.CharacterStates
{
    public class BossStateBase : CharacterStateBase
    {
        protected BossCharacter bossCharacter;

		public BossStateBase(BossCharacter character) : base(character)
		{

		}

		public override void OnStateEnter()
		{
			base.OnStateEnter();
		}

		public override void OnStateUpdate()
		{

		}

		public override void OnStateFixedUpdate()
		{

		}

		public  override void OnStateExit()
		{

		}

		public virtual void OnHit()
		{

		}

		public virtual void OnDeath()
		{

		}
	}
}


