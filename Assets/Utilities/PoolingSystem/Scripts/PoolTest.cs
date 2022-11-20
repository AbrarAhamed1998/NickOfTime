using NickOfTime.Utilities.PoolingSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PoolTest : MonoBehaviour
{
    public PoolManager poolManager;
    public float waitTime;


    List<PoolObject> usedObjects = new List<PoolObject>();

	private void Update()
	{
        OnMouse();
	}
	private void OnMouse()
	{
        //Debug.Log("Clicked on obejct");
        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;
        Vector3 mousepos = (Vector3)Mouse.current.position.ReadValue() + Camera.main.transform.forward * 5f;
        
        if(Physics.Raycast(Camera.main.ScreenToWorldPoint(mousepos), Camera.main.transform.forward, out RaycastHit raycastHit))
		{
            Vector3 pos = raycastHit.point;
            PoolObject temp = poolManager.GetPoolObject("FireVFX", this.transform);
            temp.obj.transform.position = pos;
            usedObjects.Add(temp);
            StartCoroutine(WaitToReturnToPool(temp));
        }

	}


	IEnumerator WaitToReturnToPool(PoolObject temp)
	{
        yield return new WaitForSeconds(waitTime);
        poolManager.ReturnObjectToPool(temp);
        temp = null;
        usedObjects.Remove(temp);
	}
}
