using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour, IPointerUpHandler
{
    //���Կ� �� ������(���� ������ �� SkillSlotUI ��ũ��Ʈ���� �־���)
    public Item item;
    //���Կ� �� ������ �̹���
    [SerializeField]
    private Image itemIcon;
    //���� ������ ǥ���� itemCount�� ���� ���ӿ�����Ʈ
    [SerializeField]
    private GameObject itemCountObject;
    [SerializeField]
    private TextMeshProUGUI itemCount;

    //���� ������ ����
    private int remainItemCount = 0;
    //�ִ� ���� ������ ����
    public int maxItemCount = 3;

    //������ ������ �� ���Կ� �������� �׷��ִ� �޼���
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

    //��Ÿ�� �� ������ �������� ���� ��ο� �簢�� �̹���
    [SerializeField]
    private Image cooltimeImage;
    //������ ��� ��ư
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