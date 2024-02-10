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
		#region SERIALIZED PROTECTED FIELDS

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

		#endregion

		#region PROTECTED FIELDS

		protected PlayerControls _playerControl;

        protected Action moveAction, jumpAction, lookAction, fireAction, jetRotateAction, onCharacterDeath;
        protected Action<float,Vector2> takeDamage;

        protected Vector2 _moveDirection, _lookTargetScreenPos;

        protected CharacterStateBase _currentCharacterState;

        protected ParticleSystem[] _jetParticleSystem = new ParticleSystem[2];

        protected GameUIManager _uiManager => PersistentDataManager.instance.UIManager;

		#endregion

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

        #region PROTECTED VIRTUAL METHODS

        /// <summary>
        /// Changes the Current State of the Character to <paramref name="state"/>.
        /// </summary>
        /// <param name="state"></param>
        protected virtual void ChangeCharacterState(CharacterStateBase state)
        {
            if (CurrentCharacterState == state) return;
            CurrentCharacterState?.OnStateExit();
            CurrentCharacterState = state;
            CurrentCharacterState?.OnStateEnter();
        }

        /// <summary>
        /// Negates <paramref name="damage"/> from this Characters Health Points.
        /// <br>Updates the Health Slider if this character has one.</br>
        /// </summary>
        /// <param name="damage"></param>
        protected virtual void NegateDamageFromHealth(float damage)
		{
            CharacterHealthPoints -= damage;
            if (_characterHealthSlider == null) return;
            _characterHealthSlider.SetHealthSliderVal(CharacterHealthPoints / _characterConfig.DefaultHealthPoints);
		}

        /// <summary>
        /// Starts a Coroutine which plays a colored flash on the Character when they are hit.
        /// </summary>
        protected virtual void DamageFlash()
		{
            StartCoroutine(DamageFlashRoutine());
		}

        /// <summary>
        /// Adds a pushback force of <paramref name="direction"/> which is a Vector2 that holds the direction and 
        /// magnitude of the pushback force to be applied on the character.
        /// </summary>
        /// <param name="direction"></param>
        protected virtual void DamagePushBack(Vector2 direction)
		{
            //Debug.Log($"pushback dir {direction}");
            _characterRigidbody.AddForce(direction, ForceMode2D.Impulse);
		}

        /// <summary>
        /// Blank virtual method to register controls to this CharacterBase.
        /// <br>This can be a Player Input or AI.</br>
        /// </summary>
        protected virtual void RegisterControlEvents()
        {
			
        }

        /// <summary>
        /// Blank virtual method for Jet Control as multiple characters can use jets for traversal. 
        /// </summary>
        /// <param name="direction"></param>
		protected virtual void JetControl(Vector2 direction)
		{
			
		}

        /// <summary>
        /// Deregisters all control events on this Character.
        /// </summary>
		protected virtual void DeregisterControlEvents()
        {
            moveAction = null;
            jumpAction = null;
            lookAction = null;
            fireAction = null;
        }

        /// <summary>
        /// Looks at a Set Screen position.
        /// </summary>
        protected virtual void LookAtScreenPos()
        {
            
        }

        /// <summary>
        /// Looks at the world postion of Transform <paramref name="targetWorldTransform"/>.
        /// </summary>
        /// <param name="targetWorldTransform"></param>
        protected virtual void LookAtWorldPos(Transform targetWorldTransform)
		{
           
        }

        /// <summary>
        /// Uses the equipped weapon.
        /// </summary>
        protected virtual void UseWeapon()
        {
            if (_equippedWeapon != null)
                _equippedWeapon.UseWeapon();
        }

        /// <summary>
        /// Called when this Characters Health hits 0.
        /// </summary>
        protected virtual void OnDeath()
		{
            
		}

        /// <summary>
        /// Swaps the Character's sprites for a more damaged version when player loses health 
        /// beyond certain thresholds.
        /// </summary>
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

        /// <summary>
        /// Used by Character States to Execute Movement.
        /// </summary>
        public virtual void CharacterMove()
        {
            moveAction?.Invoke();
        }

		/// <summary>
		/// Used by Character States to Execute Looking.
		/// </summary>
		public virtual void CharacterLook()
        {
            lookAction?.Invoke();
        }

		/// <summary>
		/// Used by Character States to Execute Jumping.
		/// </summary>
		public virtual void CharacterJump()
        {
            jumpAction?.Invoke();
        }

		/// <summary>
		/// Used by Character States to Execute WeaponUsage.
		/// </summary>
		public virtual void CharacterUseWeapon()
        {
            fireAction?.Invoke();
        }

		/// <summary>
		/// Used by Character States to Execute Character Death.
		/// </summary>
		public virtual void CharacterDeath()
		{
            onCharacterDeath?.Invoke();
		}

        /// <summary>
        /// Universal Check to see if the Character is on the ground.
        /// </summary>
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
        /// <summary>
        /// Public method to make DamagePushback accessible.
        /// </summary>
        /// <param name="direction"></param>
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

        /// <summary>
        /// Equips a <paramref name="weapon"/> to this Character. 
        /// </summary>
        /// <param name="weapon"></param>
        public virtual void EquipWeapon(WeaponBase weapon)
        {
            if (_equippedWeapon != null) return;
            _equippedWeapon = weapon;
            weapon.transform.SetParent(_armParent);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
        }

        /// <summary>
        /// Public method that allows us to access methods responsible for applying damage.
        /// </summary>
        /// <param name="damageValue"></param>
        /// <param name="direction"></param>
        public virtual void TakeDamage(float damageValue, Vector2 direction)
		{
            takeDamage?.Invoke(damageValue,direction);
            CurrentCharacterState?.OnCharacterTakeDamage();
		}

		#endregion

		#region IENUMERATOR

        /// <summary>
        /// A coroutine that plays a flash animation to indicate the player has been damaged or hit.
        /// </summary>
        /// <returns></returns>
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

