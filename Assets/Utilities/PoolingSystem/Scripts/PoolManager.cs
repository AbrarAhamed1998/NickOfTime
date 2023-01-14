using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NickOfTime.Utilities.PoolingSystem
{
    public class PoolManager : MonoBehaviour
    {
        public PoolDataSO poolData;

        #region UNITY CALLBACK
        /// Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < poolData.poolObjects.Count; i++)
            {
                poolData.poolObjects[i].InitializePool(this.transform);
            }
        }


        /// Update is called once per frame
        void Update()
        {

        }

        #endregion

        #region PRIVATE METHODS


        #endregion

        #region PUBLIC METHODS

        public GameObject GetObjectFromStack(string ID)
        {
            PoolObjectBaseSO poolObjectBase = poolData.poolObjects.FirstOrDefault(po => po.ID == ID);
            if (poolObjectBase)
                return poolObjectBase.GetObjectFromStack();
            else
                return null;
        }
        public PoolObject GetPoolObject(string ID, Transform targetTransform)
		{
            PoolObjectBaseSO poolObjectBase = poolData.poolObjects.FirstOrDefault(po => po.ID == ID);
            if (poolObjectBase)
            {
                return poolObjectBase.GetPoolObject(ID, targetTransform);
            }
            else
                return null;
        }
        public void SetupObjectFromPool(string ID, GameObject holder, Transform parent)
        {
            PoolObjectBaseSO poolObjectBase = poolData.poolObjects.FirstOrDefault(po => po.ID == ID);
            if (poolObjectBase != null)
            {
                poolObjectBase.SetupObjectFromPool(holder, parent);
            }
        }

        public void ReturnObjectToPool(string ID, GameObject holder)
        {
            PoolObjectBaseSO poolObjectBase = poolData.poolObjects.FirstOrDefault(po => po.ID == ID);
            if (poolObjectBase)
            {
                poolObjectBase.ReturnObjectToPool(holder);
            }
        }

        public void ReturnObjectToPool(PoolObject poolObject)
		{
            PoolObjectBaseSO poolObjectBase = poolData.poolObjects.FirstOrDefault(po => po.ID == poolObject.ID);
            if (poolObjectBase)
            {
                poolObjectBase.ReturnObjectToPool(poolObject);
            }
        }
        #endregion
    }
}


