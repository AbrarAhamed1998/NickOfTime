using NickOfTime.Characters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Weapons.Projectiles
{
    public class RicochetBullet : BulletBase
    {
        private bool ignoreRicochet = false;

        private Vector2 ricochetDirection;
        private float ricochetRotation;

		protected override void FixedUpdate()
		{
			base.FixedUpdate();
            if(!ignoreRicochet)
                CheckForRicohet();
		}

		protected override void OnTriggerEnter2D(Collider2D collision)
		{
            if (_deactivateLayers == (_deactivateLayers | (1 << collision.gameObject.layer)))
            {
                CharacterBase character = collision.gameObject.GetComponent<CharacterBase>();
                if (character != null && character == _ownerCharacter) return;
                //ignoreRicochet = true;
                if (character != null)
                {
                    character.TakeDamage(_assignedDamageValue, transform.right * OwnerWeapon.WeaponStats.PushbackIntensity);
                }
                if (ricochetDirection != Vector2.zero)
                {
                    transform.eulerAngles = new Vector3(0f, 0f, ricochetRotation);
                    _myRigidbody2D.velocity = ricochetDirection * _myRigidbody2D.velocity.magnitude;
                    Debug.Log($"ricochet set as : {ricochetDirection}, rotation : {ricochetRotation}");
                    //ignoreRicochet = false;
                    //StartCoroutine(RicochetSet());
                }
            }
            
        }

		public override void InitializeProjectile(Action OnDeactivateAction, float maxLifetime)
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

		public void CheckForRicohet()
        {
            //Check for ricochet
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, float.MaxValue, _deactivateLayers);

            float angleOfIncidence = 180f - Vector2.Angle(transform.right, hit.normal);
            float angleTOSurface = 90 - angleOfIncidence;
            //Debug.Log($"angle of incidence : {angleOfIncidence}");
            ricochetDirection = Vector2.Reflect(_myRigidbody2D.velocity.normalized, hit.normal);
            //ricochetRotation = 2f * angleTOSurface;

            float switchFactorX = ricochetDirection.x < 0 ? -1f : 1f;
            float switchFactorY = ricochetDirection.y < 0 ? -1f : 1f;

            float totalFactor = switchFactorY;

            //ricochetRotation *= totalFactor;
            ricochetRotation = Mathf.Atan2(ricochetDirection.y, ricochetDirection.x) * Mathf.Rad2Deg;
            //ignoreRicochet = true;
            //Debug.Log($"ricochet rotation : {ricochetRotation}");
            Debug.DrawRay(transform.position, _myRigidbody2D.velocity.normalized, Color.red, 5f);
        }

        protected IEnumerator RicochetSet()
        {
            //transform.localEulerAngles += new Vector3(0f, 0f, ricochetRotation);

            yield return new WaitForEndOfFrame();
            ignoreRicochet = false;
        }
    }
}

