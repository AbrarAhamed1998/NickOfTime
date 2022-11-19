using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Weapons
{
    public class WeaponBase : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _mySpriteRenderer;
		[SerializeField] private Collider2D _myCollider;
		[SerializeField] private Rigidbody2D _myRigidbody;

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {

        }
        protected virtual void OnPickUp()
		{
            _myCollider.enabled = false;    
		}

		protected virtual void OnDrop()
		{
            _myCollider.enabled = true;
		}

		protected virtual void OnUseWeapon()
		{

		}
    }
}

