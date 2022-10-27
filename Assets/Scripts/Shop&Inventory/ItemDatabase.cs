using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    //�ٸ� Ŭ�������� ������ �� �ְ� �̱��� ���
    public static ItemDatabase instance;
    //������ �ִ� �������� ������ ������ �迭 (����&�ҷ����⿡ ���)
    public int[] itemCountDB = new int[15];
    //������ ����Ʈ
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

