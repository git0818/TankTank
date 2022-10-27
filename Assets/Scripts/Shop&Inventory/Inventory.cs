using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    //플레이어가 갖고있는 인벤토리
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
                Debug.Log("add아이템;  " + ItemDatabase.instance.itemDB[i].itemName);
                AddItem(ItemDatabase.instance.itemDB[i]);
            }
        }
    }

    ////대리자 정의
    //public delegate void OnSlotCountChange(int val);
    ////대리자 인스턴스화
    //public OnSlotCountChange onSlotCountChange;

    public delegate void OnChageItem();
    public OnChageItem onChangeItem;

    //획득한 이이템을 담을 list
    public List<Item> items = new List<Item>();

    //슬롯의 개수를 정하는 변수
    private int slotCnt;
    public int SlotCnt
    {
        get => slotCnt;
        set
        {
            slotCnt = value;
            //slotCnt의 값이 변경되면 변경되었다고 알려주기 위해 델리게이트 사용
            //호출순서는 InventoryUI -> Inventory로 해야됨
            //안그러면 델리게이트가 함수를 참조하기도 전에 호출해서 오류발생
            //onSlotCountChange.Invoke(slotCnt);
        }
    }

    //아이템을 추가할 수 있는 함수
    //아이템의 개수가 현재 슬롯카운트의 수보다 작을 때만 추가할 수 있음
    public bool AddItem(Item _item)
    {
        //아이템 추가에 성공하면 true 실패하면 false를 반환
        if(items.Count < SlotCnt)
        {
            items.Add(_item);
            //아이템이 추가되면 슬롯UI에서도 추가되게 하는 델리게이트
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

