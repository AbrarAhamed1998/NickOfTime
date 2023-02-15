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
			FireProjectile(NickOfTimeStringConstants.PISTOL_BULLET_POOL_ID);
		}

		
	}
}

