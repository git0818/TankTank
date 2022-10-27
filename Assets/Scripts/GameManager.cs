using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq.Expressions;
using UnityEngine.SceneManagement;
//데이터 저장
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

    //인게임 소모품 함수에 씀
    private GameObject player;
    private Tank_Health tankhealth;
    public bool guided = false;
    public float minustimer = 0;
    public AudioClip MineSetSound;
    public AudioClip MineBoomSound;
    public AudioClip TimerSound;
    public AudioClip HealSound;
    public AudioClip GuidedSound;


    //버튼 매개변수
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


        //로비
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
            remainSP.text = "남은 포인트 : " + SP;
            shopgoldtext.text = goldtext.text;
            shopgemtext.text = gemtext.text;
        }

        //인게임
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
        Debug.Log("인벤토리ui 켜져라");
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


        //파일스트림으로 userdata.dat 파일을 생성
        //Application.persistentDataPath는 유니티에서 미리 지정된 데이터 저장 경로
        // C:\users\name\appdata\locallow\company name
        //FileMode를 Create로 하면 같은 이름의 파일이 있으면 덮어씌움
        FileStream file = new FileStream(Application.persistentDataPath + "/userdata.dat", FileMode.Create);

        //바이너리 포맷 객체 생셩
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        //userData를 직렬화(Serialization)해서 바이너리 포매터에 저장함
        binaryFormatter.Serialize(file, userData);
        //파일 닫기
        file.Close();
    }

    void LoadUserData()
    {
        //File.Exists = 파일이 경로에 존재하는지 확인
        if(File.Exists(Application.persistentDataPath + "/userdata.dat"))
        {
            //파일을 열고(Open) 역직렬화한 후 UserData 타입으로 변환 후 userData에 저장
            FileStream file = new FileStream(Application.persistentDataPath + "/userdata.dat", FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            userData = (UserData)binaryFormatter.Deserialize(file);
            //파일닫기
            file.Close();
        }
        else
        {
            //해당 파일이 없으면 새 UserData 객체를 만들어서 저장
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
    
    //섬멸전
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

    //컬러관련
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
            WariningText.text = "보석이 부족합니다!";
            return;
        }
        gem -= 5;
        gemtext.text = gem.ToString();
        T_Mat.color = color;
        T_Color = color;       
        Color.RGBToHSV(T_Color, out H, out S, out V);
        SaveUserData();
    }
    //업그레이드 관련
    public void UpgradeResetWarning()
    {
        ButtonAudio();
        ResetPopUp.SetActive(true);
        ReturnText.text = "획득 포인트 : " + (playerLevel-1)* 10;
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
        remainSP.text = "남은 포인트 : " + SP; 
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
        needSP.text = "필요 포인트 : " + (Uvalues[i] + 1).ToString();
        switch (i)
        {
            case 0:
                Detailtextbefore.text = "체력 Lv." + Uvalues[i];
                Detailtextafter.text = "체력 Lv." + (Uvalues[i] + 1);
                Detailvaluebefore.text = T_Health.ToString();
                Detailvalueafter.text = (T_Health+plushealth).ToString();
                Addedvalue.text = "(" + "+" + plushealth + ")";
                break;
            case 1:
                Detailtextbefore.text = "이동속도 Lv." + Uvalues[i];
                Detailtextafter.text = "이동속도 Lv." + (Uvalues[i] + 1);
                Detailvaluebefore.text = T_Speed.ToString("F1");
                Detailvalueafter.text = (T_Speed + plusspeed).ToString("F1");
                Addedvalue.text = "(" + "+" + plusspeed + ")";
                break;
            case 2:
                Detailtextbefore.text = "공격력 Lv." + Uvalues[i];
                Detailtextafter.text = "공격력 Lv." + (Uvalues[i] + 1);
                Detailvaluebefore.text = T_Attack.ToString();
                Detailvalueafter.text = (T_Attack + plusattack).ToString();
                Addedvalue.text = "(" + "+" + plusattack + ")";
                break;
            case 3:
                Detailtextbefore.text = "재장전 Lv." + Uvalues[i];
                Detailtextafter.text = "재장전 Lv." + (Uvalues[i] + 1);
                Detailvaluebefore.text = T_Reload.ToString("F2");
                Detailvalueafter.text = (T_Reload + plusreload).ToString("F2");
                Addedvalue.text = "(" + plusreload + ")";
                break;
            case 4:
                Detailtextbefore.text = "사거리 Lv." + Uvalues[i];
                Detailtextafter.text = "사거리 Lv." + (Uvalues[i] + 1);
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
            WariningText.text = "최대치에 도달했습니다!";
            return;
        }
        if (Uvalues[uptype] + 1 > SP)
        {
            upgradeDetail.SetActive(false);
            WarningPopUp.SetActive(true);
            WariningText.text = "포인트가 부족합니다!";
            return;
        }
        if(gold < UpgradeCost)
        {
            upgradeDetail.SetActive(false);
            WarningPopUp.SetActive(true);
            WariningText.text = "골드가 부족합니다!";
            return;
        }
        gold -= (int)UpgradeCost;
        SP -= (Uvalues[uptype] + 1);
        goldtext.text = gold.ToString();
        remainSP.text = "남은 포인트 : "+SP.ToString();
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

    //인벤토리 관련
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

//UserData 클래스의 직렬화가 가능하도록 SeralizableAttribute 특성으로 표시함
//https://hub1234.tistory.com/26
//직렬화 설명
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
