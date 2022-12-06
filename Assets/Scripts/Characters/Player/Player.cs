using NickOfTime.Characters.CharacterStates;
using NickOfTime.Characters.Player.PlayerStates;
using NickOfTime.Managers;
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
            PlayerHealthPoints = _characterConfig.DefaultHealthPoints;
            _movePlayerState = new MovePlayerState(this);
            _jumpPlayerState = new JumpPlayerState(this);
            _idlePlayerState = new IdlePlayerState(this);
            CurrentPlayerState = _idlePlayerState;
            PersistentDataManager.instance.ActivePlayer = this;
            _characterHealthSlider = PersistentDataManager.instance.UIManager.PlayerHealthBar;
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

		protected override void ChangeCharacterState(CharacterStateBase state)
		{
            base.ChangeCharacterState(state);
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
        public override void CharacterMove()
        {
            moveAction?.Invoke();
        }

        public override void CharacterLook()
        {
            lookAction?.Invoke();
        }

        public override void CharacterJump()
        {
            jumpAction?.Invoke();
        }

        public override void CharacterUseWeapon()
		{
            fireAction?.Invoke();
		}

		public override void CheckIfCharacterInAir()
		{
            base.CheckIfCharacterInAir();
		}

		protected override void CharacterInAir()
		{
			base.CharacterInAir();
            ChangeCharacterState(_jumpPlayerState);
		}

		protected override void CharacterOnGround()
		{
			base.CharacterOnGround();
            ChangeCharacterState(_idlePlayerState);
		}

		public override void CheckIfCharacterMoving()
		{
            if (_characterRigidbody.velocity.x != 0f)
                ChangeCharacterState(_movePlayerState);
            else
                ChangeCharacterState(_idlePlayerState);
		}

        public override void EquipWeapon(WeaponBase weapon)
		{
            base.EquipWeapon(weapon);
            weapon.SetProjectleLayer(true);
		}

		
		#endregion
	}

}
