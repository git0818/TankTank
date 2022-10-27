using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TankDemo
{
   public class Tank_Shooting : MonoBehaviour
    {
        #region Variables
        [Header("Shooting Properties")]
        public Rigidbody Rocket;
        public Transform FireTransform;
        public float LaunchForce = 30f;

        [Header("LaunchUI Properties")]
        public int LaunchCount = 4;
        public GameObject[] LaunchImage;
        public Image backImage;

        [Header("Shooting Sound")]
        public AudioSource tankShootingAudio;
        public AudioClip tankShootingSound;

        [Header("Shooting Effect")]
        public GameObject tankshootingEffect;


        [Header("Guided EffectPrefab")]
        [SerializeField]
        private GameObject GuidedEffect;

        bool guidedcheck = false;
        bool reload = false;
        public float ReloadTime;
        public GameObject mine;
        #endregion


        #region Builtin Methods

        private void Start()
        {
            ReloadTime = GameManager.instance.T_Reload;
            Debug.Log("¿Á¿Â¿¸" + ReloadTime);
            LaunchCount = LaunchImage.Length;
            for(int i = 0; i <LaunchCount; i++)
            {
                LaunchImage[i].SetActive(false);
            }
            GameManager.instance.onusemine += Mine;
        }
        private void Update()
        {
            foreach(GameObject i in LaunchImage)
            {
            i.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
            }
            backImage.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
            if (LaunchCount < 4 && !reload)
            {
                StartCoroutine("Reloading");
            }

            if (GameManager.instance.guided == true)
            {
                if (guidedcheck == false)
                    StartCoroutine("guidedeffect");
            }

        }

        #endregion

        #region Custom Methods

        IEnumerator guidedeffect()
        {
            guidedcheck = true;
            GuidedEffect.SetActive(true);
            yield return new WaitForSeconds(10f);
            GuidedEffect.SetActive(false);
            guidedcheck = false;

        }
        public void Mine()
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            pos.y -= 3f;
            Instantiate(mine, pos, transform.rotation);
        }
        public void Fire()
        {
            if (LaunchCount <= 0 || GameManager.instance.isGameover)
                return;
            Rigidbody rocketInstance = Rocket_ObjectPool.GetObject().GetComponent<Rigidbody>();
            rocketInstance.transform.position = FireTransform.position;
            rocketInstance.transform.rotation = FireTransform.rotation * Quaternion.Euler(0, 270f, 0);
            //Rigidbody rocketInstance = Instantiate(Rocket, FireTransform.position, FireTransform.rotation * Quaternion.Euler(0,270f,0));
            rocketInstance.velocity = LaunchForce * FireTransform.forward;
            LaunchCount--;
            tankShootingAudio.PlayOneShot(tankShootingSound);
            StartCoroutine("shooting");
            FillImage();
        }

        IEnumerator shooting()
        {
            tankshootingEffect.SetActive(true);
            yield return new WaitForSeconds(0.03f);
            tankshootingEffect.SetActive(false);

        }

        IEnumerator Reloading()
        {
            reload = true;
            yield return new WaitForSeconds(ReloadTime - GameManager.instance.minustimer);
            LaunchCount++;
            if (LaunchCount > 4)
                LaunchCount = 4;
            FillImage();
            reload = false;
        }

        private void FillImage()
        {
            for (int i = 0; i < LaunchImage.Length; i++)
            {
                LaunchImage[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < LaunchImage.Length - LaunchCount; i++)
            {
                LaunchImage[i].gameObject.SetActive(true);
            }
        }
        #endregion
    }
}