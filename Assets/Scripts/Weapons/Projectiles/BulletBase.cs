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

        private bool ignoreRicochet = false;

        private Vector2 ricochetDirection;
        private float ricochetRotation;

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

		private void FixedUpdate()
		{
            if(!ignoreRicochet)
                CheckForRicohet();
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
            if (_deactivateLayers == (_deactivateLayers | (1 << collision.gameObject.layer)))
			{
                CharacterBase character = collision.gameObject.GetComponent<CharacterBase>();
                if (character != null && character == _ownerCharacter) return;
                ignoreRicochet = true;
                if(character != null)
				{
                    character.TakeDamage(_assignedDamageValue, transform.right * OwnerWeapon.WeaponStats.PushbackIntensity);
				}
                if (ricochetDirection != Vector2.zero)
				{
                    StartCoroutine(RicochetSet());
                }
                //OnBulletDeactivate?.Invoke();
            }
        }

        public void InitializeProjectile(Action OnDeactivateAction ,float maxLifetime)
		{
            _myTrailRenderer.gameObject.SetActive(true);
            OnBulletDeactivate = () =>
            {
                _myTrailRenderer.gameObject.SetActive(false);
                ignoreRicochet = false;
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

        public void CheckForRicohet()
		{
            //Check for ricochet
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, float.MaxValue, _deactivateLayers);
            
            float angleOfIncidence = 180f - Vector2.Angle(transform.right, hit.normal);
            float angleTOSurface = 90 - angleOfIncidence;
            Debug.Log($"angle of incidence : {angleOfIncidence}");
            ricochetDirection = Vector2.Reflect(((Vector2)transform.position - hit.point).normalized , hit.normal);
            //ricochetRotation = 2f * angleTOSurface;
            ricochetRotation = 90 - Mathf.Atan2(ricochetDirection.y, ricochetDirection.x) * Mathf.Rad2Deg;
            //ignoreRicochet = true;
            Debug.Log($"ricochet rotation : {ricochetRotation}");
            Debug.DrawRay(transform.position, _myRigidbody2D.velocity.normalized, Color.red, 5f);
        }

        private IEnumerator CheckLifetime(float maxLifetime)
		{
            yield return new WaitForSeconds(maxLifetime);
            if(OnBulletDeactivate != null)
                OnBulletDeactivate?.Invoke();
		}

        private IEnumerator RicochetSet()
		{
            //transform.localEulerAngles += new Vector3(0f, 0f, ricochetRotation);
            _myRigidbody2D.velocity *= ricochetDirection;
            yield return new WaitForEndOfFrame();
            ignoreRicochet = false;
        }
	}
}

