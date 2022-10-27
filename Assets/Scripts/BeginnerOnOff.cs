using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginnerOnOff : MonoBehaviour
{
    public static BeginnerOnOff instance;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public GameObject beginner;
    private void OnEnable()
    {
        if (GameManager.instance.beginnerset == true)
            beginner.SetActive(true);
    }

    public void beginneroff()
    {
        beginner.SetActive(true);
    }

}
