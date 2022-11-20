using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Utilities.PoolingSystem
{
	[System.Serializable]
	public class PoolObject
	{
		public string ID;
		public GameObject obj;
		public Transform targetParent;
		public Transform poolParent;

		/// Add any poolObject specific funtions below
	}
}

