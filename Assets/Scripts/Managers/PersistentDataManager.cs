using NickOfTime.Characters.Player;
using NickOfTime.UI;
using NickOfTime.Utilities.PoolingSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Managers
{
    public class PersistentDataManager : MonoBehaviour
    {
		public static PersistentDataManager instance;

        [SerializeField] private PoolManager _poolManager;
		[SerializeField] private Camera _gameplayCamera;


        public PoolManager PoolManager => _poolManager;

		public Player ActivePlayer { get; set; }
		public GameUIManager UIManager { get; set; }

		public Camera GameplayCamera { get; set; }

		private void Awake()
		{
			if(instance != null)
				Destroy(this.gameObject);
			instance = this; 
			//DontDestroyOnLoad(gameObject);
			GameplayCamera = _gameplayCamera;
		}

		private void OnDestroy()
		{
			instance = null; 
		}
	}
}

