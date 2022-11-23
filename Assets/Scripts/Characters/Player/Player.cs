using NickOfTime.Characters.Player.PlayerStates;
using NickOfTime.ScriptableObjects.Player;
using NickOfTime.Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static PlayerControls;


namespace NickOfTime.Characters.Player
{
    public class Player : CharacterBase, IPlayerActions
    {
        protected PlayerStateBase _movePlayerState, _jumpPlayerState, _idlePlayerState;
		#region PROPERTIES
		public PlayerStateBase CurrentPlayerState {
            get => (PlayerStateBase)CurrentCharacterState;
			set
			{
                CurrentCharacterState = value;
			}
        }
		#endregion

		#region UNITY CALLBACKS

		protected override void OnEnable()
        {
            _playerControl = new PlayerControls();
            _playerControl.Player.SetCallbacks(this);
            _playerControl.Player.Move.Enable();
            _playerControl.Player.Look.Enable();
            _playerControl.Player.Jump.Enable();
            _playerControl.Player.Fire.Enable();
            RegisterControlEvents();
        }

        protected override void OnDisable()
        {
            _playerControl.Player.Move.Disable();
            _playerControl.Player.Look.Disable();
            _playerControl.Player.Jump.Disable();
            _playerControl.Player.Fire.Disable();
            DeregisterControlEvents();
        }

        protected override void Start()
		{
			_movePlayerState = new MovePlayerState(this);
            _jumpPlayerState = new JumpPlayerState(this);
            _idlePlayerState = new IdlePlayerState(this);
            CurrentPlayerState = _idlePlayerState;
		}

        protected override void Update()
		{
            CurrentPlayerState?.OnStateUpdate();
		}

        protected override void FixedUpdate()
		{
            CurrentPlayerState?.OnStateFixedUpdate();
		}

        protected override void OnDrawGizmos()
        {
            Gizmos.color = _characterConfig.GroundCheckBoxColor;
            Gizmos.DrawCube(_groundCheckBox.position, _characterConfig.GroundCheckBoxSize);
        }

		#endregion

		#region PRIVATE METHODS

		protected override void ChangePlayerState(PlayerStateBase state)
		{
            if (CurrentPlayerState == state) return;
            CurrentPlayerState?.OnStateExit();
            CurrentPlayerState = state;
            CurrentPlayerState?.OnStateEnter();
		}

        protected override void RegisterControlEvents()
        {
            base.RegisterControlEvents();
        }

        protected override void DeregisterControlEvents()
		{
            base.DeregisterControlEvents();
		}

        protected override void LookAtScreenPos()
        {
            base.LookAtScreenPos();
        }

        protected override void UseWeapon()
		{
            if (_equippedWeapon != null)
                _equippedWeapon.UseWeapon();
		}

		#endregion

		#region PUBLIC METHODS

		public void OnMove(InputAction.CallbackContext context)
        {
            _moveDirection = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            _lookTargetScreenPos = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            ButtonControl buttonControl = (ButtonControl)context.control;
            if(buttonControl.wasPressedThisFrame && context.performed)
                CurrentPlayerState?.OnCharacterJump();
        }
        public void OnFire(InputAction.CallbackContext context)
        {
            ButtonControl buttonControl = (ButtonControl)context.control;
            if (buttonControl.wasPressedThisFrame && context.performed)
                CurrentPlayerState?.OnCharacterUseWeapon();
        }
        public override void PlayerMove()
        {
            moveAction?.Invoke();
        }

        public override void PlayerLook()
        {
            lookAction?.Invoke();
        }

        public override void PlayerJump()
        {
            jumpAction?.Invoke();
        }

        public override void PlayerUseWeapon()
		{
            fireAction?.Invoke();
		}

		public override void CheckIfChracterInAir()
		{
            base.CheckIfChracterInAir();
		}

		protected override void CharacterInAir()
		{
			base.CharacterInAir();
            ChangePlayerState(_jumpPlayerState);
		}

		protected override void CharacterOnGround()
		{
			base.CharacterOnGround();
            ChangePlayerState(_idlePlayerState);
		}

		public override void CheckIfCharacterMoving()
		{
            if (_playerRigidbody.velocity.x != 0f)
                ChangePlayerState(_movePlayerState);
            else
                ChangePlayerState(_idlePlayerState);
		}

        public override void EquipWeapon(WeaponBase weapon)
		{
            base.EquipWeapon(weapon);
		}

		
		#endregion
	}

}