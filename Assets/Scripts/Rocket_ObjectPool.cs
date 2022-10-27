using System.Collections;
using System.Collections.Generic;
using TankDemo;
using UnityEngine;

public class Rocket_ObjectPool : MonoBehaviour
{
    public static Rocket_ObjectPool Rocketinstance;

    [SerializeField]
    private GameObject objectPoolingPrefab;

    Queue<Tank_Rocket> pollingObjectQueue = new Queue<Tank_Rocket>();

    private void Awake()
    {
        if(Rocketinstance == null)
        Rocketinstance = this;

        Initialize(7);
    }

    private  void Initialize(int initCount)
    {
        for(int i = 0; i < initCount; i++)
        {
            pollingObjectQueue.Enqueue(CreateNewObject());
        }
    }

    private Tank_Rocket CreateNewObject()
    {
        var newObj = Instantiate(objectPoolingPrefab).GetComponent<Tank_Rocket>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public static Tank_Rocket GetObject()
    {
        if(Rocketinstance.pollingObjectQueue.Count > 0)
        {
            var obj = Rocketinstance.pollingObjectQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = Rocketinstance.CreateNewObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }
        public static void ReturnObject(Tank_Rocket obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Rocketinstance.transform);
        Rocketinstance.pollingObjectQueue.Enqueue(obj);
    }
}
