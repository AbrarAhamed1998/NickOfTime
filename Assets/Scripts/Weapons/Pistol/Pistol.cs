using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Weapons
{
	public class Pistol : WeaponBase
	{
		[SerializeField] private Transform _barrel;
		protected override void OnPickUp()
		{
			base.OnPickUp();
		}

		protected override void OnUseWeapon()
		{
			base.OnUseWeapon();
			Debug.Log("Weapon fired");
		}
	}
}

