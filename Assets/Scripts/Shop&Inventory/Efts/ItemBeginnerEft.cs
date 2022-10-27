using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/GoldNGems/Beginner")]
public class ItemBeginnerEft : ItemEffect
{
    public override bool ExecuteRole()
    {
        GameManager.gold += 10000;
        GameManager.gem += 20;
        GameManager.instance.beginnerSetOff();
        GameManager.instance.GoldNGemUpdate();
        GameManager.instance.SaveUserData();
        return true;
    }
}
