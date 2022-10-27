using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ColorPicker : MonoBehaviour, ISubject
{
    private List<IObserver> observersList = new List<IObserver>();

    private Vector2 sizeOfPalette;
    private CircleCollider2D paletteCollider;
    private GraphicRaycaster graphicRaycaster;
    [SerializeField]
    private Image circlePalette;
    [SerializeField]
    private Image picker;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Slider S_Slider;
    [SerializeField]
    private Slider V_Slider;
    [SerializeField]
    private GameObject colorPopUp;

    public Material linkedObject;
    #region Builtin Methods
    private void Awake()
    {
        graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
    }
    void Start()
    {
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
                graphicRaycaster.Raycast(ped, hit);
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

    //버튼에 연결됨
    public void ChangeButtonClick(bool onoff)
    {
        colorPopUp.SetActive(onoff);
    }
    private void selectColor()
    {
        Vector3 touchPosition = Input.GetTouch(0).position;
        Vector3 offset = touchPosition - transform.position;
        Vector3 distance = Vector3.ClampMagnitude(offset, paletteCollider.radius);

        picker.transform.position = transform.position + distance;

        ColorChangeHSV(getColor());
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

    //명도,채도 슬라이더에서 사용중이니까 지우면 안됨
    public void ColorChangeHSV()
    {
        float H, S, V;
        Color.RGBToHSV(linkedObject.color, out H, out S, out V);
        linkedObject.color = Color.HSVToRGB(H, S_Slider.value/100, V_Slider.value/100);
    }
    
    private void ColorChangeHSV(Color color)
    {
        float H, S, V;
        Color.RGBToHSV(color, out H, out S, out V);
        linkedObject.color = Color.HSVToRGB(H, S_Slider.value / 100, V_Slider.value / 100);
    }

    public void ResisterObserver(IObserver observer)
    {
        observersList.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        observersList.Remove(observer);
    }

    public void NotifyObservers()
    {
        ChangeButtonClick(false);
        foreach(IObserver observer in observersList)
        {
            observer.UpdateData();
        }
    }

    #endregion
}
