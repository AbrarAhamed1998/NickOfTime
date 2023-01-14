using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NickOfTime.Common
{
    public class SceneTransitioner : MonoBehaviour
    {
        public static SceneTransitioner instance; 
		private void Awake()
		{
			if(instance != null)
			{
                Debug.Log("new instance about to be destroyed");
                Destroy(this.gameObject);
                return;
            }
            Debug.Log("set SceneTransitioner Instance");
            instance = this;
            DontDestroyOnLoad(this.gameObject);
		}

		private void OnDestroy()
		{
			
		}
		// Start is called before the first frame update
		void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void BeginLevel(int levelIndex)
		{
            SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Additive).completed += 
                (operation) => {
                    if (operation.isDone)
                        SceneManager.UnloadSceneAsync(1);
                };
        }

        public void LoadLevel(int levelIndex)
		{
            SceneManager.LoadScene(levelIndex);
		}
    }
}

