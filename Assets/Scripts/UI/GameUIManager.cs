using DG.Tweening;
using NickOfTime.Characters.Player;
using NickOfTime.Common;
using NickOfTime.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NickOfTime.UI
{
    public class GameUIManager : MonoBehaviour
    {
        [SerializeField] private Canvas mainCanvas;
        [SerializeField] private HealthSliderBase _playerHealthSliderBase;
        [SerializeField] private BossHealthSlider _bossHealthSlider;
        [SerializeField] private RectTransform _enemyHealthbarParent;
        [SerializeField] private RectTransform _dialogPanelParent;
        [SerializeField] private CanvasGroup _gameOverPanelCanvasGroup;

        public Action OnPlayerDeath;
        public HealthSliderBase PlayerHealthBar => _playerHealthSliderBase;
        public BossHealthSlider BossHealthBar => _bossHealthSlider;
        public RectTransform DialogPanelParent => _dialogPanelParent;

        void Start()
        {
            PersistentDataManager.instance.UIManager = this;
            RegisterEvents();
        }

        void Update()
        {

        }

		private void OnDestroy()
		{
            OnPlayerDeath = null;
		}

		private void RegisterEvents()
		{
            OnPlayerDeath += () => DisplayRestartGameUI();
        }

        private void DisplayRestartGameUI()
		{
            Debug.Log("Called display restart game");
            ToggleCanvasGroup(_gameOverPanelCanvasGroup, true);
		}

        private void ToggleCanvasGroup(CanvasGroup group, bool isVisible, Action OnCompleteFade = null)
		{
            group.DOFade(isVisible ? 1f : 0f, 0.25f).OnComplete(() => {
                group.blocksRaycasts = isVisible;
                OnCompleteFade?.Invoke();
            });
		}

        public HealthSliderBase SpawnHealthbar(GameObject uiHealthbar, Vector3 worldPos)
		{
            Vector3 screenPos = PersistentDataManager.instance.GameplayCamera.WorldToScreenPoint(worldPos)*mainCanvas.scaleFactor;
            HealthSliderBase healthSliderBase = Instantiate(uiHealthbar, _enemyHealthbarParent).GetComponent<HealthSliderBase>();
            healthSliderBase.SetScreenPos(screenPos);
            healthSliderBase.SetParentCanvas(mainCanvas);
            return healthSliderBase;
		}

        /// <summary>
        /// Reloads the level scene
        /// </summary>
        public void RestartLevel()
		{
            if (SceneTransitioner.instance != null)
                SceneTransitioner.instance.LoadLevel(SceneManager.GetActiveScene().buildIndex);
            else
            { 
                // calling scene manager if Scene Transitioner instance is not available.
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			}
		}

        public void DisplayDialog(Transform uiRoot)
		{
            // Take dialog prefab from pool 
            // have ref to a dialog class that provides dialog text/time to display

		}
        public void LoadMainMenu()
		{
            SceneTransitioner.instance.LoadLevel(1);
        }
    }
}

