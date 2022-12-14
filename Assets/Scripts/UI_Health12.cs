using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Health : MonoBehaviour
{
    public bool m_UseRelativeRotation = true;

    private Quaternion m_RelativeRotation;

    void Start()
    {
        m_RelativeRotation = transform.parent.localRotation;    
    }

    void Update()
    {
        if(m_UseRelativeRotation)
            transform.rotation = m_RelativeRotation;
    }
}
