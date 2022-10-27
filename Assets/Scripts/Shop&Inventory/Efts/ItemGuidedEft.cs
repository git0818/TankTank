using System.Collections;
using System.Collections.Generic;
using TankDemo;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/Guided")]
public class ItemGuidedEft : ItemEffect
{
    public override bool ExecuteRole()
    {
        GameManager.instance.guideddo();
        return true;
    }
}