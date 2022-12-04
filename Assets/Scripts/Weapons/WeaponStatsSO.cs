using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.ScriptableObjects.Weapons
{
    [CreateAssetMenu(fileName = "WeaponStats.asset", menuName = "Scriptable Objects/Weapons/WeaponStatsSO")]
    public class WeaponStatsSO : ScriptableObject
    {
        [SerializeField] protected LayerMask _pickupMask;
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private float _projectileLaunchForce;
        [SerializeField] private float _maxProjectileLifetime;
        [SerializeField] private float _projectileDamageValue;


        public LayerMask PickupMask => _pickupMask;
        public GameObject ProjectilePrefab => _projectilePrefab;
        public float ProjectileLaunchForce => _projectileLaunchForce; 
        public float MaxProjectileLifetime => _maxProjectileLifetime;
        public float ProjectileDamageValue => _projectileDamageValue;
    }
}

