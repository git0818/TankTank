using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq.Expressions;
using UnityEngine.SceneManagement;
//������ ����
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using Newtonsoft.Json.Serialization;
using Unity.VisualScripting;
using TankDemo;

public class GameManager : MonoBehaviour, IObserver
{
    public static GameManager instance;
    public AudioSource audiosource;

    [Header("Player Info")]
    public static int playerLevel = 1;
    public static int gold = 0;
    public static int gem = 0;
    public static int SP = 1;
    private int maxExp = 100;
    public Slider expSlider;
    public float T_Health = 100f;
    public float T_Speed = 15f;
    public float T_Attack = 100f;
    public float T_Reload = 1f;
    public float T_Range = 20f;
    public float H, S, V;
    public Material T_Mat;
    private Color T_Color;
    public bool beginnerset = false;


    [Header("Common Properties")]
    [SerializeField]
    private TextMeshProUGUI playerleveltext;
    [SerializeField]
    private TextMeshProUGUI exptext;
    [SerializeField]
    private TextMeshProUGUI goldtext;
    [SerializeField]
    private TextMeshProUGUI gemtext;

    [Header("Ingame Properties")]
    public bool isGameover = false;
    [SerializeField]
    private TextMeshProUGUI killscore;
    [SerializeField]
    private TextMeshProUGUI gameoverkillscore;
    [SerializeField]
    private GameObject gameoverUI;
    [SerializeField]
    private GameObject stareffect;
    [SerializeField]
    private GameObject levelupeffect;
    [SerializeField]
    private GameObject skillUI;
    private float score = 0;

    [Header("Button Properties")]
    [SerializeField]
    private GameObject battleselectbutton;
    [SerializeField]
    private AudioClip buttonaudio;
    [SerializeField]
    private AudioClip equipaudio;
    [SerializeField]
    private AudioClip equipoffaudio;

    [Header("Upgrade Properties")]
    public Slider[] Upgrades;
    public TextMeshProUGUI[] Uptexts;
    public int[] Uvalues = { 0, 0, 0, 0, 0 };
    public GameObject upgradeDetail;
    public TextMeshProUGUI Detailtextbefore;
    public TextMeshProUGUI Detailtextafter;
    public TextMeshProUGUI Detailvaluebefore;
    public TextMeshProUGUI Detailvalueafter;
    public TextMeshProUGUI Addedvalue;
    public TextMeshProUGUI remainSP;
    public TextMeshProUGUI needSP;
    public Sprite[] UpIcons;
    public Image upiconbefore;
    public Image upiconafter;
    public GameObject WarningPopUp;
    public TextMeshProUGUI WariningText;
    public TextMeshProUGUI UpgradeCostText;
    public GameObject ResetPopUp;
    public TextMeshProUGUI ReturnText;
    public float UpgradeCost;
    float plushealth = 20f;
    float plusspeed = 0.5f;
    float plusattack = 10f;
    float plusreload = -0.05f;
    float plusrange = 1f;

    [Header("Color Picker")]
    [SerializeField]
    private ColorPicker colorPicker;

    [Header("InventoryNShop Properties")]
    public GameObject invenui;
    public GameObject shopui;
    public TextMeshProUGUI shopgoldtext;
    public TextMeshProUGUI shopgemtext;

    [Header("Equipment Stats")]
    public int plusaiming = 0;
    public int plusminattack = 0;
    public int plusdefence = 0;
    public int buycheck = 0;
    public int E_aim = 9999;
    public int E_attack = 9999;
    public int E_defence = 9999;

    [Header("Consumable Stats")]
    public int C_1 = 9999;
    public int C_2 = 9999;
    public int C_3 = 9999;

    //�ΰ��� �Ҹ�ǰ �Լ��� ��
    private GameObject player;
    private Tank_Health tankhealth;
    public bool guided = false;
    public float minustimer = 0;
    public AudioClip MineSetSound;
    public AudioClip MineBoomSound;
    public AudioClip TimerSound;
    public AudioClip HealSound;
    public AudioClip GuidedSound;


    //��ư �Ű�����
    private int uptype;
    UserData userData;

    #region Builtin Methods
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        LoadUserData();
        //Application.targetFrameRate = 60;
        playerLevel = userData.playerlevel;
        gold = userData.gold;
        gem = userData.gem;
        maxExp = userData.maxexp;
        expSlider.maxValue = maxExp;
        expSlider.value = userData.expvalue;

