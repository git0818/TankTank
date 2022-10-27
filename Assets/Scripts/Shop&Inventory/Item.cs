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
//����ȭ
[System.Serializable]
public class Item
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemImage;
    public int itemcode;
    //������ �ȱ� ���� ������ ���� �߰�
    public int itemCost;
    //����������Ʈ ����Ʈ
    public List<ItemEffect> efts;

    //������ ����ϴ� ���
    public bool Use()
    {
        //��� ���� ���θ� ��ȯ�ϱ� ���� bool
        bool isUsed = false;
        //�ݺ����� ������ efts�� ExecuteRole�� ������
        foreach(ItemEffect eft in efts)
        {
            isUsed = eft.ExecuteRole(); 
        }
        return isUsed;
    }
}
