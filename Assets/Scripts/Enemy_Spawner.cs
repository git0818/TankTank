using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemy_Spawner : MonoBehaviour
{
    #region Variables
    public Transform[] spawnPoints;
    public Slider slider;
    public TextMeshProUGUI stageText;
    public TextMeshProUGUI monsterText;
    public TextMeshProUGUI stagegoalText;
    public Monster_ObjectPool[] Pools;
    public GameObject waveeffect;
    public Image waveimage;
    public TextMeshProUGUI waveText;

    private List<Enemy.Enemy_HealthNMove> enemies = new List<Enemy.Enemy_HealthNMove>();
    private int wave = 0;
    private int spawnCount = 0;
    private bool check = false;
    #endregion

    #region Builtin Methods
    void Update()
    {
        if(enemies.Count <= 0 && check == false)
        {
            SpawnWave();
        }
    }


    #endregion


    #region Custom Methods

    private void SpawnWave()
    {
        check = true;
        StartCoroutine("FadeIn");
        StartCoroutine(FadeOut(1.6f));
        wave++;
        waveText.text = "WAVE  " + wave.ToString(); 
        //kill = 0;
        slider.value = 0;
        spawnCount = Mathf.RoundToInt(wave * 1.5f);
        slider.maxValue = spawnCount;
        monsterText.text = "0";
        stageText.text = "Wave " + wave.ToString();
        stagegoalText.text = spawnCount.ToString();
        StartCoroutine("CreateEnemy");
    }

    IEnumerator FadeIn()
    {
        waveeffect.SetActive(true);
        for(int i = 0; i < 30; i++)
        {
            float f = i / 30.0f;
            Color c = waveimage.color;
            Color tc = waveText.color;
            c.a = f;
            tc.a = f;
            waveimage.color = c;
            waveText.color = tc;
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator FadeOut(float timer)
    {
        yield return new WaitForSeconds(timer);
        for (int i = 30; i >= 0; i--)
        {
            float f = i / 30.0f;
            Color c = waveimage.color;
            Color tc = waveText.color;
            c.a = f;
            tc.a = f;
            waveimage.color = c;
            waveText.color = tc;
            yield return new WaitForSeconds(0.05f);
        }
        waveeffect.SetActive(false);
    }

    IEnumerator CreateEnemy()
    {
        int i = 0;
        while (true)
        {
            if (i >= spawnCount)
            {
                check = false;
                break;
            }
            yield return new WaitForSeconds(0.1f);
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            int type = Random.Range(0, 8);
            var enemy = Pools[type].GetObject();
            enemy.gameObject.transform.position = spawnPoint.position;
            enemy.gameObject.transform.rotation = spawnPoint.rotation;
            enemies.Add(enemy);
            Enemy.Enemy_HealthNMove enemyhealth = enemy.gameObject.GetComponent<Enemy.Enemy_HealthNMove>();
            //enemyhealth.onDeath += () => kill++;
            enemyhealth.onDeath += () => enemies.Remove(enemy);
            enemyhealth.onDeath += () => monsterText.text = (spawnCount - enemies.Count).ToString();
            enemyhealth.onDeath += () => slider.value = (spawnCount - enemies.Count);
            enemyhealth.onPool += () => Pools[type].ReturnObject(enemy);
            i++;
        }
    }
    /*
    private void CreateEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        int type = Random.Range(0, 8);
        var enemy = Pools[type].GetObject();
        enemy.gameObject.transform.position = spawnPoint.position;
        enemy.gameObject.transform.rotation = spawnPoint.rotation;
        enemies.Add(enemy);
        Enemy.Enemy_HealthNMove enemyhealth = enemy.gameObject.GetComponent<Enemy.Enemy_HealthNMove>();
        enemyhealth.onDeath += () => kill++;
        enemyhealth.onDeath += () => enemies.Remove(enemy);
        enemyhealth.onDeath += () => monsterText.text = (spawnCount - enemies.Count).ToString();
        enemyhealth.onDeath += () => slider.value = kill;
        enemyhealth.onPool += () => Pools[type].ReturnObject(enemy);
    }
    */

    #endregion
}
