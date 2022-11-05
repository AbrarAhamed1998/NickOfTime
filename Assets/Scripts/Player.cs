using NickOfTime.ScriptableObjects.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerControls;


namespace NickOfTime.Player
{
    public class Player : MonoBehaviour, IPlayerActions
    {
        [SerializeField]
        private PlayerConfig _playerConfig;
    
        [SerializeField] private Rigidbody2D playerRigidbody;

        protected PlayerControls _playerControl;

        protected Action moveAction, jumpAction, lookAction;

        private Vector2 _moveDirection, _lookDirection;

        private PlayerStateBase _currentPlayerState, _movePlayerState, _jumPlayerState, _idlePlayerState;

        public PlayerStateBase CurrentPlayerState => _currentPlayerState;

        private void OnEnable()
        {
            _playerControl = new PlayerControls();
            _playerControl.Player.SetCallbacks(this);
            _playerControl.Player.Move.Enable();
            _playerControl.Player.Look.Enable();
            RegisterControlEvents();
        }

        private void OnDisable()
        {
            _playerControl.Player.Move.Disable();
            _playerControl.Player.Look.Disable();
        }


        public void OnMove(InputAction.CallbackContext context)
        {
            _moveDirection = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            _lookDirection = context.ReadValue<Vector2>();
        }

        public void MovePlayer()
        {
            moveAction?.Invoke();
        }

        public void PlayerLook()
        {
            lookAction?.Invoke();
        }

        public void PlayerJump()
        {
            jumpAction?.Invoke();
        }

        private void RegisterControlEvents()
        {
            moveAction = () => 
            {
                playerRigidbody.AddForce(_moveDirection * _playerConfig.MovementSpeed * Time.deltaTime, ForceMode2D.Force);
            };
            jumpAction = () =>
            {
                playerRigidbody.AddForce(Vector2.up * _playerConfig.JumpForce, ForceMode2D.Impulse);
            };
        }
    }

}
