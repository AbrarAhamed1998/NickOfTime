using DG.Tweening;
using NickOfTime.Characters.CharacterStates;
using NickOfTime.Managers;
using NickOfTime.ScriptableObjects.Characters;
using NickOfTime.UI;
using NickOfTime.UI.DialogSystem;
using NickOfTime.Weapons;
using System;
using System.Collections;
using UnityEngine;

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

        [SerializeField] protected Transform _uiRoot;
		[SerializeField] protected DialogPlayer _dialogPlayer;

        [SerializeField] protected bool IsGrounded;

        [SerializeField] protected Transform _groundCheckBox;

        [SerializeField] protected WeaponBase _equippedWeapon;

        [SerializeField] protected float _characterHealthPoints;
        [SerializeField] protected HealthSliderBase _characterHealthSlider;

        [SerializeField] protected Transform _deathVFXTransform;
        protected PlayerControls _playerControl;

        protected Action moveAction, jumpAction, lookAction, fireAction, jetRotateAction, onCharacterDeath;
        protected Action<float,Vector2> takeDamage;

        protected Vector2 _moveDirection, _lookTargetScreenPos;

        protected CharacterStateBase _currentCharacterState;

        protected ParticleSystem[] _jetParticleSystem = new ParticleSystem[2];

        protected GameUIManager _uiManager => PersistentDataManager.instance.UIManager;

        #region PROPERTIES
        public CharacterStateBase CurrentCharacterState
        {
            get => _currentCharacterState;
            set
            {
                _currentCharacterState = value;
            }
        }
        public float CharacterHealthPoints
		{
            get => _characterHealthPoints;
            set => _characterHealthPoints = value;
		}

        public DialogPlayer DialogPlayer => _dialogPlayer;

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
            CharacterHealthPoints -= damage;
            _characterHealthSlider.SetHealthSliderVal(CharacterHealthPoints / _characterConfig.DefaultHealthPoints);
		}

        protected virtual void DamageFlash()
		{
            StartCoroutine(DamageFlashRoutine());
		}

        protected virtual void DamagePushBack(Vector2 direction)
		{
            //Debug.Log($"pushback dir {direction}");
            _characterRigidbody.AddForce(direction, ForceMode2D.Impulse);
		}

        protected virtual void RegisterControlEvents()
        {
			
        }

		protected virtual void JetControl(Vector2 direction)
		{
			
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
            
        }

        protected virtual void LookAtWorldPos(Transform targetWorldTransform)
		{
           
        }

        protected virtual void UseWeapon()
        {
            if (_equippedWeapon != null)
                _equippedWeapon.UseWeapon();
        }

        protected virtual void OnDeath()
		{
            
		}

        protected virtual void ChangeToDamageSprite()
		{
            int totalCount = _characterConfig.DamageSprites.Length;
            int maxIndex = totalCount - 1;

            float result = CharacterHealthPoints / _characterConfig.DamageSpriteFactor;
            int resultIndex = Mathf.Clamp( totalCount - (Mathf.RoundToInt(result)), 0, maxIndex);

            SwapSprites(resultIndex);
		}

        protected virtual void SwapSprites(int damageIndex)
		{
            _characterSprite.sprite = _characterConfig.DamageSprites[damageIndex].BodySprite;
            _debugLookObjects[0].GetComponent<SpriteRenderer>().sprite
                = _characterConfig.DamageSprites[damageIndex].HeadSprite;
        }

        protected virtual void OnDisplayDialog()
		{

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

        public virtual void CharacterDeath()
		{
            onCharacterDeath?.Invoke();
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

        public void Pushback(Vector2 direction)
		{
            DamagePushBack(direction);
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

        public virtual void CheckForCharacterDeath()
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
	[Serializable]
    public class DamageSpriteSetBase
	{
        public Sprite HeadSprite;
        public Sprite BodySprite;
	}

}

