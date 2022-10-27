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

    //�������� ���õǸ� ������ �ٲپ��ٰ� OK��ư�� ���� �� ���γ༮�鸸 �Ⱦƹ����� ������ִ� ��ư
    public bool isSell = false;
    //isSell�� Ʈ��� ����UI�� üũǥ�ø� �������
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
        Debug.Log("��ο����������");
        invenui.DrawConsumeUI(slotnum);
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if(item!=null)
        {
            //isSHop��尡 ���̸� �ǸŸ�� �����̸� �����ۻ����
            if (invenui.isSellMode)
            {
                if (item.itemType != ItemType.Consumables)
                {
                    GameManager.instance.ButtonAudio();
                    //����
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
            Debug.Log("������� ������ : " + item.itemName);
            ItemDatabase.instance.UpdateDB(item.itemcode, -1);
            Inventory.instance.RemoveItem(slotnum);
            isSell = false;
            chkSell?.SetActive(isSell);
        }
    }
    
    //������Ʈ�� ��Ȱ��ȭ�� �� ȣ��Ǵ� �Լ�
    //�ǸŸ� ���ϰ� ������ �����ϸ� ���õ� �������� �����ǰ� ��
    private void OnDisable()
    {
        SellMode(false);
    }
}
