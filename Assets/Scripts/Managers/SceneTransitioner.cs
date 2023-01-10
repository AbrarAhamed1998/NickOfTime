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
			if(instance == null)
                instance = this;
            else
                Destroy(this.gameObject);

            DontDestroyOnLoad(this.gameObject);
		}

		private void OnDestroy()
		{
			instance = null;
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