        Uvalues[0] = userData.HPvalue;
        Uvalues[1] = userData.Speedvalue;
        Uvalues[2] = userData.Attackvalue;
        Uvalues[3] = userData.Reloadvalue;
        Uvalues[4] = userData.Rangevalue;
        SP = userData.SPvalue;
        T_Health = userData.T_Healthvalue;
        T_Speed = userData.T_Speedvalue;
        T_Attack = userData.T_Attackvalue;
        T_Reload = userData.T_Reloadvalue;
        T_Range = userData.T_Rangevalue;
        H = userData.H_value;
        S = userData.S_value;
        V = userData.V_value;
        T_Mat.color = Color.HSVToRGB(H, S, V);
        T_Color = T_Mat.color;
        Debug.Log("H: " + H*360 + " S: " + S*100 + " V: " + V*100);
        plusaiming = userData.E_plusaiming;
        plusminattack = userData.E_plusminattack;
        plusdefence = userData.E_plusdefence;
        E_aim = userData.E_aimcode;
        E_attack = userData.E_attackcode;
        E_defence = userData.E_defencecode;
        C_1 = userData.C_1;
        C_2 = userData.C_2;
        C_3 = userData.C_3;
        beginnerset = userData.Beginnerset;
        ItemDatabase.instance.itemCountDB = userData.saveItemDB;


        //�κ�
        playerleveltext.text = playerLevel.ToString();
        goldtext.text = gold.ToString();
        gemtext.text = gem.ToString();
        exptext.text = ((int)expSlider.value).ToString() + "/" + maxExp;
        for (int i = 0; i < Upgrades.Length; i++)
        {
            Upgrades[i].value = Uvalues[i];
            Uptexts[i].text = Uvalues[i].ToString() + " / 10";
        }
        if (SceneManager.GetActiveScene().name == "Main")
        {
            remainSP.text = "���� ����Ʈ : " + SP;
            shopgoldtext.text = goldtext.text;
            shopgemtext.text = gemtext.text;
        }

        //�ΰ���
        if (SceneManager.GetActiveScene().name == "Battle1")
        {
            killscore.text = 0.ToString();
            player = GameObject.FindWithTag("Player");
            tankhealth = player.GetComponent<Tank_Health>();
        }

