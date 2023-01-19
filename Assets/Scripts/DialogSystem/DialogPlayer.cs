using NickOfTime.Helper.Constants;
using NickOfTime.Managers;
using NickOfTime.Utilities.PoolingSystem;
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
        void Update()
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
		}

        private IEnumerator WaitForInitialize()
		{
            yield return new WaitUntil(() => PersistentDataManager.instance != null);
            yield return new WaitUntil(() => _poolManagerRef != null);
            InitializeDialogPanel();
		}
    }
}

