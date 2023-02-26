using NickOfTime.Helper.Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NickOfTime.Weapons
{
    public class OneLinerRocket : WeaponBase
    {
        // Start is called before the first frame update
        protected override void Start()
        {

        }

        // Update is called once per frame
        protected override void Update()
        {

        }

		protected override void OnUseWeapon()
		{
			base.OnUseWeapon();
            FireProjectile(NickOfTimeStringConstants.ROCKET_PROJECTILE_POOL_ID);
		}
	}
}

