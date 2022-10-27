using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineEffect : MonoBehaviour
{
    void Start()
    {
        StartCoroutine("minedestroy");
    }

    IEnumerator minedestroy()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
