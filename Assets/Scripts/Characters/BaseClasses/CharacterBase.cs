using NickOfTime.Characters.CharacterStates;
using NickOfTime.Characters.Player.PlayerStates;
using NickOfTime.ScriptableObjects.Characters;
using NickOfTime.Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Characters
{
    public class CharacterBase : MonoBehaviour
    {
        [SerializeField]
        protected CharacterBaseConfigSO _characterConfig;

        [SerializeField] protected Rigidbody2D _playerRigidbody;
        [SerializeField] protected SpriteRenderer _playerSprite;
        [SerializeField] protected Transform _armParent;

        [SerializeField] protected GameObject[] _debugLookObjects;
        [SerializeField] protected Transform[] _jetTransforms;
        [SerializeField] protected List<SpriteRenderer> _childSpritesToFlip;

        [SerializeField] protected bool IsGrounded;

        [SerializeField] protected Transform _groundCheckBox;

        [SerializeField] protected WeaponBase _equippedWeapon;

        protected PlayerControls _playerControl;

        protected Action moveAction, jumpAction, lookAction, fireAction, _jetRotateAction;

        protected Vector2 _moveDirection, _lookTargetScreenPos;

        protected CharacterStateBase _currentCharacterState;

        #region PROPERTIES
        public CharacterStateBase CurrentCharacterState
        {
            get => _currentCharacterState;
            set
            {
                _currentCharacterState = value;
            }
        }
        #endregion

        #region UNITY CALLBACKS

        protected virtual void OnEnable()
        {
            RegisterControlEvents();
        }

        protected virtual void OnDisable()
        {
            DeregisterControlEvents();
        }

        protected virtual void Start()
        {
			/*_movePlayerState = new MovePlayerState(this);
			_jumPlayerState = new JumpPlayerState(this);
			_idlePlayerState = new IdlePlayerState(this);*/
			CurrentCharacterState = null;
		}

        protected virtual void Update()
        {
            CurrentCharacterState?.OnStateUpdate();
        }

        protected virtual void FixedUpdate()
        {
            CurrentCharacterState?.OnStateFixedUpdate();
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = _characterConfig.GroundCheckBoxColor;
            Gizmos.DrawCube(_groundCheckBox.position, _characterConfig.GroundCheckBoxSize);
        }

        #endregion

        #region PRIVATE METHODS

        protected virtual void ChangePlayerState(PlayerStateBase state)
        {
            if (CurrentCharacterState == state) return;
            CurrentCharacterState?.OnStateExit();
            CurrentCharacterState = state;
            CurrentCharacterState?.OnStateEnter();
        }

        protected virtual void RegisterControlEvents()
        {
            moveAction = () =>
            {
                _playerRigidbody.AddForce(_moveDirection * _characterConfig.MovementSpeed * Time.deltaTime, ForceMode2D.Force);
                Vector3 target = (Vector2)_jetTransforms[0].position + (5f * _moveDirection);
                float y = _jetTransforms[0].position.y - target.y;
                float x = _jetTransforms[0].position.x - target.x;
                float targetAngle = (Mathf.Atan2(y, x) * Mathf.Rad2Deg) + (_moveDirection == Vector2.zero ? 0f : 90f);
                _jetTransforms[0].localEulerAngles = new Vector3(0f, 0f, targetAngle);
                for (int i = 0; i < _jetTransforms.Length; i++)
                {
                    _jetTransforms[i].localEulerAngles = _jetTransforms[0].localEulerAngles;
                }
            };
            jumpAction = () =>
            {
                _playerRigidbody.AddForce(Vector2.up * _characterConfig.JumpForce, ForceMode2D.Impulse);
            };
            lookAction = () =>
            {
                LookAtScreenPos();
            };
            fireAction = () =>
            {
                UseWeapon();
            };
        }

        protected virtual void DeregisterControlEvents()
        {
            moveAction = null;
            jumpAction = null;
            lookAction = null;
            fireAction = null;
        }

        protected virtual void LookAtScreenPos()
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(_lookTargetScreenPos);
            Transform target = _debugLookObjects[0].transform;
            float y = target.position.y - worldPos.y;
            float x = target.position.x - worldPos.x;
            float targetAngle = (Mathf.Atan2(y, x) * Mathf.Rad2Deg) + 180f;
            target.localEulerAngles = new Vector3(0f, 0f, targetAngle);
            for (int i = 1; i < _debugLookObjects.Length; i++)
            {
                _debugLookObjects[i].transform.localEulerAngles = target.localEulerAngles;
            }
            Vector2 mouseDirection = (worldPos - (Vector2)this.transform.position).normalized * Vector2.right;
            Vector2 playerdirection = Vector2.right * (_playerSprite.flipX ? -1f : 1f);
            float dotProduct = Vector2.Dot(mouseDirection, playerdirection);
            if (dotProduct < 0f)
            {
                _playerSprite.flipX = !_playerSprite.flipX;
                for (int i = 0; i < _debugLookObjects.Length; i++)
                {
                    SpriteRenderer spriteRenderer = _debugLookObjects[i].GetComponent<SpriteRenderer>();
                    spriteRenderer.flipY = !spriteRenderer.flipY;
                    if (i < _childSpritesToFlip.Count)
                    {
                        spriteRenderer = _childSpritesToFlip[i];
                        spriteRenderer.flipY = !spriteRenderer.flipY;
                    }
                }
            }
        }

        protected virtual void UseWeapon()
        {
            if (_equippedWeapon != null)
                _equippedWeapon.UseWeapon();
        }

        #endregion

        #region PUBLIC METHODS

        public virtual void PlayerMove()
        {
            moveAction?.Invoke();
        }

        public virtual void PlayerLook()
        {
            lookAction?.Invoke();
        }

        public virtual void PlayerJump()
        {
            jumpAction?.Invoke();
        }

        public virtual void PlayerUseWeapon()
        {
            fireAction?.Invoke();
        }

        public virtual void CheckIfChracterInAir()
        {
            Collider2D[] colliders = new Collider2D[1];
            int results = Physics2D.OverlapBoxNonAlloc(_groundCheckBox.transform.position,
                _characterConfig.GroundCheckBoxSize,
                0f,
                colliders,
                _characterConfig.GroundCheckLayerMask
                );
            if (results > 0)
            {
                CharacterOnGround();
            }
            else
            {
                CharacterInAir();
            }
        }

        protected virtual void CharacterInAir()
		{
            IsGrounded = false;
		}

        protected virtual void CharacterOnGround()
		{
            IsGrounded = true;
		}

        public virtual void CheckIfCharacterMoving()
        {
            /*if (_playerRigidbody.velocity.x != 0f)
                ChangePlayerState(_movePlayerState);
            else
                ChangePlayerState(_idlePlayerState);*/
        }

        public virtual void EquipWeapon(WeaponBase weapon)
        {
            if (_equippedWeapon != null) return;
            _equippedWeapon = weapon;
            weapon.transform.SetParent(_armParent);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
            _childSpritesToFlip.Add(weapon.ItemSpriteRenderer);
        }

        #endregion
    }
}

