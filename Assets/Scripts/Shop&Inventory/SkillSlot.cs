using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour, IPointerUpHandler
{
    //슬롯에 들어갈 아이템(게임 시작할 때 SkillSlotUI 스크립트에서 넣어줌)
    public Item item;
    //슬롯에 들어갈 아이템 이미지
    [SerializeField]
    private Image itemIcon;
    //남은 개수를 표시할 itemCount를 가진 게임오브젝트
    [SerializeField]
    private GameObject itemCountObject;
    [SerializeField]
    private TextMeshProUGUI itemCount;

    //남은 아이템 개수
    private int remainItemCount = 0;
    //최대 소지 아이템 개수
    public int maxItemCount = 3;

    //게임을 시작할 때 슬롯에 아이템을 그려주는 메서드
    public void UpdateSlotUI()
    {
        itemIcon.sprite = item.itemImage;
        itemIcon.gameObject.SetActive(true);
        itemCountObject.SetActive(true);
        remainItemCount = Mathf.Min(ItemDatabase.instance.itemCountDB[item.itemcode], maxItemCount);
        itemCount.text = remainItemCount.ToString();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (item != null)
        {
            if (ItemDatabase.instance.itemCountDB[item.itemcode] >= 1)
            {
                StartCoroutine("CoolTimeStart");
                remainItemCount--;
                item.Use();
                itemCount.text = remainItemCount.ToString();
                ItemDatabase.instance.UpdateDB(item.itemcode, -1);
                if(remainItemCount == 0)
                {
                    item = null;
                }
            }
        }
    }

    //쿨타임 때 아이템 아이콘을 가릴 어두운 사각형 이미지
    [SerializeField]
    private Image cooltimeImage;
    //아이템 사용 버튼
    [SerializeField]
    private Button itemUseButton;

    public float cooltime = 10.0f;
    private float lefttime;
    IEnumerator CoolTimeStart()
    {
        lefttime = cooltime;
        if (itemUseButton != null)
            itemUseButton.enabled = false;
        if (cooltimeImage != null)
            cooltimeImage.gameObject.SetActive(true);
        while (true)
        {
            if (lefttime > 0)
            {
                lefttime -= Time.deltaTime;
                if (lefttime <= 0)
                {
                    lefttime = 0;
                    itemUseButton.enabled = true;
                }

                float ratio = 0f + (lefttime / cooltime);
                cooltimeImage.fillAmount = ratio;
            }
            else
            {
                cooltimeImage.gameObject.SetActive(false);
                break;
            }
            yield return null;
        }
    }
}