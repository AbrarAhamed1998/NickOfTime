using NickOfTime.ScriptableObjects.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static PlayerControls;


namespace NickOfTime.Player
{
    public class Player : MonoBehaviour, IPlayerActions
    {
        [SerializeField]
        private PlayerConfig _playerConfig;
    
        [SerializeField] private Rigidbody2D _playerRigidbody;
		[SerializeField] private SpriteRenderer _playerSprite;

        [SerializeField] private GameObject _debugLookObject;

        [SerializeField] private bool IsGrounded;

        [SerializeField] private Transform _groundCheckBox;

        protected PlayerControls _playerControl;

        protected Action moveAction, jumpAction, lookAction;

        private Vector2 _moveDirection, _lookTargetScreenPos;

        private PlayerStateBase _currentPlayerState, _movePlayerState, _jumPlayerState, _idlePlayerState;

        public PlayerStateBase CurrentPlayerState {
            get => _currentPlayerState;
			set
			{
                _currentPlayerState = value;
			}
        }
       

        private void OnEnable()
        {
            _playerControl = new PlayerControls();
            _playerControl.Player.SetCallbacks(this);
            _playerControl.Player.Move.Enable();
            _playerControl.Player.Look.Enable();
            _playerControl.Player.Jump.Enable();
            RegisterControlEvents();
        }

        private void OnDisable()
        {
            _playerControl.Player.Move.Disable();
            _playerControl.Player.Look.Disable();
            _playerControl.Player.Jump.Disable();
        }

		private void Start()
		{
			_movePlayerState = new MovePlayerState(this);
            _jumPlayerState = new JumpPlayerState(this);
            _idlePlayerState = new IdlePlayerState(this);
            CurrentPlayerState = _idlePlayerState;
		}

        void Update()
		{
            CurrentPlayerState?.OnStateUpdate();
		}

        void FixedUpdate()
		{
            CurrentPlayerState?.OnStateFixedUpdate();
		}


        private void ChangePlayerState(PlayerStateBase state)
		{
            if (CurrentPlayerState == state) return;
            CurrentPlayerState?.OnStateExit();
            CurrentPlayerState = state;
            CurrentPlayerState?.OnStateEnter();
		}

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
            if(buttonControl.wasPressedThisFrame)
                CurrentPlayerState?.OnPlayerJump();
        }
        public void PlayerMove()
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

        public void CheckIfPlayerInAir()
		{
            Collider2D[] colliders = new Collider2D[1];
            int results = Physics2D.OverlapBoxNonAlloc(_groundCheckBox.transform.position,
                _playerConfig.GroundCheckBoxSize,
                0f,
                colliders,
                _playerConfig.GroundCheckLayerMask
                );
            if (results > 0)
			{
                //Debug.Log($"hit the ground {colliders[0].name}");
                IsGrounded = true;
                ChangePlayerState(_idlePlayerState);
			}
            else
			{
                //Debug.Log("Off the Ground");
                IsGrounded=false;
                ChangePlayerState(_jumPlayerState);
			}
		}

        private void RegisterControlEvents()
        {
            moveAction = () => 
            {
                _playerRigidbody.AddForce(_moveDirection * _playerConfig.MovementSpeed * Time.deltaTime, ForceMode2D.Force);
            };
            jumpAction = () =>
            {
                _playerRigidbody.AddForce(Vector2.up * _playerConfig.JumpForce, ForceMode2D.Impulse);
            };
            lookAction = () =>
            {
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(_lookTargetScreenPos);
                Transform target = _debugLookObject.transform;
                float y = /*target.position.y > worldPos.y ? */target.position.y - worldPos.y /*: worldPos.y - target.position.y*/;
                float x = /*target.position.x > worldPos.x ?*/ target.position.x - worldPos.x /*: worldPos.x - target.position.x*/;
                float targetAngle = (Mathf.Atan2(y, x) * Mathf.Rad2Deg) + 180f;
                target.localEulerAngles = new Vector3(0f, 0f, targetAngle);

                //default direction of sprite is Vector2.right
                Vector2 mouseDirection = (worldPos - (Vector2)this.transform.position).normalized * Vector2.right;
                //Debug.Log($"mouse Direction : {mouseDirection}");
                Vector2 playerdirection = Vector2.right * (_playerSprite.flipX ? -1f : 1f);
                //Debug.Log($"player Direction : {playerdirection}");
                float dotProduct = Vector2.Dot(mouseDirection, playerdirection);
                //Debug.Log($"dot product = {dotProduct}");
                if(dotProduct < 0f)
                    _playerSprite.flipX = !_playerSprite.flipX;
            };
        }

		private void OnDrawGizmos()
		{
            Gizmos.color = _playerConfig.GroundCheckBoxColor;
            Gizmos.DrawCube(_groundCheckBox.position, _playerConfig.GroundCheckBoxSize);
		}

	}

}
