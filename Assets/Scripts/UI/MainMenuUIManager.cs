using NickOfTime.Common;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NickOfTime.MainMenu
{
    public class MainMenuUIManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _mainCanvas;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartGame()
		{
            SceneTransitioner.instance.BeginLevel(2);
        }

        public void QuitGame()
		{
#if !UNITY_EDITOR
            Application.Quit();
#else
			EditorApplication.isPlaying = false;
#endif
        }
    }

}
