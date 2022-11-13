using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Weapons
{
    public class WeaponBase : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _mySpriteRenderer;
		[SerializeField] private Rigidbody2D _myRigidbody;

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {

        }
        protected virtual void OnPickUp()
		{

		}

		protected virtual void OnDrop()
		{

		}

		protected virtual void OnUseWeapon()
		{

		}
    }
}

