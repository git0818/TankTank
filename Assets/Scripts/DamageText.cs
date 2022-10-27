
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamageText : MonoBehaviour
{
    #region Variables
    [Header("Damage Properties")]
    public int damage;
    public float moveSpeed = 4f;
    public float alphaSpeed = 3f;
    public float Timer = 3f;
    
    TextMeshProUGUI text;
    Color alpha;

    #endregion

    #region Builtin Methods
    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        alpha = text.color;
        Invoke("Destroy", Timer);
        text.text = damage.ToString();
    }

    private void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        text.color = alpha;
    }


    #endregion

    #region Custom Methods

    private void Destroy()
    {
        Destroy(gameObject);
    }

    #endregion
}
