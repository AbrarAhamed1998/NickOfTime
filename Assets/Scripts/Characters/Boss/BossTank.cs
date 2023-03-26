using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace NickOfTime.Characters
{
    public class BossTank : MonoBehaviour
    {
		[SerializeField] private Transform _gunTransform;
        [SerializeField] private Transform _target;

        [SerializeField] private AngularLimits _tankGunAngularLimits;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            LookAtWorldPos(_target);
        }

        protected virtual void LookAtWorldPos(Transform targetWorldTransform)
        {
            if (targetWorldTransform == null)
            {
                //Debug.Log("target World Pos is null");
                return;
            }
            Vector2 worldPos = targetWorldTransform.position;
            Transform target = _gunTransform;
            float y = target.position.y - worldPos.y;
            float x = target.position.x - worldPos.x;
            float localEulerY = transform.localEulerAngles.y;
            float trueYRot = localEulerY < 180f ? localEulerY : localEulerY - 360;
            float switchFactor = (trueYRot < 0 ? 0f : 180f);
            float targetAngle = (Mathf.Atan2(y, x) * Mathf.Rad2Deg) + switchFactor;
            //Debug.Log($"switch factor : {switchFactor}");
            float finalZangle = (switchFactor == 0f ? -1f : 1f) * targetAngle;

            //target.localEulerAngles = new Vector3(0f, 0f, finalZangle);

            target.localEulerAngles = new Vector3(0f, 0f, ClampAngle(switchFactor, finalZangle));

            /*for (int i = 1; i < _debugLookObjects.Length; i++)
            {
                _debugLookObjects[i].transform.localEulerAngles = target.localEulerAngles;
            }*/

            Vector2 mouseDirection = (worldPos - (Vector2)this.transform.position).normalized * Vector2.right;
            Vector2 playerdirection = transform.right.normalized;
            float dotProduct = Vector2.Dot(mouseDirection, playerdirection);
            if (dotProduct < 0f)
            {
                transform.localEulerAngles += Vector3.up * 180f;
            }
        }

        // Makes sure to limit angles when character faces wither direction
        public float ClampAngle(float switchFactor, float value)
		{
            float minValue;
            float maxValue;
            if(switchFactor == 0f)
			{
                minValue = _tankGunAngularLimits.minAngle;
                maxValue = _tankGunAngularLimits.maxAngle;
			}
			else
			{
                minValue = _tankGunAngularLimits.minAngle;
                maxValue = _tankGunAngularLimits.maxAngle;
			}
            float finalVal = switchFactor == 0f ? value : value > 180f ? value - 360f : value;
            //Debug.Log($"minValue : {minValue}, maxValue : {maxValue}, value : {finalVal}");
                
            return Mathf.Clamp(finalVal, minValue, maxValue);
		}

		[Serializable]
        public struct AngularLimits
		{
            public float minAngle;
            public float maxAngle;
		}
    }
}

