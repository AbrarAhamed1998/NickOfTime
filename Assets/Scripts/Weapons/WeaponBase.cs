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

		[SerializeField, Layer] private int _playerLayer;

		public SpriteRenderer ItemSpriteRenderer => _mySpriteRenderer;
		
        protected virtual void Start()
        {

        }

        protected virtual void Update()
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
			if(collision.gameObject.layer == _playerLayer)
			{
				Player.Player _player = collision.GetComponent<Player.Player>();
				OnPickUp();
				_player.EquipWeapon(this);
			}
		}
	}
}

