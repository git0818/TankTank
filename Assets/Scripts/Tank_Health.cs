using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TankDemo
{
    public class Tank_Health : MonoBehaviour
    {
        #region Variables
        [Header("Health Properties")]
        private float StartingHealth;
        public Slider Slider;
        public Image FillImage;
        public Canvas canvas;
        public GameObject damageText;
        public GameObject healthText;
        public TextMeshProUGUI hpText; 
        private float CurrentHealth;
        private bool Dead;

        public GameObject diesound;
        #endregion

        #region Builtin Methods

        private void OnEnable()
        {
            StartingHealth = GameManager.instance.T_Health;
            CurrentHealth = StartingHealth;
            Debug.Log("체력" + CurrentHealth);
            Debug.Log("방어력" + GameManager.instance.plusdefence);
            SetHealthUI();
            Dead = false;

            hpText.text = CurrentHealth.ToString();
        }
        void Update()
        {
            Slider.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("Deadzone"))
            {
                TakeDamage(1000);
            }
        }
        #endregion

        #region Custom Methods

        public void TakeDamage(float amount)
        {
            amount -= GameManager.instance.plusdefence;
            CurrentHealth -= amount;
            SetHealthUI();

            Vector3 dmgpos = canvas.transform.position + new Vector3(0, 3f, 0);
            GameObject dmg = Instantiate(damageText, dmgpos, damageText.transform.rotation);
            dmg.GetComponent<DamageText>().damage = (int)amount;
            dmg.transform.SetParent(canvas.transform);
            dmg.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
            hpText.text = CurrentHealth.ToString();

            if (CurrentHealth <= 0f && !Dead)
            {
                OnDeath();
            }
        }

        public void TankHealing(float amount)
        {
            GameManager.instance.audiosource.PlayOneShot(GameManager.instance.HealSound);
            CurrentHealth += amount;
            if (CurrentHealth >= StartingHealth)
                CurrentHealth = StartingHealth;
            SetHealthUI();
            Vector3 healpos = canvas.transform.position + new Vector3(0, 3f, 0);
            GameObject heal = Instantiate(healthText, healpos, damageText.transform.rotation);
            heal.GetComponent<DamageText>().damage = (int)amount;
            heal.transform.SetParent(canvas.transform);
            heal.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
            hpText.text = CurrentHealth.ToString();
        }

        private void SetHealthUI()
        {
            Slider.maxValue = StartingHealth;
            Slider.value = CurrentHealth;
        }

        private void OnDeath()
        {
            hpText.text = "0";
            Dead = true;
            Instantiate(diesound);
            GameManager.instance.PlayerDead();
            gameObject.SetActive(false);
        }
        #endregion
    }

}