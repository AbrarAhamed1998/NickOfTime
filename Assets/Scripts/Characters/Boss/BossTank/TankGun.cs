using NickOfTime.Characters;
using NickOfTime.Managers;
using NickOfTime.ScriptableObjects.Enemy;
using NickOfTime.Utilities.PoolingSystem;
using NickOfTime.Weapons.Projectiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NickOfTime.Enemy
{
	public class TankGun : MonoBehaviour
	{
		[SerializeField] private BossTank _bossTank;
		[SerializeField] private Transform _tankBarrel;
		[SerializeField, Layer] private int _projectileLayer;

		public TankStats TankStats => _bossTank.TankStats;
		public Transform TankGunBarrel => _tankBarrel;

		private bool _isReloading;

		public void OnUseGun()
		{
			if (_isReloading) return;
			FireWeapon(TankStats.TankRoundPoolID);
		}

		private void FireWeapon(string poolID)
		{
			PoolObject bulletPoolObject = PersistentDataManager.instance.PoolManager
				.GetPoolObject(poolID, null);
			TankRound bullet = bulletPoolObject.obj.GetComponent<TankRound>();
			bullet.SetDamageValue(TankStats.HitDamage);
			bullet.gameObject.SetActive(false);
			bullet.OwnerGun = this;
			bullet.gameObject.layer = _projectileLayer;
			bullet.transform.position = _tankBarrel.position;
			bullet.transform.rotation = _tankBarrel.rotation;

			bullet.gameObject.SetActive(true);

			bullet.InitializeProjectile(() => {
				PersistentDataManager.instance.PoolManager.ReturnObjectToPool(bulletPoolObject);
			},
			TankStats.TankRoundLifetime);
			bullet.GetComponent<Rigidbody2D>().AddForce(_tankBarrel.right * TankStats.LaunchForce, ForceMode2D.Impulse);
		}

		private IEnumerator ReloadingCoroutine()
		{
			yield return null;
		}
	}
}

