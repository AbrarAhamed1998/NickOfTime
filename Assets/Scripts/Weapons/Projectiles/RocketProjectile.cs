using NickOfTime.Characters;
using NickOfTime.Helper.Constants;
using NickOfTime.Managers;
using NickOfTime.ScriptableObjects.Weapons;
using NickOfTime.Utilities.PoolingSystem;
using NickOfTime.Weapons.Projectiles;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NickOfTime.Weapons.Projectiles
{
    public class RocketProjectile : BulletBase
    {
		public Action<Vector3> OnContactPoint;
        // Start is called before the first frame update
        protected override void Start()
        {

        }

        // Update is called once per frame
        protected override void Update()
        {

        }

		public override void InitializeProjectile(Action OnDeactivateAction, float maxLifetime)
		{
			base.InitializeProjectile(OnDeactivateAction, maxLifetime);
			OnContactPoint = (contactPoint) =>
			{
				SpawnExplosionAtPoint(contactPoint, maxLifetime);
			};
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
				OnContactPoint?.Invoke(transform.position);
				OnBulletDeactivate?.Invoke();
			}

			// Spawn explosion on collision with any object
		}

		public void SpawnExplosionAtPoint(Vector3 location, float maxLifetime)
		{
			PoolObject _explosionPoolObj = PersistentDataManager.instance.PoolManager.
				GetPoolObject(NickOfTimeStringConstants.ROCKET_EXPLOSION_VFX_POOL_ID, null);
			_explosionPoolObj.obj.transform.position = location;
			_explosionPoolObj.obj.GetComponent<ExplosionSphere>()
				.SetExplosionStats(
				((RocketLauncherStatsSO)OwnerWeapon.WeaponStats).ExplosionRadius,
				OwnerWeapon.WeaponStats.PushbackIntensity,
				((RocketLauncherStatsSO)OwnerWeapon.WeaponStats).ExplosionDamage);
			_explosionPoolObj.obj.GetComponent<ExplosionSphere>().TriggerExplosion();
			PersistentDataManager.instance
				.StartCoroutine(CheckLifetimeOfExplosion(maxLifetime, _explosionPoolObj));
		}

		IEnumerator CheckLifetimeOfExplosion(float maxLifetime, PoolObject poolObject)
		{
			yield return new WaitForSeconds(maxLifetime);
			PersistentDataManager.instance.PoolManager.ReturnObjectToPool(poolObject);
		}
	}
}

