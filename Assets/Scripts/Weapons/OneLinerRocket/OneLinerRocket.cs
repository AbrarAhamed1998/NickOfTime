using NickOfTime.Helper.Constants;
using NickOfTime.ScriptableObjects.Weapons;
using System.Collections;
using UnityEngine;


namespace NickOfTime.Weapons
{
	public class OneLinerRocket : WeaponBase
    {

        private Coroutine _fireRoutine;

        [SerializeField] private GameObject _rocketHead;

        // Probably make a state for reloading
        [SerializeField] private bool _isReloading;
 
        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

		protected override void OnUseWeapon()
		{
			base.OnUseWeapon();
            if(_fireRoutine == null && !_isReloading)
                _fireRoutine = StartCoroutine(RocketFireProcedure());
		}

        // Refactor this section
        /// <summary>
        /// Fire Rocket after firing the dialogue event
        /// </summary>
        /// <returns></returns>
        protected IEnumerator RocketFireProcedure()
		{
            yield return new WaitForEndOfFrame();
            WeaponOwner.DialogPlayer.AssignDialogSet(
                ((RocketLauncherStatsSO)WeaponStats).OneLinerDialogSet);
            WeaponOwner.DialogPlayer.PlayAssignedDialogSet(0,
                () =>
				{
                    FireProjectile(NickOfTimeStringConstants.ROCKET_PROJECTILE_POOL_ID);
                    _rocketHead.SetActive(false);
                    WeaponOwner.Pushback(-1f * transform.right *
                        ((RocketLauncherStatsSO)WeaponStats).PushbackOnFire);
                    _isReloading = true;
                    StartCoroutine(ReloadRoutine());
                    _fireRoutine = null;
                });
		}

        /// <summary>
        /// Routine for reloading this weapon
        /// </summary>
        /// <returns></returns>
        protected IEnumerator ReloadRoutine()
		{
            WeaponOwner.DialogPlayer.AssignDialogSet(
                ((RocketLauncherStatsSO)WeaponStats).ReloadDialogSet);
            WeaponOwner.DialogPlayer.PlayAssignedDialogSet(0);
            yield return new WaitForSeconds(WeaponStats.ReloadTime);
            _rocketHead.SetActive(true);
            _isReloading = false;
		}
	}
}