        colorPicker.ResisterObserver(this);

    }

    private void Start()
    {
        Debug.Log("�κ��丮ui ������");
        if (SceneManager.GetActiveScene().name == "Main")
            StartCoroutine("initinven");
    }
    #endregion
    IEnumerator initinven()
    {
        invenui.SetActive(true);
        yield return null;
        invenui.SetActive(false);
    }
    #region Custom Methods
    #region Ingame Methods
    public void AddScore(int newScore)
    {
        if(!isGameover)
        {
            StartCoroutine(upCount(score + newScore));
        }
    }

    public void PlayerDead()
    {
        isGameover = true;
        skillUI.SetActive(false);
        gameoverUI.SetActive(true);
        gameoverkillscore.text = killscore.text;
        float originscore = score;
        StartCoroutine("downCount");
        StartCoroutine(ExpupCount(originscore));
        gold += goldCount(originscore);
        gem += gemCount(originscore);
        Invoke("SaveUserData", 2f);
    }

    private int goldCount(float score)
    {
        float earngold = score / 5f;
        float weight = UnityEngine.Random.Range(0.8f, 1.2f);
        earngold *= weight;
        int i_earngold = (int)earngold;
        goldtext.text = i_earngold.ToString();
        return i_earngold;
    }

    private int gemCount(float score)
    {
        float earngem = score / 10000f;
        int i_earngem = Mathf.Max(1, (int)earngem);
        gemtext.text = i_earngem.ToString();
        return i_earngem;
    }
    IEnumerator upCount(float target)
    {
        float duration = 0.5f;
        float offset = (target - score) / duration;

        while (score < target)
        {
            score += offset * Time.deltaTime;
            killscore.text = ((int)score).ToString();
            yield return null;
        }

        score = target;
        killscore.text = ((int)score).ToString();
    }

    IEnumerator downCount()
    {
        float duration = 2f;
        float offset = score / duration;

        while (score > 0)
        {
            score -= offset * Time.deltaTime;
            gameoverkillscore.text = ((int)score).ToString();
            yield return null;
        }

        score = 0;
        gameoverkillscore.text = ((int)score).ToString();
        stareffect.SetActive(false);
    }

    IEnumerator ExpupCount(float target)
    {
        float originexp = expSlider.value;
        float exptarget = target / 50f;
        float duration = 2f;
        float offset = exptarget / duration;
        while (expSlider.value < originexp + exptarget)
        {
            expSlider.value += offset * Time.deltaTime;
            exptext.text = ((int)expSlider.value).ToString() + "/" + maxExp;
            if(expSlider.value >= maxExp)
            {
                levelupeffect.SetActive(true);
                playerLevel++;
                playerleveltext.text = playerLevel.ToString();
                SP+=10;
                expSlider.value = 0;
                exptarget -= maxExp;
                maxExp = 100 + (playerLevel - 1) * 200;
                expSlider.maxValue = maxExp;
                exptext.text = ((int)expSlider.value).ToString() + "/" + maxExp;
            }
            yield return null;
        }
    }
    public void guideddo()
    {
        StartCoroutine("guidedrocket");
        audiosource.PlayOneShot(GuidedSound);
    }

    IEnumerator guidedrocket()
    {
        guided = true;
        yield return new WaitForSeconds(10f);
        guided = false;
    }

    public delegate void OnUseMine();
    public OnUseMine onusemine;

    public void minesetup()
    {
        if (onusemine != null)
        {
            onusemine.Invoke();
            audiosource.PlayOneShot(MineSetSound);
        }
    }

    public void reloaddo()
    {
        StartCoroutine("reloading");
        audiosource.PlayOneShot(TimerSound);
    }

    IEnumerator reloading()
    {
        minustimer = 0.4f;
        yield return new WaitForSeconds(10f);
        minustimer = 0;
    }
    #endregion

    #region Data Methods

    public void cheat()
    {
        gold = 100000;
        goldtext.text = gold.ToString();
        gem = 100000;
        gemtext.text = gem.ToString();
    }

    public void GoldNGemUpdate()
    {
        goldtext.text = gold.ToString();
        gemtext.text = gem.ToString();
        shopgoldtext.text = goldtext.text;
        shopgemtext.text = gemtext.text;
    }

    public void beginnerSetOff()
    {
        beginnerset = true;
        BeginnerOnOff.instance.beginneroff();
    }
    public void SaveUserData()
    {
        userData.playerlevel = playerLevel;
        userData.gold = gold;
        userData.gem = gem;
        userData.maxexp = maxExp;
        userData.expvalue = expSlider.value;
        userData.HPvalue = Uvalues[0];
        userData.Speedvalue = Uvalues[1];
        userData.Attackvalue = Uvalues[2];
        userData.Reloadvalue = Uvalues[3];
        userData.Rangevalue = Uvalues[4];
        userData.SPvalue = SP;
        userData.T_Healthvalue = T_Health;
        userData.T_Speedvalue = T_Speed;
        userData.T_Attackvalue = T_Attack;
        userData.T_Reloadvalue = T_Reload;
        userData.T_Rangevalue = T_Range;
        userData.H_value = H;
        userData.S_value = S;
        userData.V_value = V;
        userData.E_plusaiming = plusaiming;
        userData.E_plusminattack = plusminattack;
        userData.E_plusdefence = plusdefence;
        userData.E_aimcode = E_aim;
        userData.E_attackcode = E_attack;
        userData.E_defencecode = E_defence;
        userData.C_1 = C_1;
        userData.C_2 = C_2;
        userData.C_3 = C_3;
        userData.Beginnerset = beginnerset;
        userData.saveItemDB = ItemDatabase.instance.itemCountDB;


        //���Ͻ�Ʈ������ userdata.dat ������ ����
        //Application.persistentDataPath�� ����Ƽ���� �̸� ������ ������ ���� ���
        // C:\users\name\appdata\locallow\company name
        //FileMode�� Create�� �ϸ� ���� �̸��� ������ ������ �����
        FileStream file = new FileStream(Application.persistentDataPath + "/userdata.dat", FileMode.Create);

        //���̳ʸ� ���� ��ü ����
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        //userData�� ����ȭ(Serialization)�ؼ� ���̳ʸ� �����Ϳ� ������
        binaryFormatter.Serialize(file, userData);
        //���� �ݱ�
        file.Close();
    }

    void LoadUserData()
    {
        //File.Exists = ������ ��ο� �����ϴ��� Ȯ��
        if(File.Exists(Application.persistentDataPath + "/userdata.dat"))
        {
            //������ ����(Open) ������ȭ�� �� UserData Ÿ������ ��ȯ �� userData�� ����
            FileStream file = new FileStream(Application.persistentDataPath + "/userdata.dat", FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            userData = (UserData)binaryFormatter.Deserialize(file);
            //���ϴݱ�
            file.Close();
        }
        else
        {
            //�ش� ������ ������ �� UserData ��ü�� ���� ����
            userData = new UserData();
        }
    }
    #endregion

    #region Button Methods

    public void GameExit()
    {
        Application.Quit();
    }
    public void ButtonAudio()
    {
        audiosource.PlayOneShot(buttonaudio);
    }
    public void EquipAudio(int i)
    {
        if (i == 0)
            audiosource.PlayOneShot(equipaudio);
        else if (i == 1)
            audiosource.PlayOneShot(equipoffaudio);
    }

    public void BattleSelect()
    {
        ButtonAudio();
        battleselectbutton.SetActive(true);
    }

    public void BattleSelectCansle()
    {
        ButtonAudio();
        battleselectbutton.SetActive(false);
    }
    
    //������
    public void Battle1Start()
    {
        SaveUserData();
        ButtonAudio();
        SceneManager.LoadScene("Battle1");
    }

    public void GoToLobby()
    {
        SaveUserData();
        ButtonAudio();
        SceneManager.LoadScene("Main");
    }

    //�÷�����
    public void ColorOK()
    {
        ButtonAudio();
        T_Mat.color = T_Color;
    }

    public void ColorChange(Color color)
    {
        ButtonAudio();
        if (gem < 5)
        {
            WarningPopUp.SetActive(true);
            WariningText.text = "������ �����մϴ�!";
            return;
        }
        gem -= 5;
        gemtext.text = gem.ToString();
        T_Mat.color = color;
        T_Color = color;       
        Color.RGBToHSV(T_Color, out H, out S, out V);
        SaveUserData();
    }
    //���׷��̵� ����
    public void UpgradeResetWarning()
    {
        ButtonAudio();
        ResetPopUp.SetActive(true);
        ReturnText.text = "ȹ�� ����Ʈ : " + (playerLevel-1)* 10;
    }
    public void UpgradeReset()
    {
        ButtonAudio();
        ResetPopUp.SetActive(false);
        for(int i = 0; i < Upgrades.Length; i++)
        {
            Uvalues[i] = 0;
            Upgrades[i].value = Uvalues[i];
            Uptexts[i].text = Uvalues[i].ToString() + " / 10";
        }
        T_Health = 100f;
        T_Speed = 15f;
        T_Attack = 100f;
        T_Reload = 1f;
        T_Range = 20f;
        SP = (playerLevel-1) * 10;
        remainSP.text = "���� ����Ʈ : " + SP; 
        SaveUserData();
    }

    public void whenplusButtonPush(int i)
    {
        uptype = i;
        ButtonAudio();
        upgradeDetail.SetActive(true);
        UpgradeCost = (Uvalues[i] + 1) * 300;
        UpgradeCostText.text = UpgradeCost.ToString();
        upiconbefore.sprite = UpIcons[i];
        upiconafter.sprite = UpIcons[i];
        needSP.text = "�ʿ� ����Ʈ : " + (Uvalues[i] + 1).ToString();
        switch (i)
        {
            case 0:
                Detailtextbefore.text = "ü�� Lv." + Uvalues[i];
                Detailtextafter.text = "ü�� Lv." + (Uvalues[i] + 1);
                Detailvaluebefore.text = T_Health.ToString();
                Detailvalueafter.text = (T_Health+plushealth).ToString();
                Addedvalue.text = "(" + "+" + plushealth + ")";
                break;
            case 1:
                Detailtextbefore.text = "�̵��ӵ� Lv." + Uvalues[i];
                Detailtextafter.text = "�̵��ӵ� Lv." + (Uvalues[i] + 1);
                Detailvaluebefore.text = T_Speed.ToString("F1");
                Detailvalueafter.text = (T_Speed + plusspeed).ToString("F1");
                Addedvalue.text = "(" + "+" + plusspeed + ")";
                break;
            case 2:
                Detailtextbefore.text = "���ݷ� Lv." + Uvalues[i];
                Detailtextafter.text = "���ݷ� Lv." + (Uvalues[i] + 1);
                Detailvaluebefore.text = T_Attack.ToString();
                Detailvalueafter.text = (T_Attack + plusattack).ToString();
                Addedvalue.text = "(" + "+" + plusattack + ")";
                break;
            case 3:
                Detailtextbefore.text = "������ Lv." + Uvalues[i];
                Detailtextafter.text = "������ Lv." + (Uvalues[i] + 1);
                Detailvaluebefore.text = T_Reload.ToString("F2");
                Detailvalueafter.text = (T_Reload + plusreload).ToString("F2");
                Addedvalue.text = "(" + plusreload + ")";
                break;
            case 4:
                Detailtextbefore.text = "��Ÿ� Lv." + Uvalues[i];
                Detailtextafter.text = "��Ÿ� Lv." + (Uvalues[i] + 1);
                Detailvaluebefore.text = T_Range.ToString("F1");
                Detailvalueafter.text = (T_Range + plusrange).ToString("F1");
                Addedvalue.text = "(" + "+" + plusrange + ")";
                break;
            default:
                break;
        }

    }
    public void whencloseButtonPush()
    {
        ButtonAudio();
        upgradeDetail.SetActive(false);
        WarningPopUp.SetActive(false);
        ResetPopUp.SetActive(false);
    }
    public void UpgradePlus()
    {
        ButtonAudio();
        if(Uvalues[uptype] >= 10)
        {
            upgradeDetail.SetActive(false);
            WarningPopUp.SetActive(true);
            WariningText.text = "�ִ�ġ�� �����߽��ϴ�!";
            return;
        }
        if (Uvalues[uptype] + 1 > SP)
        {
            upgradeDetail.SetActive(false);
            WarningPopUp.SetActive(true);
            WariningText.text = "����Ʈ�� �����մϴ�!";
            return;
        }
        if(gold < UpgradeCost)
        {
            upgradeDetail.SetActive(false);
            WarningPopUp.SetActive(true);
            WariningText.text = "��尡 �����մϴ�!";
            return;
        }
        gold -= (int)UpgradeCost;
        SP -= (Uvalues[uptype] + 1);
        goldtext.text = gold.ToString();
        remainSP.text = "���� ����Ʈ : "+SP.ToString();
        Uvalues[uptype]++;
        Upgrades[uptype].value = Uvalues[uptype];
        Uptexts[uptype].text = Uvalues[uptype].ToString() + " / 10";
        switch (uptype)
        {
            case 0:
                T_Health += plushealth;
                break;
            case 1:
                T_Speed += plusspeed;
                break;
            case 2:
                T_Attack += plusattack;
                break;
            case 3:
                T_Reload += plusreload;
                break;
            case 4:
                T_Range += plusrange;
                break;
            default:
                break;
        }
        SaveUserData();
        upgradeDetail.SetActive(false);
    }

    //�κ��丮 ����
    public void InventoryOnOff(bool ox)
    {
        ButtonAudio();
        invenui.SetActive(ox);
    }
    public void ShopOnOff(bool ox)
    {
        ButtonAudio();
        shopui.SetActive(ox);
    }
    #endregion

    #region Consumable Methods

    public void TankHeal(int amount)
    {
        tankhealth.TankHealing(amount);
    }
    #endregion

    #region Interface Methods
    public void UpdateData()
    {
        ColorChange(colorPicker.linkedObject.color);
    }
    #endregion

    #endregion
}

