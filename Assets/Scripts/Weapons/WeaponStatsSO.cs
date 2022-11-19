using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.ScriptableObjects.Weapons
{
    [CreateAssetMenu(fileName = "WeaponStats.asset", menuName = "Scriptable Objects/Weapons/WeaponStatsSO")]
    public class WeaponStatsSO : ScriptableObject
    {
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private float _projectileLaunchForce;

        public GameObject ProjectilePrefab => _projectilePrefab;
        public float ProjectileLaunchForce => _projectileLaunchForce;  
    }
}

