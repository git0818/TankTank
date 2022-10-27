using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Equipment")]

public class ItemEquipmentEft : ItemEffect
{
    public ItemType type;
    public int stats;
    public override bool ExecuteRole()
    {
        if (type == ItemType.EquipmentAiming)
        {
            GameManager.instance.plusaiming = stats;
        }
        else if(type == ItemType.EquipmentAttack)
        {

            GameManager.instance.plusminattack = stats;
        }
        else if(type == ItemType.EquipmentDefence)
        {
            GameManager.instance.plusdefence = stats;
        }
        GameManager.instance.SaveUserData();

        return true;
    }
}