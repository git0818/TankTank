using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TankDemo
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Tank_Input))]
    public class Tank_Controller : MonoBehaviour
    {
        #region Variables
        [Header("Movement Properties")]
        public float tankSpeed;
        public float tankRotationSpeed = 20f;

        [Header("Turret Properties")]
        public Transform turretTransform;
        public float turretLegSpeed;
        public float distanceCheck;

        [Header("Reticle Properties")]
        public Transform reticleTransform;

        [Header("Sound Properties")]
        public AudioSource tankAudio;
        public AudioClip tankengineSound;
        private float originalPitch;

        private Rigidbody rb;
        private Tank_Input input;
        private Vector3 finalTurretLookDir;
        private Transform target;

        [Header("Tank Particles")]
        [SerializeField]
        private GameObject movingdust;
        private float timer;
        #endregion

        #region Builtin Methods
        void Start()
        {
            turretLegSpeed = 5f + GameManager.instance.plusaiming;
            Debug.Log("조준속도" + turretLegSpeed);
            tankSpeed = GameManager.instance.T_Speed;
            distanceCheck = GameManager.instance.T_Range;
        rb = GetComponent<Rigidbody>();
            input = GetComponent<Tank_Input>();
            originalPitch = tankAudio.pitch;
            Debug.Log("사거리" + distanceCheck);
            Debug.Log("이동속도" + tankSpeed);
        }

        private void Update()
        {
            EngineAudio();
        }
        void FixedUpdate()
        {
            if (GameManager.instance.isGameover == false)
            {
                if (rb && input)
                {
                    HandleMovement();
                    HandleTurret();
                    //HandleReticle();
                }
            }
        }
        #endregion


        #region Custom Methods

        private void EngineAudio()
        {
            float newPitch = tankAudio.pitch;

            if(input.HInput != 0 || input.VInput != 0)
            {
                if (tankAudio.pitch < 1f)
                    tankAudio.pitch += 2f * Time.deltaTime;
                else
                    tankAudio.pitch = 1f;

                //흙먼지파티클
                movingdust.SetActive(true);
            }
            else
            {
                tankAudio.pitch = Mathf.Lerp(newPitch, originalPitch, 10f * Time.deltaTime);
                timer += Time.deltaTime;
                if (timer >= 3f)
                {
                    timer = 0f;
                    movingdust.SetActive(false);
                }
            }
        }
        protected virtual void HandleMovement()
        {
            //Move Tank Forward
            Vector3 wantedPosition = transform.position + new Vector3(input.HInput * tankSpeed * Time.deltaTime, 0, input.VInput*tankSpeed *Time.deltaTime);
            rb.MovePosition(wantedPosition);

            if( input.HInput != 0f || input.VInput != 0f)
            {
                Vector3 dir = input.HInput * Vector3.right + input.VInput * Vector3.forward;
                transform.rotation = Quaternion.LookRotation(dir);
                
            }
            //Rotate the Tank
            //Quaternion wantedRotation = transform.rotation * Quaternion.Euler(Vector3.up * tankRotationSpeed * input.RotationInput * Time.deltaTime);
            //rb.MoveRotation(wantedRotation);
        }

        protected virtual void HandleTurret()
        {
            /*
            if(turretTransform)
            {
                Vector3 turretLookDir = input.ReticlePosition - turretTransform.position;
                turretLookDir.y = 0f;

                finalTurretLookDir = Vector3.Slerp(finalTurretLookDir, turretLookDir, Time.deltaTime * turretLegSpeed);
                turretTransform.rotation = Quaternion.LookRotation(finalTurretLookDir);
            }
            */


            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, distanceCheck, Vector3.up, 0f, LayerMask.GetMask("Enemy"));
            float minDistance = Mathf.Infinity;
            float dist;
            Transform closestenemy = null;
            foreach(RaycastHit hit in rayHits)
            {
                Vector3 objectPos = hit.transform.position;
                dist = (objectPos - transform.position).sqrMagnitude;
                if(dist < minDistance)
                {
                    minDistance = dist;
                    closestenemy = hit.transform;
                }
            }
            target = closestenemy;

            if (closestenemy != null && turretTransform != null)
            {
                Vector3 turretLookDir = target.transform.position - turretTransform.position;
                turretLookDir.y = 0f;

                turretLookDir.Normalize();

                finalTurretLookDir = Vector3.Lerp(finalTurretLookDir, turretLookDir, Time.deltaTime * turretLegSpeed);
                turretTransform.rotation = Quaternion.LookRotation(finalTurretLookDir);
                //turretTransform.rotation = Quaternion.LookRotation(turretLookDir);
            }


        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, distanceCheck);
        }

        //protected virtual void HandleReticle()
        //{
        //if(reticleTransform)
        //    {
        //        reticleTransform.position = input.ReticlePosition;
        //    }
        //}

        #endregion
    }
}
