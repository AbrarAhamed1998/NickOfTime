using NickOfTime.Helper.Constants;
using NickOfTime.ScriptableObjects.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NickOfTime.Weapons
{
    public class OneLinerRocket : WeaponBase
    {
        private Coroutine _fireRoutine;
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

		protected override void OnUseWeapon()
		{
			base.OnUseWeapon();
            if(_fireRoutine == null)
                _fireRoutine = StartCoroutine(RocketFireProcedure());
		}

        protected IEnumerator RocketFireProcedure()
		{
            yield return new WaitForEndOfFrame();
            WeaponOwner.DialogPlayer.AssignDialogSet(
                ((RocketLauncherStatsSO)WeaponStats).OneLinerDialogSet);
            WeaponOwner.DialogPlayer.PlayAssignedDialogSet(
                () =>
				{
                    FireProjectile(NickOfTimeStringConstants.ROCKET_PROJECTILE_POOL_ID);
                    _fireRoutine = null;
                });
		}
	}
}

