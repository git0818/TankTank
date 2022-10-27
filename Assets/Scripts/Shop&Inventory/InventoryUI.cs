using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI invenUIinstance;

    //인벤토리 변수 선언 
    Inventory inven;

    //슬롯의 배열
    public Slot[] slots;

    //장비창
    public EquipSlot[] equipslots;

    //소모품
    public ConsumableSlot[] consumableslots;
    
    //Content 게임오브젝트
    public Transform slotHolder;

    //판매버튼
    public bool isSellMode = false;

    [SerializeField]
    private TextMeshProUGUI numofitem;
    [SerializeField]
    private GameObject trashbutton;
    [SerializeField]
    private GameObject dobutton;
    [SerializeField]
    private Image cancel;

    private void Awake()
    {
        if (invenUIinstance == null)
        {
            invenUIinstance = this;
        }
        else
        {
            Destroy(this);
        } 
    }
    void Start()
    {
        Debug.Log("인벤토리 켜졌다");
        //인벤토리 인스턴스로 inven 초기화
        inven = Inventory.instance;
        //슬롯 스크립트를 가지고 있는 자식 오브젝트들로 배열 한번에 채우기
        //게임시작하면 자동으로 Slot[]이 slot의 개수만큼 채워짐
        slots = slotHolder.GetComponentsInChildren<Slot>();
        //onChangeItem이 참조할 함수를 정의함 =
        inven.onChangeItem += RedrawSlotUI;
        //모든 슬롯을 한번 초기화해줌
        RedrawSlotUI();
        RedrawEquipUI();
        RedrawConsumeUI();
    }

    //반복문을 통해 슬롯들을 초기화하고 아이템의 개수만큼 슬롯을 채워넣음
    void RedrawSlotUI()
    {
        Debug.Log("리드로우슬로우아이");
        for(int i = 0; i <slots.Length; i++)
        {
            slots[i].slotnum = i;
            slots[i].RemoveSlot();
        }
        for(int i = 0; i<inven.items.Count; i++)
        {
            slots[i].item = inven.items[i];
            slots[i].UpdateSlotUI();
        }
        numofitem.text = inven.items.Count.ToString() + " / 30";

    }

    void RedrawEquipUI()
    {
        if (GameManager.instance.E_aim != 9999)
            InventoryUI.invenUIinstance.LoadEquip(0, GameManager.instance.E_aim);
        if (GameManager.instance.E_attack != 9999)
            InventoryUI.invenUIinstance.LoadEquip(1, GameManager.instance.E_attack);
        if (GameManager.instance.E_defence != 9999)
            InventoryUI.invenUIinstance.LoadEquip(2, GameManager.instance.E_defence);
    }

    void RedrawConsumeUI()
    {
        if (GameManager.instance.C_1 != 9999)
            InventoryUI.invenUIinstance.LoadConsume(0, GameManager.instance.C_1);
        if (GameManager.instance.C_2 != 9999)
            InventoryUI.invenUIinstance.LoadConsume(1, GameManager.instance.C_2);
        if (GameManager.instance.C_3 != 9999)
            InventoryUI.invenUIinstance.LoadConsume(2, GameManager.instance.C_3);
    }

    public void LoadEquip(int i, int code)
    {
        equipslots[i].item = ItemDatabase.instance.itemDB[code];
        equipslots[i].UpdateSlotUI();
        equipslots[i].item.Use();
    }

    public void LoadConsume(int i, int code)
    {
        consumableslots[i].item = ItemDatabase.instance.itemDB[code];
        consumableslots[i].UpdateSlotUI();
    }

    public void DrawEquipUI(int i, int slotnum)
    {
        equipslots[i].item = slots[slotnum].item;
        equipslots[i].UpdateSlotUI();
        if (i == 0)
            GameManager.instance.E_aim = equipslots[i].item.itemcode;
        else if (i == 1)
            GameManager.instance.E_attack = equipslots[i].item.itemcode;
        else if (i == 2)
            GameManager.instance.E_defence = equipslots[i].item.itemcode;
        GameManager.instance.SaveUserData();
    }

    public void DrawConsumeUI(int slotnum)
    {
        for (int i = 0; i < consumableslots.Length; i++)
        {
            if (consumableslots[i].have == true)
            {
                if (consumableslots[i].item.itemcode == slots[slotnum].item.itemcode)
                    return;
            }
        }
        for (int i = 0; i < consumableslots.Length; i++)
        { 
            if (consumableslots[i].have == false)
            {
                consumableslots[i].item = slots[slotnum].item;
                consumableslots[i].UpdateSlotUI();
                if (i == 0)
                    GameManager.instance.C_1 = consumableslots[i].item.itemcode;
                else if (i == 1)
                    GameManager.instance.C_2 = consumableslots[i].item.itemcode;
                else if (i == 2)
                    GameManager.instance.C_3 = consumableslots[i].item.itemcode;
                GameManager.instance.SaveUserData();
                Debug.Log("C1 : " + GameManager.instance.C_1);
                Debug.Log("C2 : " + GameManager.instance.C_2);
                Debug.Log("C3 : " + GameManager.instance.C_3);
                return;
            }
        }
        consumableslots[2].item = null;
        consumableslots[2].item = slots[slotnum].item;
        consumableslots[2].UpdateSlotUI();
        GameManager.instance.C_3 = consumableslots[2].item.itemcode;
        GameManager.instance.SaveUserData();

    }

    public void SellModeOn()
    {
        GameManager.instance.ButtonAudio();
        isSellMode = true;
        trashbutton.SetActive(false);
        dobutton.SetActive(true);
        cancel.gameObject.SetActive(false);
    }

    public void SellModeOff()
    {
        GameManager.instance.ButtonAudio();
        isSellMode = false;
        trashbutton.SetActive(true);
        dobutton.SetActive(false);
        cancel.gameObject.SetActive(true);
        for (int i = slots.Length; i > 0; i--)
        {
            slots[i-1].SellMode(false);
        }
    }

    void OnDisable()
    {
        isSellMode = false;
        trashbutton.SetActive(true);
        dobutton.SetActive(false);
        cancel.gameObject.SetActive(true);
    }
    public void SellBtn()
    {
        //슬롯의 크기부터 거꾸로 하나씩 빼면서 반복문을 돌림
        //0번슬롯부터 시작하게 되면 앞부분의 슬롯이 팔리는 순간 뒷부분의 슬롯이 앞으로 오면서 데이터가 꼬여버리기 때문
        GameManager.instance.ButtonAudio();
        for (int i = slots.Length; i > 0; i--)
        {
            for (int j = 0; j < 3; j++)
            {
                if (slots[i - 1].isSell == true)
                {
                    if (equipslots[j].item != null)
                    {
                        if (slots[i - 1].item.itemcode == equipslots[j].item.itemcode)
                            equipslots[j].RemoveSlot();
                    }
                }
            }
            slots[i - 1].SellItem();
        }
    }
}
