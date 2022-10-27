using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShopUI : MonoBehaviour
{
    public GameObject[] menus;
    public GameObject[] menutexts;
    public GameObject[] pickedtexts;
    
    public void menuselect(int num)
    {
        GameManager.instance.ButtonAudio();
        menuonoff(num);
    }

    void menuonoff(int num)
    {
        for(int i = 0; i<menus.Length;i++)
        {
            if (i == num)
            {
                menus[i].SetActive(true);
                pickedtexts[i].SetActive(true);
                menutexts[i].SetActive(false);
            }
            else
            {
                menus[i].SetActive(false);
                pickedtexts[i].SetActive(false);
                menutexts[i].SetActive(true);
            }
        }
    }
}
