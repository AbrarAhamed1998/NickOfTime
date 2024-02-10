using NickOfTime.Characters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


namespace NickOfTime.Weapons.Projectiles
{
    public class BulletBase : MonoBehaviour
    {
        [SerializeField] protected Rigidbody2D _myRigidbody2D;
        [SerializeField] protected TrailRenderer _myTrailRenderer;
        public Action OnBulletDeactivate;
        [SerializeField] protected float _assignedDamageValue;
        [SerializeField] protected LayerMask _deactivateLayers;

        protected WeaponBase _ownerWeapon;
        protected CharacterBase _ownerCharacter;

        

        public WeaponBase OwnerWeapon {
            get => _ownerWeapon;
            set => _ownerWeapon = value;
        }
        public CharacterBase OwnerCharacter {
            get => _ownerCharacter;
            set => _ownerCharacter = value;
        }

        protected virtual void Start()
        {

        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }

		protected virtual void FixedUpdate()
		{
			
		}

		protected virtual void OnTriggerEnter2D(Collider2D collision)
		{
            if (_deactivateLayers == (_deactivateLayers | (1 << collision.gameObject.layer)))
			{
				CharacterBase character = collision.gameObject.GetComponent<CharacterBase>();
				BossTank bossTank = collision.gameObject.GetComponent<BossTank>();
				if (character != null && character == _ownerCharacter) return;
				//ignoreRicochet = true;
				ApplyDamageOnCharacter(character, bossTank);

				//ricochetDirection = Vector2.Reflect(_myRigidbody2D.velocity.normalized, collision.GetContacts());
				OnBulletDeactivate?.Invoke();
			}
		}

		

		/*private void OnCollisionEnter2D(Collision2D collision)
		{
            if (_deactivateLayers == (_deactivateLayers | (1 << collision.gameObject.layer)))
            {
                CharacterBase character = collision.gameObject.GetComponent<CharacterBase>();
                if (character != null && character == _ownerCharacter) return;
                ignoreRicochet = true;
                if (character != null)
                {
                    character.TakeDamage(_assignedDamageValue, transform.right * OwnerWeapon.WeaponStats.PushbackIntensity);
                }
                *//*if (ricochetDirection != Vector2.zero)
				{
                    StartCoroutine(RicochetSet());
                }*//*
                ricochetDirection = Vector2.Reflect(_myRigidbody2D.velocity, collision.contacts[0].normal);
                _myRigidbody2D.velocity = ricochetDirection;
                Debug.DrawRay(transform.position, _myRigidbody2D.velocity.normalized, Color.red, 5f);
                ignoreRicochet = false;
                //OnBulletDeactivate?.Invoke();
            }
        }*/

		public virtual void InitializeProjectile(Action OnDeactivateAction ,float maxLifetime)
		{
            _myTrailRenderer.gameObject.SetActive(true);
            OnBulletDeactivate = () =>
            {
                _myTrailRenderer.gameObject.SetActive(false);
                //ignoreRicochet = false;
                OnDeactivateAction?.Invoke();
                OnBulletDeactivate = null;
                StopCoroutine(CheckLifetime(maxLifetime));
            };
            StartCoroutine(CheckLifetime(maxLifetime));
		}

        public void SetDamageValue(float value)
		{
            _assignedDamageValue = value;
		}

        protected IEnumerator CheckLifetime(float maxLifetime)
		{
            yield return new WaitForSeconds(maxLifetime);
            if(OnBulletDeactivate != null)
                OnBulletDeactivate?.Invoke();
		}

		protected void ApplyDamageOnCharacter(CharacterBase character, BossTank bossTank)
		{
			if (character != null)
			{
				character.TakeDamage(_assignedDamageValue, transform.right * OwnerWeapon.WeaponStats.PushbackIntensity);
			}

			if (bossTank != null)
				bossTank.TakeDamage(_assignedDamageValue, transform.right * OwnerWeapon.WeaponStats.PushbackIntensity);
		}
	}
}

