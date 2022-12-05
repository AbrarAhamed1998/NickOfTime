using NickOfTime.Characters.CharacterStates;
using NickOfTime.Characters.Player.PlayerStates;
using NickOfTime.ScriptableObjects.Characters;
using NickOfTime.Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NickOfTime.Characters
{
    public class CharacterBase : MonoBehaviour
    {
        [SerializeField]
        protected CharacterBaseConfigSO _characterConfig;

        [SerializeField] protected Rigidbody2D _characterRigidbody;
        [SerializeField] protected SpriteRenderer _characterSprite;
        [SerializeField] protected Transform _armParent;

        [SerializeField] protected GameObject[] _debugLookObjects;
        [SerializeField] protected Transform[] _jetTransforms;

        [SerializeField] protected bool IsGrounded;

        [SerializeField] protected Transform _groundCheckBox;

        [SerializeField] protected WeaponBase _equippedWeapon;

        [SerializeField] protected float _playerHealthPoints;
        [SerializeField] protected Slider _characterHealthSlider;


        protected PlayerControls _playerControl;

        protected Action moveAction, jumpAction, lookAction, fireAction, _jetRotateAction;
        protected Action<float,Vector2> takeDamage;

        protected Vector2 _moveDirection, _lookTargetScreenPos;

        protected CharacterStateBase _currentCharacterState;

        protected ParticleSystem[] _jetParticleSystem = new ParticleSystem[2];
        

        #region PROPERTIES
        public CharacterStateBase CurrentCharacterState
        {
            get => _currentCharacterState;
            set
            {
                _currentCharacterState = value;
            }
        }
        public float PlayerHealthPoints
		{
            get => _playerHealthPoints;
            set => _playerHealthPoints = value;
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

        protected virtual void ChangeCharacterState(CharacterStateBase state)
        {
            if (CurrentCharacterState == state) return;
            CurrentCharacterState?.OnStateExit();
            CurrentCharacterState = state;
            CurrentCharacterState?.OnStateEnter();
        }

        protected virtual void NegateDamageFromHealth(float damage)
		{
            Debug.Log($"Player Health Points before deducting : {PlayerHealthPoints}");

            PlayerHealthPoints -= damage;
            _characterHealthSlider.value = PlayerHealthPoints / _characterConfig.DefaultHealthPoints;
		}

        protected virtual void DamageFlash()
		{
            StartCoroutine(DamageFlashRoutine());
		}

        protected virtual void DamagePushBack(Vector2 direction)
		{
            _characterRigidbody.AddForce(direction, ForceMode2D.Impulse);
		}

        protected virtual void RegisterControlEvents()
        {
			moveAction = () =>
			{
				_characterRigidbody.AddForce(_moveDirection * _characterConfig.MovementSpeed * Time.deltaTime, ForceMode2D.Force);
				JetControl(_moveDirection);
			};
            jumpAction = () =>
            {
                _characterRigidbody.AddForce(Vector2.up * _characterConfig.JumpForce, ForceMode2D.Impulse);
            };
            lookAction = () =>
            {
                LookAtScreenPos();
            };
            fireAction = () =>
            {
                UseWeapon();
            };
            takeDamage = (damage, direction) =>
            {
                NegateDamageFromHealth(damage);
                DamageFlash();
                DamagePushBack(direction);

            };
        }

		protected virtual void JetControl(Vector2 direction)
		{
			Vector3 target = (Vector2)_jetTransforms[0].position + (5f * direction);
			float y = _jetTransforms[0].position.y - target.y;
			float x = _jetTransforms[0].position.x - target.x;
			float localEulerY = transform.localEulerAngles.y;
			float trueYRot = localEulerY < 180f ? localEulerY : localEulerY - 360;
			float switchFactor = (trueYRot < 0 ? -1f : 1f);
			float targetAngle = (Mathf.Atan2(y, x) * Mathf.Rad2Deg) + (direction == Vector2.zero ? 0f : 90f);
			Vector3 targetEuler = new Vector3(0f, 0f, switchFactor * targetAngle);
            Quaternion targetQuaternion = Quaternion.Euler(targetEuler);
			for (int i = 0; i < _jetTransforms.Length; i++)
			{
				_jetTransforms[i].localRotation = Quaternion.Slerp(_jetTransforms[i].localRotation, targetQuaternion, _characterConfig.JetPackRotSpeed * Time.deltaTime);
			}
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
            float localEulerY = transform.localEulerAngles.y;
            float trueYRot = localEulerY < 180f ? localEulerY : localEulerY - 360; 
            float switchFactor = (trueYRot < 0 ? 0f : 180f);
            float targetAngle = (Mathf.Atan2(y, x) * Mathf.Rad2Deg) + switchFactor;
            target.localEulerAngles = new Vector3(0f, 0f, (switchFactor == 0f?-1f:1f) * targetAngle);
            for (int i = 1; i < _debugLookObjects.Length; i++)
            {
                _debugLookObjects[i].transform.localEulerAngles = target.localEulerAngles;
            }
            Vector2 mouseDirection = (worldPos - (Vector2)this.transform.position).normalized * Vector2.right;
            Vector2 playerdirection = transform.right.normalized;/*Vector2.right * (_playerSprite.flipX ? -1f : 1f)*/;
            float dotProduct = Vector2.Dot(mouseDirection, playerdirection);
            if (dotProduct < 0f)
            {
                //_playerSprite.flipX = !_playerSprite.flipX;
                transform.localEulerAngles += Vector3.up * 180f;
                /*for (int i = 0; i < _debugLookObjects.Length; i++)
                {
                    SpriteRenderer spriteRenderer = _debugLookObjects[i].GetComponent<SpriteRenderer>();
                    spriteRenderer.flipY = !spriteRenderer.flipY;
                    if (i < _childSpritesToFlip.Count)
                    {
                        spriteRenderer = _childSpritesToFlip[i];
                        spriteRenderer.flipY = !spriteRenderer.flipY;
                    }
                }*/
            }
        }

        protected virtual void LookAtWorldPos(Transform targetWorldTransform)
		{
            if (targetWorldTransform == null)
            {
                Debug.Log("target World Pos is null");
                return;
            }
            Vector2 worldPos = targetWorldTransform.position;
            Transform target = _debugLookObjects[0].transform;
            float y = target.position.y - worldPos.y;
            float x = target.position.x - worldPos.x;
            float localEulerY = transform.localEulerAngles.y;
            float trueYRot = localEulerY < 180f ? localEulerY : localEulerY - 360;
            float switchFactor = (trueYRot < 0 ? 0f : 180f);
            float targetAngle = (Mathf.Atan2(y, x) * Mathf.Rad2Deg) + switchFactor;
            target.localEulerAngles = new Vector3(0f, 0f, (switchFactor == 0f ? -1f : 1f) * targetAngle);
            for (int i = 1; i < _debugLookObjects.Length; i++)
            {
                _debugLookObjects[i].transform.localEulerAngles = target.localEulerAngles;
            }

            Vector2 mouseDirection = (worldPos - (Vector2)this.transform.position).normalized * Vector2.right;
            Vector2 playerdirection = transform.right.normalized;
            float dotProduct = Vector2.Dot(mouseDirection, playerdirection);
            if (dotProduct < 0f)
            {
                transform.localEulerAngles += Vector3.up * 180f;
            }
        }

        protected virtual void UseWeapon()
        {
            if (_equippedWeapon != null)
                _equippedWeapon.UseWeapon();
        }

        #endregion

        #region PUBLIC METHODS

        public virtual void CharacterMove()
        {
            moveAction?.Invoke();
        }

        public virtual void CharacterLook()
        {
            lookAction?.Invoke();
        }

        public virtual void CharacterJump()
        {
            jumpAction?.Invoke();
        }

        public virtual void CharacterUseWeapon()
        {
            fireAction?.Invoke();
        }

        public virtual void CheckIfCharacterInAir()
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
			
		}

        public virtual void EquipWeapon(WeaponBase weapon)
        {
            if (_equippedWeapon != null) return;
            _equippedWeapon = weapon;
            weapon.transform.SetParent(_armParent);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
        }

        public virtual void TakeDamage(float damageValue, Vector2 direction)
		{
            takeDamage?.Invoke(damageValue,direction);
            CurrentCharacterState?.OnCharacterTakeDamage();
		}

		#endregion

		#region IENUMERATOR

        protected virtual IEnumerator DamageFlashRoutine()
		{
            Color defaultColor = _characterSprite.color;
            _characterSprite.color = _characterConfig.DamageFlashColor;
            _debugLookObjects[0].GetComponent<SpriteRenderer>().color = _characterConfig.DamageFlashColor;
            yield return new WaitForSeconds(_characterConfig.DamageFlashTime);
            _characterSprite.color = defaultColor;
            _debugLookObjects[0].GetComponent<SpriteRenderer>().color = defaultColor;
        }

		#endregion
	}
}

