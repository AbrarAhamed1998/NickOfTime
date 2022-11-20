using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NickOfTime.Utilities.PoolingSystem
{
	[CreateAssetMenu(fileName = "VFXData.asset", menuName = "Scriptable Objects/VFXData")]
	public class PoolDataSO : ScriptableObject
	{
		[Header("Pool Objects")]
		public List<PoolObjectBaseSO> poolObjects;
	}
}