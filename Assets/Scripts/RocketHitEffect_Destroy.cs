using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketHitEffect_Destroy : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("DestroyEffect", 2f);
    }
    private void DestroyEffect()
    {
        RocketHitEffect_ObjectPool.ReturnObject(gameObject);
    }
}
