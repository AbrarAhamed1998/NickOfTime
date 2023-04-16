using NickOfTime.Helper.Constants;
using NickOfTime.Managers;
using NickOfTime.Utilities.PoolingSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.UI.DialogSystem
{
    public class DialogPlayer : MonoBehaviour
    {
        [SerializeField] private DialogSetSO _dialogSetToPlay;
        [SerializeField] private Transform _playerDialogTransform;

        private PoolObject _dialogPanelPoolObject;
        private GameObject _spawnedDialogPanelObject;
        private DialogPanel _spawnedDialogPanel;
        private PoolManager _poolManagerRef => PersistentDataManager.instance.PoolManager;
        private GameUIManager _gameUIManager => PersistentDataManager.instance.UIManager;
        private bool _trackOnTransform = false;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(WaitForInitialize());
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (_trackOnTransform && _spawnedDialogPanel != null)
                _spawnedDialogPanel.SetUIPosition(_playerDialogTransform.position);
        }

        private void InitializeDialogPanel()
		{
            //Get a dailog panel from Pool
            _dialogPanelPoolObject = _poolManagerRef
                .GetPoolObject(NickOfTimeStringConstants.UI_DIALOG_PANEL, 
                _gameUIManager.DialogPanelParent);
            _spawnedDialogPanelObject = _dialogPanelPoolObject.obj;
            _spawnedDialogPanel = _spawnedDialogPanelObject.GetComponent<DialogPanel>();
            if(_spawnedDialogPanel != null)
                _trackOnTransform = true;
		}

        public void AssignDialogSet(DialogSetSO dialogSetSO)
		{
            _dialogSetToPlay = dialogSetSO;
		}

        public void PlayAssignedDialogSet(int index,Action OnCompleteSequence = null)
		{
            if (_spawnedDialogPanel == null)
			{
                StartCoroutine(WaitForSpawnedDialogPanel(index,OnCompleteSequence));
                return;
            }
			_spawnedDialogPanel.SetCurrentDialogQueue(_dialogSetToPlay.DialogSequenceList[index], OnCompleteSequence);
            _spawnedDialogPanel.PlayCurrentDialogQueue();
		}

        private IEnumerator WaitForInitialize()
		{
            yield return new WaitUntil(() => PersistentDataManager.instance != null);
            yield return new WaitUntil(() => _poolManagerRef != null);
            InitializeDialogPanel();
		}

        private IEnumerator WaitForSpawnedDialogPanel(int index,Action onCompleteSequence = null)
		{
            yield return new WaitUntil(() => _spawnedDialogPanel != null);
			PlayAssignedDialogSet(index,onCompleteSequence);
		}
    }
}

