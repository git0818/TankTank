using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    //�÷��̾ �����ִ� �κ��丮
    public static Inventory instance;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        SlotCnt = 30;
    }

    private void Start()
    {
        for (int i = 0; i < ItemDatabase.instance.itemDB.Count; i++)
        {
            if (ItemDatabase.instance.itemCountDB[i] > 0)
            {
                Debug.Log("add������;  " + ItemDatabase.instance.itemDB[i].itemName);
                AddItem(ItemDatabase.instance.itemDB[i]);
            }
        }
    }

    ////�븮�� ����
    //public delegate void OnSlotCountChange(int val);
    ////�븮�� �ν��Ͻ�ȭ
    //public OnSlotCountChange onSlotCountChange;

    public delegate void OnChageItem();
    public OnChageItem onChangeItem;

    //ȹ���� �������� ���� list
    public List<Item> items = new List<Item>();

    //������ ������ ���ϴ� ����
    private int slotCnt;
    public int SlotCnt
    {
        get => slotCnt;
        set
        {
            slotCnt = value;
            //slotCnt�� ���� ����Ǹ� ����Ǿ��ٰ� �˷��ֱ� ���� ��������Ʈ ���
            //ȣ������� InventoryUI -> Inventory�� �ؾߵ�
            //�ȱ׷��� ��������Ʈ�� �Լ��� �����ϱ⵵ ���� ȣ���ؼ� �����߻�
            //onSlotCountChange.Invoke(slotCnt);
        }
    }

    //�������� �߰��� �� �ִ� �Լ�
    //�������� ������ ���� ����ī��Ʈ�� ������ ���� ���� �߰��� �� ����
    public bool AddItem(Item _item)
    {
        //������ �߰��� �����ϸ� true �����ϸ� false�� ��ȯ
        if(items.Count < SlotCnt)
        {
            items.Add(_item);
            //�������� �߰��Ǹ� ����UI������ �߰��ǰ� �ϴ� ��������Ʈ
            if (onChangeItem != null)
            {
                onChangeItem.Invoke();
            }

            return true;
        }
        return false;
    }

    public void Addcnt()
    {
        if (onChangeItem != null)
        {
            onChangeItem.Invoke();
        }
    }

    public void RemoveItem(int _index)
    {
        items.RemoveAt(_index);
        onChangeItem.Invoke();
    }
}

