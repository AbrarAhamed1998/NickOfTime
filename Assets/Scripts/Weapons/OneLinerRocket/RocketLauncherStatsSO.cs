using NickOfTime.UI.DialogSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NickOfTime.ScriptableObjects.Weapons
{
    [CreateAssetMenu(fileName = "RocketLauncherStats.asset", menuName = "Scriptable Objects/Weapons/RocketLauncherStatsSO")]
    public class RocketLauncherStatsSO : WeaponStatsSO
    {
        [SerializeField] private DialogSetSO _oneLinerDialogSet;
        [SerializeField] private float _explosionRadius;
        [SerializeField] private float _explosionDamage;
         
        public float ExplosionRadius => _explosionRadius;
        public float ExplosionDamage => _explosionDamage;
        public DialogSetSO OneLinerDialogSet => _oneLinerDialogSet;
    }
}

