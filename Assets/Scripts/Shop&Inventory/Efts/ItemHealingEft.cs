using System.Collections;
using System.Collections.Generic;
using TankDemo;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/Health")] 
public class ItemHealingEft : ItemEffect
{
    public int amount;
    public override bool ExecuteRole()
    {
        GameManager.instance.TankHeal(amount);
        return true;
    }
}