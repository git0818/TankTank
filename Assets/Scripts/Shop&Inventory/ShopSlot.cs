using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ShopSlot : MonoBehaviour
{
    public Item item;
    //아이템이 팔렸다는 것을 알려주는 변수
    //public bool soldOut = false;

    InventoryUI inventoryUI;

    public void Init(InventoryUI Iui)
    {
        inventoryUI = Iui;
    }

    //슬롯이 눌렸을 때
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
                        GameManager.instance.WariningText.text = "이미 보유중인 아이템입니다!";
                        return;
                    }
                    else if (item.itemType == ItemType.Consumables)
                    {
                        if (ItemDatabase.instance.itemCountDB[item.itemcode] >= 9)
                        {
                            GameManager.instance.WarningPopUp.SetActive(true);
                            GameManager.instance.WariningText.text = "더 이상 가질 수 없습니다!";
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
                GameManager.instance.WariningText.text = "골드가 부족합니다!";
            }
            else if(Inventory.instance.items.Count >= Inventory.instance.SlotCnt)
            {
                GameManager.instance.WarningPopUp.SetActive(true);
                GameManager.instance.WariningText.text = "인벤토리가 가득찼습니다!";
            }
        }
        GameManager.instance.SaveUserData();
    }

    void dobuy()
    {
        GameManager.gold -= item.itemCost;
        GameManager.instance.GoldNGemUpdate();
        GameManager.instance.WarningPopUp.SetActive(true);
        GameManager.instance.WariningText.text = "아이템을 구매했습니다!";
        ItemDatabase.instance.UpdateDB(item.itemcode, 1);
    }
    public void DirectUse()
    {
        GameManager.instance.ButtonAudio();
        item.Use();
    }
}
