using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConsumableSlot : MonoBehaviour, IPointerUpHandler
{
    public Item item;
    public Image itemIcon;
    public Image emptyIcon;
    private InventoryUI invenui;
    public GameObject cnt;
    public TextMeshProUGUI itemcnt;
    public bool have = false;

    private void Start()
    {
        invenui = InventoryUI.invenUIinstance;

    }
    public void UpdateSlotUI()
    {
        if (ItemDatabase.instance.itemCountDB[item.itemcode] == 0)
        {
            RemoveSlot();
            return;
        }

        have = true;
        itemIcon.sprite = item.itemImage;
        itemIcon.gameObject.SetActive(true);
        emptyIcon.gameObject.SetActive(false);
        cnt.SetActive(true);
        if (ItemDatabase.instance.itemCountDB[item.itemcode] >= 3)
            itemcnt.text = "3";
        else
            itemcnt.text = ItemDatabase.instance.itemCountDB[item.itemcode].ToString();


    }

    public void RemoveSlot()
    {
        have = false;
        item = null;
        for (int i = 0; i < 3; i++)
        {
            if (invenui.consumableslots[i].item == null)
            {
                if (i == 0)
                    GameManager.instance.C_1 = 9999;
                else if (i == 1)
                    GameManager.instance.C_2 = 9999;
                else if (i == 2)
                    GameManager.instance.C_3 = 9999;
            }
        }
        cnt.gameObject.SetActive(false);
        itemIcon.gameObject.SetActive(false);
        emptyIcon.gameObject.SetActive(true);
        GameManager.instance.SaveUserData();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (item != null)
        {
            GameManager.instance.EquipAudio(1);
            RemoveSlot();
        }

    }
}
