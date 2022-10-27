using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LayerLab.CasualGame
{
    public class PanelCasualGame : MonoBehaviour
    {
        [SerializeField] private GameObject[] onotherPanels;
        [SerializeField] private GameObject[] offotherPanels;

        public void OnEnable()
        {
            Invoke("Wait", 2f);
        }

        //public void OnDisable()
        //{
        //    for (int i = 0; i < otherPanels.Length; i++) otherPanels[i].SetActive(false);
        //}


        void Wait()
        {
            for (int i = 0; i < onotherPanels.Length; i++) onotherPanels[i].SetActive(true);
            //for (int i = 0; i < offotherPanels.Length; i++) offotherPanels[i].SetActive(false);
        }
    }
}
