using NickOfTime.Characters.Player;
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
        public PoolManager PoolManager => _poolManager;

		public Player ActivePlayer { get; set; }

		private void Awake()
		{
			if(instance != null)
				Destroy(this.gameObject);
			instance = this; 
			DontDestroyOnLoad(gameObject);
		}

		private void OnDestroy()
		{
			instance = null; 
		}
	}
}

