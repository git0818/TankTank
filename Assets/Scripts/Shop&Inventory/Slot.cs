using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour, IPointerUpHandler
{
    public int slotnum;
    public Item item;
    public Image itemIcon;

    //아이템이 선택되면 참으로 바꾸었다가 OK버튼이 눌릴 때 참인녀석들만 팔아버리게 만들어주는 버튼
    public bool isSell = false;
    //isSell이 트루면 슬롯UI에 체크표시를 만들어줌
    public GameObject chkSell;
    public GameObject cnt;
    public TextMeshProUGUI itemcnt;
    public Image frame;
    public Sprite frameblue;
    public Sprite framegreen;
    public Sprite framepurple;
    public Sprite framered;
    public Sprite frameempty;

    private InventoryUI invenui;
    private void Start()
    {
        invenui = InventoryUI.invenUIinstance;
    }
    public void UpdateSlotUI()
    {
        itemIcon.sprite = item.itemImage;
        itemIcon.gameObject.SetActive(true);
        cnt.gameObject.SetActive(true);
        itemcnt.text = ItemDatabase.instance.itemCountDB[item.itemcode].ToString();
        Debug.Log(item.itemName + " " + ItemDatabase.instance.itemCountDB[item.itemcode]);
        if(item.itemType == ItemType.Consumables)
        {
            frame.sprite = framegreen;
        }
        else if(item.itemType == ItemType.EquipmentAiming)
        {
            frame.sprite = framepurple;
        }
        else if(item.itemType == ItemType.EquipmentAttack)
        {
            frame.sprite = framered;
        }
        else if(item.itemType == ItemType.EquipmentDefence)
        {
            frame.sprite = frameblue;
        }
    }

    public void RemoveSlot()
    {
        item = null;
        frame.sprite = frameempty;
        cnt.gameObject.SetActive(false);
        itemIcon.gameObject.SetActive(false);
    }
    public void SetEquipment()
    {
        if (item.itemType == ItemType.EquipmentAiming)
        {
            invenui.DrawEquipUI(0, slotnum);
            item.Use();
        }
        else if(item.itemType == ItemType.EquipmentAttack)
        {
            invenui.DrawEquipUI(1, slotnum);
            item.Use();
        }
        else if(item.itemType == ItemType.EquipmentDefence)
        {
            invenui.DrawEquipUI(2, slotnum);
            item.Use();
        }
    }

    public void SetConsumable()
    {
        Debug.Log("드로우컨슘우아이");
        invenui.DrawConsumeUI(slotnum);
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if(item!=null)
        {
            //isSHop모드가 참이면 판매모드 거짓이면 아이템사용모드
            if (invenui.isSellMode)
            {
                if (item.itemType != ItemType.Consumables)
                {
                    GameManager.instance.ButtonAudio();
                    //상점
                    SellMode(true);
                }
            }
            else
            {
            GameManager.instance.EquipAudio(0);
                if (item.itemType != ItemType.Consumables)
                {
                    SetEquipment();
                }
                else if (item.itemType == ItemType.Consumables)
                {
                    SetConsumable();
                }
            }
        }

    }
    
    public void SellMode(bool ox)
    {
        isSell = ox;
        chkSell?.SetActive(isSell);
    }

    public void SellItem()
    {
        if(isSell)
        {
            Debug.Log("사라지는 아이템 : " + item.itemName);
            ItemDatabase.instance.UpdateDB(item.itemcode, -1);
            Inventory.instance.RemoveItem(slotnum);
            isSell = false;
            chkSell?.SetActive(isSell);
        }
    }
    
    //오브젝트가 비활성화될 때 호출되는 함수
    //판매를 안하고 상점을 종료하면 선택된 아이템이 해제되게 함
    private void OnDisable()
    {
        SellMode(false);
    }
}
