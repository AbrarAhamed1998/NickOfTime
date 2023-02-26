using NickOfTime.Weapons.Projectiles;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NickOfTime.Weapons.Projectiles
{
    public class RocketProjectile : BulletBase
    {
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
		}

		protected override void OnTriggerEnter2D(Collider2D collision)
		{
			base.OnTriggerEnter2D(collision);
			// Spawn explosion on collision with any object
		}

		public void SpawnExoplosionAtPoint(Vector3 location)
		{

		}
	}
}

