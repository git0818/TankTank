using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ShopSlot : MonoBehaviour
{
    public Item item;
    //�������� �ȷȴٴ� ���� �˷��ִ� ����
    //public bool soldOut = false;

    InventoryUI inventoryUI;

    public void Init(InventoryUI Iui)
    {
        inventoryUI = Iui;
    }

    //������ ������ ��
    public void buy()
    {
        GameManager.instance.ButtonAudio();
        if (item != null)
        {

            if (GameManager.gold >= item.itemCost && Inventory.instance.items.Count < Inventory.instance.SlotCnt)
            {
                if (ItemDatabase.instance.itemCountDB[item.itemcode] > 0)
                {
                    if (item.itemType != ItemType.Consumables)
                    {
                        GameManager.instance.WarningPopUp.SetActive(true);
                        GameManager.instance.WariningText.text = "�̹� �������� �������Դϴ�!";
                        return;
                    }
                    else if (item.itemType == ItemType.Consumables)
                    {
                        if (ItemDatabase.instance.itemCountDB[item.itemcode] >= 9)
                        {
                            GameManager.instance.WarningPopUp.SetActive(true);
                            GameManager.instance.WariningText.text = "�� �̻� ���� �� �����ϴ�!";
                            return;
                        }
                    }
                    dobuy();
                    Inventory.instance.Addcnt();
                }
                else
                {
                    dobuy();
                    Inventory.instance.AddItem(item);
                }
            }
            else if (GameManager.gold < item.itemCost)
            {
                GameManager.instance.WarningPopUp.SetActive(true);
                GameManager.instance.WariningText.text = "��尡 �����մϴ�!";
            }
            else if(Inventory.instance.items.Count >= Inventory.instance.SlotCnt)
            {
                GameManager.instance.WarningPopUp.SetActive(true);
                GameManager.instance.WariningText.text = "�κ��丮�� ����á���ϴ�!";
            }
        }
        GameManager.instance.SaveUserData();
    }

    void dobuy()
    {
        GameManager.gold -= item.itemCost;
        GameManager.instance.GoldNGemUpdate();
        GameManager.instance.WarningPopUp.SetActive(true);
        GameManager.instance.WariningText.text = "�������� �����߽��ϴ�!";
        ItemDatabase.instance.UpdateDB(item.itemcode, 1);
    }
    public void DirectUse()
    {
        GameManager.instance.ButtonAudio();
        item.Use();
    }
}
