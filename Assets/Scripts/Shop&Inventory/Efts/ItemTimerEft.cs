using System.Collections;
using System.Collections.Generic;
using TankDemo;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/Timer")]
public class ItemTimerEft : ItemEffect
{
    public override bool ExecuteRole()
    {
        GameManager.instance.reloaddo();
        return true;
    }
}
