using NickOfTime.Helper.Constants;
using NickOfTime.Utilities.PoolingSystem;
using NickOfTime.Weapons;
using NickOfTime.Weapons.Projectiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcelotRevolver : WeaponBase
{
    [SerializeField] private bool _reloading;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

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
        FireProjectile(NickOfTimeStringConstants.RICOCHET_BULLET_POOL_ID);
	}


}
