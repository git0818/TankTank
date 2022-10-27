using System.Collections;
using System.Collections.Generic;
using TankDemo;
using UnityEngine;

public class RocketHitEffect_ObjectPool : MonoBehaviour
{
    public static RocketHitEffect_ObjectPool RocketHitinstance;

    [SerializeField]
    private GameObject objectPoolingPrefab;

    Queue<GameObject> pollingObjectQueue = new Queue<GameObject>();

    private void Awake()
    {
        if (RocketHitinstance == null)
            RocketHitinstance = this;

        Initialize(7);
    }

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            pollingObjectQueue.Enqueue(CreateNewObject());
        }
    }

    private GameObject CreateNewObject()
    {
        var newObj = Instantiate(objectPoolingPrefab);
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public static GameObject GetObject()
    {
        if (RocketHitinstance.pollingObjectQueue.Count > 0)
        {
            var obj = RocketHitinstance.pollingObjectQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = RocketHitinstance.CreateNewObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }
    public static void ReturnObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(RocketHitinstance.transform);
        RocketHitinstance.pollingObjectQueue.Enqueue(obj);
    }
}
