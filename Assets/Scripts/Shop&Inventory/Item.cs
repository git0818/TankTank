using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    EquipmentAiming,
    EquipmentAttack,
    EquipmentDefence,
    Consumables,
    GoldNGem
}
//직렬화
[System.Serializable]
public class Item
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemImage;
    public int itemcode;
    //상점에 팔기 위해 아이템 가격 추가
    public int itemCost;
    //아이템이펙트 리스트
    public List<ItemEffect> efts;

    //아이템 사용하는 기능
    public bool Use()
    {
        //사용 성공 여부를 반환하기 위한 bool
        bool isUsed = false;
        //반복문을 돌려서 efts의 ExecuteRole을 실행함
        foreach(ItemEffect eft in efts)
        {
            isUsed = eft.ExecuteRole(); 
        }
        return isUsed;
    }
}
