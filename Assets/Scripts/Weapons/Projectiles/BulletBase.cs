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
        [SerializeField] private Rigidbody2D _myRigidbody2D;
        [SerializeField] private TrailRenderer _myTrailRenderer;
        public Action OnBulletDeactivate;
        [SerializeField] private float _assignedDamageValue;
        [SerializeField] private LayerMask _deactivateLayers;

        private WeaponBase _ownerWeapon;
        private CharacterBase _ownerCharacter;

        public WeaponBase OwnerWeapon {
            get => _ownerWeapon;
            set => _ownerWeapon = value;
        }
        public CharacterBase OwnerCharacter {
            get => _ownerCharacter;
            set => _ownerCharacter = value;
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

		private void OnTriggerEnter2D(Collider2D collision)
		{
            if (_deactivateLayers == (_deactivateLayers | (1 << collision.gameObject.layer)))
			{
                CharacterBase character = collision.gameObject.GetComponent<CharacterBase>();
                if (character != null && character == _ownerCharacter) return; 
                if(character != null)
				{
                    character.TakeDamage(_assignedDamageValue, transform.right * OwnerWeapon.WeaponStats.PushbackIntensity);
				}
                OnBulletDeactivate?.Invoke();
            }
        }

        public void InitializeProjectile(Action OnDeactivateAction ,float maxLifetime)
		{
            _myTrailRenderer.gameObject.SetActive(true);
            OnBulletDeactivate = () =>
            {
                _myTrailRenderer.gameObject.SetActive(false);
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

        private IEnumerator CheckLifetime(float maxLifetime)
		{
            yield return new WaitForSeconds(maxLifetime);
            if(OnBulletDeactivate != null)
                OnBulletDeactivate?.Invoke();
		}
	}
}

