using NickOfTime.Characters.CharacterStates;
using NickOfTime.UI;
using NickOfTime.UI.DialogSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace NickOfTime.Characters.Enemy
{
    public class BossCharacter : CharacterBase
    {
		[Header("Boss Tank Variables")]
        [SerializeField] private BossTank _bossTank;
        [SerializeField] private Seeker _bossTankSeeker;
        [SerializeField] private Rigidbody2D _tankRigidbody;

        public bool CanUseWeapon;
        public bool CanCheckForPlayer;

        private Path aiPath;
        private int currentWaypoint = 0;
        bool reachedEndOfPath;

        public BossStateBase CurrentBossState;

        protected BossStateBase _tankState, _idleState, _moveState, _dialogState;


		protected override void Start()
		{
            _tankState = new BossStateBase(this);
            ChangeBossState(_tankState);
		}

		protected void Update()
		{
			
		}

        public void ChangeBossState(BossStateBase toBossState)
		{
            CurrentBossState?.OnStateExit();
            CurrentBossState = toBossState;
            CurrentBossState?.OnStateEnter();
		}
	}
}

