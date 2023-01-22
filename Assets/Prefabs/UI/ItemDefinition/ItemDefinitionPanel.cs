using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NickOfTime.UI.ItemDefinition
{
    public class ItemDefinitionPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Image _itemImage;
        [SerializeField] private CanvasGroup _panelCanvasGroup;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void DisplayPanel(ItemDefinitionData itemDefinitionData)
		{
            Time.timeScale = 0f;
            _titleText.text = itemDefinitionData.ItemTitle;
            _itemImage.sprite = itemDefinitionData.ItemSprite;
            _descriptionText.text = itemDefinitionData.ItemDescription;
            _panelCanvasGroup.blocksRaycasts = true;
            _panelCanvasGroup.interactable = true;
            transform.DOScale(1f, 0.5f).SetEase(Ease.InOutExpo).SetUpdate(true);
		}

        public void ClosePanel()
		{
            transform.DOScale(0f, 0.5f)
                .SetEase(Ease.InOutExpo)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    Time.timeScale = 1f;
                    _panelCanvasGroup.blocksRaycasts = true;
                    _panelCanvasGroup.interactable = true;
                });
		}
    }

    public class ItemDefinitionData
	{
        public string ItemTitle;
        public Sprite ItemSprite;
        public string ItemDescription;
	}
}
