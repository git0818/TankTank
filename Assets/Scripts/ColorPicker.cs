using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class ColorPicker : MonoBehaviour
{
    private static ColorPicker instance = null;
    public static ColorPicker Instance
    {
        get
        {
            if(null == instance) instance = FindObjectOfType<ColorPicker>();
            return instance;
        }
    }

    public Image circlePalette;
    public Image picker;
    public Color selectedColor;
    private Vector2 sizeOfPalette;
    private CircleCollider2D paletteCollider;
    public Material linkedObject;
    private GraphicRaycaster gr;
    public Canvas canvas;
    public Slider S_Slider;
    public Slider V_Slider;

    #region Builtin Methods
    private void Awake()
    {
        if (null == instance) instance = this;
        gr = canvas.GetComponent<GraphicRaycaster>();

    }
    void Start()
    {
        //float H, S, V;
        //Color.RGBToHSV(linkedObject.color, out H, out S, out V);
        S_Slider.value = Mathf.Round(GameManager.instance.S * 100);
        V_Slider.value = Mathf.Round(GameManager.instance.V * 100);
        paletteCollider = circlePalette.GetComponent<CircleCollider2D>();

        sizeOfPalette = new Vector2(circlePalette.GetComponent<RectTransform>().rect.width,
            circlePalette.GetComponent<RectTransform>().rect.height);
    }
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                var ped = new PointerEventData(null);
                ped.position = Input.GetTouch(0).position;
                List<RaycastResult> hit = new List<RaycastResult>();
                gr.Raycast(ped, hit);
                if (hit.Count <= 0)
                    return;
                else if (hit[0].gameObject.tag == "Color")
                selectColor();
                
            }
            else return;
        }
    }

    #endregion


    #region Custom Methods
    private void selectColor()
    {
        Vector3 touchpos = Input.GetTouch(0).position;
        Vector3 offset = touchpos - transform.position;
        Vector3 diff = Vector3.ClampMagnitude(offset, paletteCollider.radius);

        picker.transform.position = transform.position + diff;

        selectedColor = getColor();
        Color temp = selectedColor;
        float H, S, V;
        Color.RGBToHSV(temp, out H, out S, out V);
        linkedObject.color = Color.HSVToRGB(H, S_Slider.value / 100, V_Slider.value / 100);
    }

    private Color getColor()
    {
        Vector2 circlePalettePosition = circlePalette.transform.position;
        Vector2 pickerPosition = picker.transform.position;

        Vector2 position = pickerPosition - circlePalettePosition + sizeOfPalette * 0.5f;
        Vector2 normalized = new Vector2((position.x / (circlePalette.GetComponent<RectTransform>().rect.width)),
            (position.y / circlePalette.GetComponent<RectTransform>().rect.height));

        Texture2D texture = circlePalette.mainTexture as Texture2D;
        Color circularSelectedColor = texture.GetPixelBilinear(normalized.x, normalized.y);

        return circularSelectedColor;
    }

    public void HSV_SV()
    {
        float H, S, V;
        Color.RGBToHSV(linkedObject.color, out H, out S, out V);
        linkedObject.color = Color.HSVToRGB(H, S_Slider.value/100, V_Slider.value/100);
    }

    #endregion
}
