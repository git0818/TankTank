using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlotUI : MonoBehaviour
{
    [SerializeField]
    private SkillSlot[] skillSlots;
    void Start()
    {
        if (GameManager.instance.C_1 != 9999)
        {
            LoadConsumable(0, GameManager.instance.C_1);
        }
        else skillSlots[0].gameObject.SetActive(false);
        if (GameManager.instance.C_2 != 9999)
        {
            LoadConsumable(1, GameManager.instance.C_2);
        }
        else skillSlots[1].gameObject.SetActive(false);
        if (GameManager.instance.C_3 != 9999)
        {
            LoadConsumable(2, GameManager.instance.C_3);
        }
        else skillSlots[2].gameObject.SetActive(false);
    }


    public void LoadConsumable(int i, int itemcode)
    {
        skillSlots[i].item = ItemDatabase.instance.itemDB[itemcode];
        skillSlots[i].UpdateSlotUI();
    }
}
