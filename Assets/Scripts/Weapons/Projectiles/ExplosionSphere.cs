using Cinemachine;
using NickOfTime.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Weapons.Projectiles
{
	public class ExplosionSphere : MonoBehaviour
	{
		[SerializeField] private ParticleSystem _explosionParticleSystem;
		[SerializeField] private CinemachineImpulseSource _impulseSource;

		[Header("Explosion Variables (Values provided by parent weapon/projectile)")]
		[SerializeField] private float _explosionRadius;
		[SerializeField] private float _explosionForce;
		[SerializeField] private float _explosionDamage;

		[Header("Gizmo Properties")]
		[SerializeField] private Color _gizmoColor;
		

		public void SetExplosionStats(float radius, float explosionForce, float damage)
		{
			_explosionForce = explosionForce;
			_explosionRadius = radius;
			_explosionDamage = damage;
		}
		
		public void TriggerExplosion()
		{
			_explosionParticleSystem.Play();
			_impulseSource.GenerateImpulse();
			RaycastHit2D[] results = new RaycastHit2D[5];
			if(Physics2D
				.CircleCastNonAlloc(transform.position, _explosionRadius, Vector2.one, results) > 0)
			{
				for(int i = 0; i < results.Length; i++)
				{
					if (results[i].collider == null) continue;
					Rigidbody2D itemRigidbody = results[i].collider.GetComponent<Rigidbody2D>();
					if (itemRigidbody != null)
					{
						Vector2 forceToBeApplied = (itemRigidbody.position - (Vector2)transform.position).normalized * _explosionForce;
						itemRigidbody.AddForce(forceToBeApplied, ForceMode2D.Impulse);
						//Debug.Log($"force applied : {forceToBeApplied}, on item : {itemRigidbody.gameObject.name}");
					}
					CharacterBase character = results[i].collider.GetComponent<CharacterBase>();
					BossTank bossTank = results[i].collider.GetComponent<BossTank>();
					
					if (character != null)
					{
						character.TakeDamage(_explosionDamage, Vector2.zero);
					}

					if (bossTank != null)
					{
						bossTank.TakeDamage(_explosionDamage, Vector2.zero);
						Debug.Log("boss tank took damage from explosion");
					}
				}
			}
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = _gizmoColor;
			Gizmos.DrawSphere(transform.position, _explosionRadius);
		}
	}
}

