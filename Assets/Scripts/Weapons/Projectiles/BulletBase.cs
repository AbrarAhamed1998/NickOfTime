using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NickOfTime.Weapons.Projectiles
{
    public class BulletBase : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _myRigidbody2D;
        [SerializeField] private TrailRenderer _myTrailRenderer;
        public Action OnBulletDeactivate;

        [SerializeField] private LayerMask _deactivateLayers;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

		private void OnCollisionEnter2D(Collision2D collision)
		{
            
            Debug.Log($"bullet collided with : {collision.gameObject.name}");
            if (_deactivateLayers == (_deactivateLayers | (1 << collision.gameObject.layer)))
                OnBulletDeactivate?.Invoke();
		}

        public void InitializeProjectile(Action OnDeactivateAction ,float maxLifetime)
		{
            _myTrailRenderer.gameObject.SetActive(true);
            OnBulletDeactivate = () =>
            {
                _myTrailRenderer.gameObject.SetActive(false);
                OnDeactivateAction?.Invoke();
                OnBulletDeactivate = null;
                StopCoroutine(CheckLifetime(maxLifetime));
            };
            StartCoroutine(CheckLifetime(maxLifetime));
		}

        private IEnumerator CheckLifetime(float maxLifetime)
		{
            yield return new WaitForSeconds(maxLifetime);
            if(OnBulletDeactivate != null)
                OnBulletDeactivate?.Invoke();
		}
	}
}

