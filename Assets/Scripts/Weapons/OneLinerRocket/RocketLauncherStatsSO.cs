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
        [SerializeField] private DialogSetSO _reloadDialogset;
        [SerializeField] private float _explosionRadius;
        [SerializeField] private float _explosionDamage;
        [SerializeField] private float _pushbackOnFire;
         
        public float ExplosionRadius => _explosionRadius;
        public float ExplosionDamage => _explosionDamage;
        public DialogSetSO OneLinerDialogSet => _oneLinerDialogSet;
        public DialogSetSO ReloadDialogSet => _reloadDialogset;
        public float PushbackOnFire => _pushbackOnFire;
    }
}

