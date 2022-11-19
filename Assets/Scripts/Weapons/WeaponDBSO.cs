using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.ScriptableObjects.Weapons
{
    [CreateAssetMenu(fileName = "WeaponDB.asset", menuName = "Scriptable Objects/Weapons/WeaponDB")]
    public class WeaponDBSO : ScriptableObject
    {
		[SerializeField] private List<WeaponDBItem> weaponDBItems = new List<WeaponDBItem>();
    }

    public class WeaponDBItem
	{
        public string weaponID;
        public WeaponStatsSO weaponStatsSO;
	}
}

