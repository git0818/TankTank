using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    //다른 클래스에서 접근할 수 있게 싱글턴 사용
    public static ItemDatabase instance;
    //가지고 있는 아이템의 개수를 저장할 배열 (저장&불러오기에 사용)
    public int[] itemCountDB = new int[15];
    //아이템 리스트
    public List<Item> itemDB = new List<Item>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateDB(int itemcode, int cnt)
    {
        itemCountDB[itemcode] += cnt;     
        GameManager.instance.SaveUserData();
    }
}

