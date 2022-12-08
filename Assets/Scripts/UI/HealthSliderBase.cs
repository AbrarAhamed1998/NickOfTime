using NickOfTime.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NickOfTime.UI
{
    public class HealthSliderBase : MonoBehaviour
    {
        [SerializeField] protected Slider _healthSlider;
		[SerializeField] protected RectTransform _myRectTransform;

        private Canvas _parentCanvas;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public virtual void SetHealthSliderVal(float val)
		{
            _healthSlider.value = val;
		}

        public virtual void SetWordlPos(Vector3 worldPos)
		{
            Vector3 screenPos = PersistentDataManager.instance.GameplayCamera.WorldToScreenPoint(worldPos);
            //screenPos *= _parentCanvas.scaleFactor;
            SetScreenPos(screenPos);
		}

        public virtual void SetScreenPos(Vector2 pos)
		{
            _myRectTransform.anchoredPosition = pos;
		}

        public virtual void SetParentCanvas(Canvas canvas)
		{
            this._parentCanvas = canvas;
		}

    }

}
