using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.ScriptableObjects.Weapons
{
    [CreateAssetMenu(fileName = "WeaponStats.asset", menuName = "Scriptable Objects/Weapons/WeaponStatsBaseSO")]
    public class WeaponStatsSO : ScriptableObject
    {
        [SerializeField] protected LayerMask _pickupMask;
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private float _projectileLaunchForce;
        [SerializeField] private float _maxProjectileLifetime;
        [SerializeField] private float _projectileDamageValue;

        [SerializeField,Layer] private int _globalProjectileLayer;
        [SerializeField,Layer] private int _playerProjectileLayer;

        [SerializeField] private float _pushbackIntensity;
        [SerializeField] private float _reloadTime;

        public LayerMask PickupMask => _pickupMask;
        public GameObject ProjectilePrefab => _projectilePrefab;
        public float ProjectileLaunchForce => _projectileLaunchForce; 
        public float MaxProjectileLifetime => _maxProjectileLifetime;
        public float ProjectileDamageValue => _projectileDamageValue;
		public int GlobalProjectileLayer => _globalProjectileLayer;
        public int PlayerProjectileLayer => _playerProjectileLayer;
        public float PushbackIntensity => _pushbackIntensity;
        public float ReloadTime => _reloadTime;
    }
}

