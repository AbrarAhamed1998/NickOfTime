using NickOfTime.Characters;
using NickOfTime.Enemy;
using NickOfTime.Helper.Constants;
using NickOfTime.Managers;
using NickOfTime.ScriptableObjects.Enemy;
using NickOfTime.Utilities.PoolingSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Weapons.Projectiles
{
    public class TankRound : MonoBehaviour
    {
        [SerializeField] protected Rigidbody2D _myRigidbody2D;
        [SerializeField] protected TrailRenderer _myTrailRenderer;
        public Action OnTankRoundDeactivate;
        public Action<Vector3> OnContactPoint;
        [SerializeField] protected float _assignedDamageValue;
        [SerializeField] protected LayerMask _deactivateLayers;

        protected TankGun _ownerGun;

        public TankGun OwnerGun
		{
            get => _ownerGun;
            set => _ownerGun = value;
		}


        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (_deactivateLayers == (_deactivateLayers | (1 << collision.gameObject.layer)))
            {
                CharacterBase character = collision.gameObject.GetComponent<CharacterBase>();
                if (character != null) return;
                //ignoreRicochet = true;
                if (character != null)
                {
                    character.TakeDamage(_assignedDamageValue, transform.right * OwnerGun.TankStats.HitPushbackIntensity);
                }
                OnContactPoint?.Invoke(transform.position);
                //ricochetDirection = Vector2.Reflect(_myRigidbody2D.velocity.normalized, collision.GetContacts());
                OnTankRoundDeactivate?.Invoke();
            }
        }

        public void SpawnExplosionAtPoint(Vector3 location, float maxLifetime)
        {
            PoolObject _explosionPoolObj = PersistentDataManager.instance.PoolManager.
                GetPoolObject(NickOfTimeStringConstants.ROCKET_EXPLOSION_VFX_POOL_ID, null);
            _explosionPoolObj.obj.transform.position = location;
            _explosionPoolObj.obj.GetComponent<ExplosionSphere>()
                .SetExplosionStats(
                OwnerGun.TankStats.ExplosionRadius,
                OwnerGun.TankStats.HitPushbackIntensity,
                OwnerGun.TankStats.ExplosionDamage);
            _explosionPoolObj.obj.GetComponent<ExplosionSphere>().TriggerExplosion();
            PersistentDataManager.instance
                .StartCoroutine(CheckLifetimeOfExplosion(maxLifetime, _explosionPoolObj));
        }

        public virtual void InitializeProjectile(Action OnDeactivateAction, float maxLifetime)
        {
            _myTrailRenderer.gameObject.SetActive(true);
            OnTankRoundDeactivate = () =>
            {
                _myTrailRenderer.gameObject.SetActive(false);
                //ignoreRicochet = false;
                OnDeactivateAction?.Invoke();
                OnTankRoundDeactivate = null;
                StopCoroutine(CheckLifetime(maxLifetime));
            };
            StartCoroutine(CheckLifetime(maxLifetime));
            OnContactPoint = (contactPoint) =>
            {
                SpawnExplosionAtPoint(contactPoint, OwnerGun.TankStats.ExplosionLifetime);
            };
        }

        public void SetDamageValue(float value)
        {
            _assignedDamageValue = value;
        }

        protected IEnumerator CheckLifetime(float maxLifetime)
        {
            yield return new WaitForSeconds(maxLifetime);
            if (OnTankRoundDeactivate != null)
                OnTankRoundDeactivate?.Invoke();
        }
        IEnumerator CheckLifetimeOfExplosion(float maxLifetime, PoolObject poolObject)
        {
            yield return new WaitForSeconds(maxLifetime);
            PersistentDataManager.instance.PoolManager.ReturnObjectToPool(poolObject);
        }
    }
}

