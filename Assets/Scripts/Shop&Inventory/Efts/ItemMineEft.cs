using System.Collections;
using System.Collections.Generic;
using TankDemo;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/Mine")]
public class ItemMineEft : ItemEffect
{
    public override bool ExecuteRole()
    {
        GameManager.instance.minesetup();
        return true;
    }
}