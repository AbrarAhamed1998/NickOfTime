using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NickOfTime.ScriptableObjects.Enemy
{
	[CreateAssetMenu(fileName = "TankStatsSO.asset", menuName = "Scriptable Objects/Boss/TankStatsSO")]
	public class TankStats : ScriptableObject
	{
		[SerializeField] private float _tankHealth;
		[SerializeField] private AngularLimits _tankGunAngularLimits;
		[SerializeField] private List<Sprite> _tankDamageSprites;

		[Header("Tank Behaviour Stats")]
		[SerializeField] private float _minDistanceFromPlayer;
		[SerializeField] private float _maxDistanceFromPlayer;
		[SerializeField] private float _tankMovementSpeed;
		[SerializeField] private float _tankLookSpeed;
		[SerializeField] private float _tankReloadTime;


		[Header("Tank Round Stats")]
		[SerializeField] private string _tankRoundPoolID;
		[SerializeField] private float _launchForce;
		[SerializeField] private float _hitPushbackIntensity;
		[SerializeField] private float _hitDamage;
		[SerializeField] private float _tankRoundLifeTime;

		[Header("Explosion stats")]
		[SerializeField] private float _explsoionRadius;
		[SerializeField] private float _explosionDamage;
		[SerializeField] private float _explosionLifetime;

		public float TankHealth => _tankHealth;
		public AngularLimits TankAngularLimits => _tankGunAngularLimits;
		public List<Sprite> TankDamageSprites => _tankDamageSprites;
		public float MinDistanceFromPlayer => _minDistanceFromPlayer;
		public float MaxDistanceFromPlayer => _maxDistanceFromPlayer;
		public float TankMovementSpeed => _tankMovementSpeed;
		public float TankLookSpeed => _tankLookSpeed;
		public float TankReloadTime => _tankReloadTime;
		public string TankRoundPoolID => _tankRoundPoolID;
		public float LaunchForce => _launchForce;
		public float HitPushbackIntensity => _hitPushbackIntensity;
		public float HitDamage => _hitDamage;
		public float ExplosionRadius => _explsoionRadius;
		public float TankRoundLifetime => _tankRoundLifeTime;
		public float ExplosionDamage => _explosionDamage;
		public float ExplosionLifetime => _explosionLifetime;
	}

	[Serializable]
	public struct AngularLimits
	{
		public float minAngle;
		public float maxAngle;
	}
}
