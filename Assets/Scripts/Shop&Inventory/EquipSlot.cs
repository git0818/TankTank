using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour, IPointerUpHandler
{
    public Item item;
    public Image itemIcon;
    public Image emptyIcon;
    private InventoryUI invenui;
    

    private void Start()
    {
        invenui = InventoryUI.invenUIinstance;
        
    }
    public void UpdateSlotUI()
    {
        itemIcon.sprite = item.itemImage;
        itemIcon.gameObject.SetActive(true);
        emptyIcon.gameObject.SetActive(false);

    }

    public void RemoveSlot()
    {
        if (item.itemType == ItemType.EquipmentAiming)
        {
            GameManager.instance.plusaiming = 0;
            GameManager.instance.E_aim = 9999;
        }
        else if (item.itemType == ItemType.EquipmentAttack)
        {
            GameManager.instance.plusminattack = 0;
            GameManager.instance.E_attack = 9999;
        }
        else if (item.itemType == ItemType.EquipmentDefence)
        {
            GameManager.instance.plusdefence = 0;
            GameManager.instance.E_defence = 9999;
        }

        item = null;
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
