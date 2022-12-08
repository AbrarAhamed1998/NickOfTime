using NickOfTime.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.UI
{
    public class GameUIManager : MonoBehaviour
    {
        [SerializeField] private Canvas mainCanvas;
        [SerializeField] private HealthSliderBase _playerHealthSliderBase;
        [SerializeField] private RectTransform _enemyHealthbarParent;

        public HealthSliderBase PlayerHealthBar => _playerHealthSliderBase;
        // Start is called before the first frame update
        void Start()
        {
            PersistentDataManager.instance.UIManager = this;
        }

        // Update is called once per frame
        void Update()
        {

        }


        public HealthSliderBase SpawnHealthbar(GameObject uiHealthbar, Vector3 worldPos)
		{
            Vector3 screenPos = PersistentDataManager.instance.GameplayCamera.WorldToScreenPoint(worldPos)*mainCanvas.scaleFactor;
            HealthSliderBase healthSliderBase = Instantiate(uiHealthbar, _enemyHealthbarParent).GetComponent<HealthSliderBase>();
            healthSliderBase.SetScreenPos(screenPos);
            healthSliderBase.SetParentCanvas(mainCanvas);
            return healthSliderBase;
		}
    }
}

