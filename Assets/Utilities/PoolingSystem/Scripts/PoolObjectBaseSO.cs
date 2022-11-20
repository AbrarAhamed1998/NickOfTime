using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NickOfTime.Utilities.PoolingSystem
{
    [CreateAssetMenu(fileName = "PoolObject.asset", menuName = "Scriptable Objects/Utilities/Pooling/PoolObject")]
    public class PoolObjectBaseSO : ScriptableObject
    {
        public string ID;
        public int poolCount;
        public GameObject prefab;


        private Stack<GameObject> PoolStack;
        private GameObject poolParent;


		#region PUBLIC METHODS
		public void InitializePool(Transform parent)
        {
            poolParent = new GameObject($"PoolParent_{ID}");
            poolParent.transform.SetParent(parent);
            if (PoolStack == null)
            {
                PoolStack = new Stack<GameObject>();
            }
            for (int i = 0; i < poolCount; i++)
            {
                GameObject obj = Instantiate(prefab, poolParent.transform);
                obj.SetActive(false);
                PoolStack.Push(obj);
            }
        }
        public void SetupObjectFromPool(GameObject holder, Transform parent)
        {
            holder.transform.SetParent(parent);
            holder.transform.localPosition = Vector3.zero;
            holder.transform.localEulerAngles = Vector3.zero;
            holder.SetActive(true);
        }

        public void SetupObjectFromPool(PoolObject poolObject)
		{
            poolObject.obj.transform.SetParent(poolObject.targetParent);
            poolObject.obj.transform.localPosition = Vector3.zero;
            poolObject.obj.transform.localEulerAngles = Vector3.zero;
            poolObject.obj.SetActive(true);
        }

        public void ReturnObjectToPool(GameObject holder)
		{
            holder.SetActive(false);
            holder.transform.SetParent(poolParent.transform);
            holder.transform.localPosition = Vector3.zero;
            holder.transform.localEulerAngles = Vector3.zero;
            PoolStack.Push(holder);
        }

        public void ReturnObjectToPool(PoolObject poolObject)
		{
            poolObject.obj.SetActive(false);
            poolObject.obj.transform.SetParent(poolParent.transform);
            poolObject.obj.transform.localPosition = Vector3.zero;
            poolObject.obj.transform.localEulerAngles = Vector3.zero;
            PoolStack.Push(poolObject.obj);
        }

        public PoolObject GetPoolObject(string ID, Transform targetTransform)
		{
            PoolObject poolObject = new PoolObject();
            poolObject.ID = ID;
            poolObject.obj = GetObjectFromStack();
            poolObject.targetParent = targetTransform;
            poolObject.poolParent = GetPoolParent();
            SetupObjectFromPool(poolObject);
            return poolObject;
        }
        public GameObject GetObjectFromStack()
		{
            return PoolStack.Pop();
		}

        public Transform GetPoolParent()
		{
            return poolParent.transform;
		}
		#endregion
	}
}

