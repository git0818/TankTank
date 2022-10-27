using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ExchangeType
{
    GoldToGem,
    GemToGold
}

[CreateAssetMenu(menuName = "ItemEft/GoldNGems/Exchange")]

public class ItemGoldNGemEft : ItemEffect
{
    public ExchangeType exchangeType;
    public int gold;
    public int gem;
    public override bool ExecuteRole()
    {
        if (exchangeType == ExchangeType.GoldToGem)
        {
            if (GameManager.gold >= gold)
            {
                GameManager.gold -= gold;
                GameManager.gem += gem;
                GameManager.instance.WarningPopUp.SetActive(true);
                GameManager.instance.WariningText.text = "교환에 성공했습니다!";
            }
            else
            {
                GameManager.instance.WarningPopUp.SetActive(true);
                GameManager.instance.WariningText.text = "골드가 부족합니다!";
            }
        }
        else if (exchangeType == ExchangeType.GemToGold)
        {
            if (GameManager.gem >= gem)
            {
                GameManager.gold += gold;
                GameManager.gem -= gem;
                GameManager.instance.WarningPopUp.SetActive(true);
                GameManager.instance.WariningText.text = "교환에 성공했습니다!";
            }
            else
            {
                GameManager.instance.WarningPopUp.SetActive(true);
                GameManager.instance.WariningText.text = "보석이 부족합니다!";
            }
        }
        GameManager.instance.GoldNGemUpdate();
        GameManager.instance.SaveUserData();
        return true;
    }
}