//UserData Ŭ������ ����ȭ�� �����ϵ��� SeralizableAttribute Ư������ ǥ����
//https://hub1234.tistory.com/26
//����ȭ ����
[Serializable]
class UserData
{
    public int playerlevel = 1;
    public int gold = 0;
    public int gem = 0;
    public int maxexp = 100;
    public float expvalue = 0;
    public int HPvalue = 0;
    public int Speedvalue = 0;
    public int Attackvalue = 0;
    public int Reloadvalue = 0;
    public int Rangevalue = 0;
    public int SPvalue = 0;
    public float T_Healthvalue = 100f;
    public float T_Speedvalue = 15f;
    public float T_Attackvalue = 100f;
    public float T_Reloadvalue = 1f;
    public float T_Rangevalue = 20f;
    public float H_value = 1;
    public float S_value = 1;
    public float V_value = 1;
    public int E_plusaiming = 0;
    public int E_plusminattack = 0;
    public int E_plusdefence = 0;
    public string Haveitem = "000000000000000";
    public int[] saveItemDB = new int[15] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    public int E_aimcode = 9999;
    public int E_attackcode = 9999;
    public int E_defencecode = 9999;
    public int C_1 = 9999;
    public int C_2 = 9999;
    public int C_3 = 9999;
    public bool Beginnerset = false;
}
