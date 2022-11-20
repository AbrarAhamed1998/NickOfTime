using NickOfTime.Helper.Constants;
using NickOfTime.Managers;
using NickOfTime.Utilities.PoolingSystem;
using NickOfTime.Weapons.Projectiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Weapons
{
	public class Pistol : WeaponBase
	{
		[SerializeField] private Transform _barrel;

		protected override void InitializePool()
		{
			base.InitializePool();
		}

		protected override void OnPickUp()
		{
			base.OnPickUp();
		}

		protected override void OnUseWeapon()
		{
			base.OnUseWeapon();
			PoolObject bulletPoolObject = PersistentDataManager.instance.PoolManager.GetPoolObject(NickOfTimeStringConstants.PISTOL_BULLET_POOL_ID, null);
			BulletBase bullet = bulletPoolObject.obj.GetComponent<BulletBase>();
			bullet.gameObject.SetActive(false);
			bullet.transform.position = _barrel.position;
			bullet.transform.rotation = _barrel.rotation;
			
			bullet.gameObject.SetActive(true);

			bullet.InitializeProjectile(() => {
				PersistentDataManager.instance.PoolManager.ReturnObjectToPool(bulletPoolObject);
			},
			_weaponStatsSO.MaxProjectileLifetime);
			bullet.GetComponent<Rigidbody2D>().AddForce(_barrel.right * _weaponStatsSO.ProjectileLaunchForce, ForceMode2D.Impulse);
		}
	}
}

