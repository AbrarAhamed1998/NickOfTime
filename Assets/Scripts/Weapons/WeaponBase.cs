using NickOfTime.Characters;
using NickOfTime.Characters.Player;
using NickOfTime.ScriptableObjects.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Weapons
{
    public class WeaponBase : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _mySpriteRenderer;
		[SerializeField] private Collider2D _myCollider;
		[SerializeField] private Collider2D _pickupCollider;
		[SerializeField] private Rigidbody2D _myRigidbody;
		[SerializeField] protected WeaponStatsSO _weaponStatsSO;

		[SerializeField, Layer] private int _playerLayer;


		public SpriteRenderer ItemSpriteRenderer => _mySpriteRenderer;
		
        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {

        }

		protected virtual void InitializePool()
		{

		}

        protected virtual void OnPickUp()
		{
            _myCollider.enabled = false;  
			_pickupCollider.enabled = false;
		}

		protected virtual void OnDrop()
		{
            _myCollider.enabled = true;
			_pickupCollider.enabled = true;
		}

		protected virtual void OnUseWeapon()
		{

		}

		public virtual void UseWeapon()
		{
			OnUseWeapon();
		}

		protected void OnTriggerEnter2D(Collider2D collision)
		{
			if((_weaponStatsSO.PickupMask & 1<< collision.gameObject.layer) != 0)
			{
				CharacterBase _character = collision.GetComponent<CharacterBase>();
				if(_character != null)
				{
					OnPickUp();
					_character.EquipWeapon(this);
				}
			}
		}
	}
}

