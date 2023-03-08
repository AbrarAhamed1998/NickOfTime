using DG.Tweening;
using NickOfTime.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NickOfTime.UI.DialogSystem
{
	public class DialogPanel : MonoBehaviour
    {
        public TextMeshProUGUI DialogTextContent;
        public Button NextDialogButton;
        public Image NextDialogImage;
        public GameObject NextDialogPanelParent;

        private Sequence displaySequence;
        private RectTransform _myRectTransform;
        private Queue<DialogSequenceParams> _currentDialogQueue = new Queue<DialogSequenceParams>();

        private Action OnCompleteCurrentSequence;
		private void Start()
		{
            _myRectTransform = GetComponent<RectTransform>();
		}

        public void SetCurrentDialogQueue(List<DialogSequenceParams> dialogSequence, Action OnCompleteSequence = null)
		{
            _currentDialogQueue.Clear();
            for(int i=0; i<dialogSequence.Count; i++)
			{
                _currentDialogQueue.Enqueue(dialogSequence[i]);
			}
            OnCompleteCurrentSequence = OnCompleteSequence;
		}

        public void PlayCurrentDialogQueue()
		{
            DialogSequenceParams dialogSequence;
            if (_currentDialogQueue.Count > 0)
			{
                dialogSequence = _currentDialogQueue.Dequeue();
                PlayDisplaySequence(dialogSequence, PlayCurrentDialogQueue);
            }
            else
			{
                OnCompleteCurrentSequence?.Invoke();
                return;
			}
        }

		public void SetDialog(string dialogContent)
		{
            DialogTextContent.text = dialogContent;
		}

        public void SetUIPosition(Vector3 worldPos)
		{
            Vector2 screenPos = PersistentDataManager.instance.GameplayCamera.WorldToScreenPoint(worldPos);
            if(_myRectTransform != null)
                _myRectTransform.anchoredPosition = screenPos;
		}

        public void PlayDisplaySequence(DialogSequenceParams sequenceParameters, Action OnComplete = null)
		{
            if (displaySequence != null && displaySequence.IsPlaying()) return; //usually this call shouldn't be made when a dialog is running
            displaySequence = DOTween.Sequence();
            displaySequence
                .PrependCallback(() => SetDialog(sequenceParameters.dialogContent))
                .AppendInterval(sequenceParameters.waitTimePreDisplay)
                .Append(transform.DOScale(1f, sequenceParameters.scaleInTime))
                .SetEase(Ease.InOutExpo)
                .AppendInterval(sequenceParameters.waitTimeOnDisplay)
                .Append(transform.DOScale(0f, sequenceParameters.scaleOutTime))
                .SetEase(Ease.InOutExpo)
                .Play().OnComplete(() => OnComplete?.Invoke());
            Debug.Log("Called Play Display Sequence");
		}

        public void KillSequence()
		{
            if (displaySequence.IsPlaying())
                displaySequence.Kill();
		}

        public void PauseSequence()
		{
            if (displaySequence.IsPlaying())
                displaySequence.Pause();
		}

        public void ResumeSequence()
		{
            if (!displaySequence.IsPlaying())
                displaySequence.Play();
		}
    }

	[Serializable]
    public class DialogSequenceParams
	{
		[TextArea]
        public string dialogContent;
        public float waitTimePreDisplay = 0.5f;
        public float scaleInTime = 0.5f;
        public float waitTimeOnDisplay = 3f;
        public float scaleOutTime = 0.5f;
	}
}

