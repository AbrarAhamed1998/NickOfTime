using DG.Tweening;
using NickOfTime.Characters.Player.PlayerStates;
using NickOfTime.Helper.Constants;
using NickOfTime.Managers;
using NickOfTime.ScriptableObjects.Player;
using NickOfTime.UI.DialogSystem;
using NickOfTime.Utilities.PoolingSystem;
using NickOfTime.Weapons;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static PlayerControls;


namespace NickOfTime.Characters.Player
{
	public class Player : CharacterBase, IPlayerActions
    {
        protected PlayerStateBase _movePlayerState, _jumpPlayerState, _idlePlayerState, _deathPlayerState;
		#region PROPERTIES
		public PlayerStateBase CurrentPlayerState {
            get => (PlayerStateBase)CurrentCharacterState;
			set
			{
                CurrentCharacterState = value;
			}
        }

        protected PlayerConfig _playerConfig => (PlayerConfig)_characterConfig;
 
        public DialogPlayer DialogPlayer => _dialogPlayer;

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
            CharacterHealthPoints = _characterConfig.DefaultHealthPoints;
            _movePlayerState = new MovePlayerState(this);
            _jumpPlayerState = new JumpPlayerState(this);
            _idlePlayerState = new IdlePlayerState(this);
            _deathPlayerState = new DeathPlayerState(this);
            CurrentPlayerState = _idlePlayerState;
            PersistentDataManager.instance.ActivePlayer = this;
            _dialogPlayer.PlayAssignedDialogSet(0);
            StartCoroutine(RegisterUI());
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

        protected override void RegisterControlEvents()
        {
            base.RegisterControlEvents();
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
                ChangeToDamageSprite();
                DamageFlash();
                DamagePushBack(direction);
                CheckForCharacterDeath();
            };

            onCharacterDeath = () =>
            {
                OnDeath();
            };
        }

        protected override void DeregisterControlEvents()
		{
            base.DeregisterControlEvents();
		}

        protected override void LookAtScreenPos()
        {
            base.LookAtScreenPos();
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(_lookTargetScreenPos);
            //Vector2 worldPos = (Vector2)transform.position + (_lookTargetScreenPos * 10f);
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

        protected override void UseWeapon()
		{
            if (_equippedWeapon != null)
                _equippedWeapon.UseWeapon();
		}

        protected override void OnDeath()
        {
            base.OnDeath();
            Vector3 targetBodyRot = new Vector3(transform.localEulerAngles.x,
                transform.localEulerAngles.y,
                _playerConfig.DeathBodyZRot);
            Vector3 targetHeadRot = new Vector3(_debugLookObjects[0].transform.localEulerAngles.x,
                _debugLookObjects[0].transform.localEulerAngles.y,
                _playerConfig.DeathHeadZRot);
            
            transform.DOLocalRotate(targetBodyRot, 0.25f).OnComplete(
                () => 
                {
                    //lookAction = null;
                    _debugLookObjects[0].transform.DOLocalRotate(targetHeadRot, 0.25f); 
                });
            PoolObject bleedVFX = PersistentDataManager.instance.PoolManager
                .GetPoolObject(NickOfTimeStringConstants.EFFECT_BLOODPOOL_POOL_ID, _deathVFXTransform);
            StartCoroutine(HandleDeathVFX(bleedVFX));
            StartCoroutine(DeathRoutine());

        }

		protected override void JetControl(Vector2 direction)
		{
			base.JetControl(direction);
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
                _jetTransforms[i].localRotation = Quaternion.Slerp(_jetTransforms[i].localRotation, targetQuaternion, _playerConfig.JetPackRotSpeed * Time.deltaTime);
            }
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

		public override void CharacterDeath()
		{
            base.CharacterDeath();
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

		public override void CheckForCharacterDeath()
		{
			base.CheckForCharacterDeath();
            if (CharacterHealthPoints <= 0f)    
                ChangeCharacterState(_deathPlayerState);
		}

		public override void EquipWeapon(WeaponBase weapon)
		{
            base.EquipWeapon(weapon);
            weapon.SetProjectleLayer(true);
		}


		#endregion

		#region IENUMERATOR

        IEnumerator RegisterUI()
		{
            yield return new WaitUntil(() => PersistentDataManager.instance != null);
            _characterHealthSlider = PersistentDataManager.instance.UIManager.PlayerHealthBar;
        }

        IEnumerator HandleDeathVFX(PoolObject poolObject)
		{
            yield return new WaitForSeconds(_playerConfig.DeathEffectTime);
            PersistentDataManager.instance.PoolManager.ReturnObjectToPool(poolObject);
		}

        IEnumerator DeathRoutine()
		{
            yield return new WaitForSeconds(1f);
            _uiManager.OnPlayerDeath?.Invoke();
		}
        #endregion
    }

}
