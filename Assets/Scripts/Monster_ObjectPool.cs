using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_ObjectPool: MonoBehaviour
{
    public Monster_ObjectPool Monsterinstance;

    [SerializeField]
    private GameObject objectPoolingPrefab;

    Queue<Enemy.Enemy_HealthNMove> pollingObjectQueue = new Queue<Enemy.Enemy_HealthNMove>();

    private void Awake()
    {
        if (Monsterinstance == null)
            Monsterinstance = this;
            Initialize(10);
    }

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            pollingObjectQueue.Enqueue(CreateNewObject());
        }
    }

    private Enemy.Enemy_HealthNMove CreateNewObject()
    {
        var newObj = Instantiate(objectPoolingPrefab).GetComponent<Enemy.Enemy_HealthNMove>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public Enemy.Enemy_HealthNMove GetObject()
    {
        if (Monsterinstance.pollingObjectQueue.Count > 0)
        {
            var obj = Monsterinstance.pollingObjectQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = Monsterinstance.CreateNewObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }
    public void ReturnObject(Enemy.Enemy_HealthNMove obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Monsterinstance.transform);
        Monsterinstance.pollingObjectQueue.Enqueue(obj);
    }
}
