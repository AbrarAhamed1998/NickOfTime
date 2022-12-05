using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Common
{
    public class JetController : MonoBehaviour
    {
		[SerializeField] float _minStartLifetime, _maxStartLifetime;
		[SerializeField]
		private ParticleSystem _particleSystem;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AdjustLifetime(float velocityValNormalized)
		{
            var main = _particleSystem.main;
            main.startLifetime = Mathf.Clamp(main.startLifetime.constant,_minStartLifetime,_maxStartLifetime * velocityValNormalized);
		}

        public void AdjustSize(float velocityNormalized)
		{

		}
    }
}

