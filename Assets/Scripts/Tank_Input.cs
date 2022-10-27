using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankDemo
{
    public class Tank_Input : MonoBehaviour
    {
        #region Variables
        [Header("Input Properties")]
        public Camera camera;
        public FloatingJoystick L_joystick;
        public Transform cameraArm;

        #endregion

        #region Properties
        private Vector3 reticlePosition;
        public Vector3 ReticlePosition
        {
            get { return reticlePosition; }
        }

        private Vector3 reticleNormal;
        public Vector3 ReticleNormal
        {
            get { return reticleNormal; }
        }

        private float vInput;
        public float VInput
        {
            get { return vInput; }
        }

        private float hInput;
        public float HInput
        {
            get { return hInput; }
        }
 
        #endregion
        #region Builtin Methods
        void Update()
        {
            if(camera)
            {
                HandleInputs();
                LookAround();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(reticlePosition, 0.5f);
        }
        #endregion



        #region Custom Methods

        protected virtual void HandleInputs()
        {
            //Ray screenRay = camera.ScreenPointToRay(new Vector3(Screen.width/2,Screen.height/2,0));
            //RaycastHit hit;
            //if(Physics.Raycast(screenRay, out hit))
            //{
            //    reticlePosition = hit.point;
            //    reticleNormal = hit.normal;
            //}


            //forwardInput = Input.GetAxis("Vertical");
            //rotationInput = Input.GetAxis("Horizontal");
            vInput = L_joystick.Vertical;
            hInput = L_joystick.Horizontal;
            
        }

        public void LookAround()
        {
            //Vector2 delta = new Vector2(R_joystick.Horizontal, 0);
            cameraArm.transform.position = transform.position;
            //Vector3 camAngle = cameraArm.rotation.eulerAngles;

            //cameraArm.rotation = Quaternion.Euler(camAngle.x, camAngle.y + delta.x, camAngle.z);
        }

        [SerializeField][Range(0.01f, 0.1f)] float shakeRange = 0.07f;
        [SerializeField][Range(0.1f, 1f)] float duration = 1f;

        public void Shake()
        {
            InvokeRepeating("StartShake", 0f, 0.005f);
            Invoke("StopShake", duration);
        }
        void StartShake()
        {
            float cameraPosX = Random.value * shakeRange * 2 - shakeRange;
            float cameraPosY = Random.value * shakeRange * 2 - shakeRange;
            Vector3 shakePos = camera.transform.position;
            shakePos.x += cameraPosX;
            shakePos.y += cameraPosY;
            camera.transform.position = shakePos;
        }

        void StopShake()
        {
            CancelInvoke("StartShake");
            Vector3 oripos = new Vector3(cameraArm.transform.position.x, cameraArm.transform.position.y + 35.73f, cameraArm.transform.position.z-10.9f);
            camera.transform.position = oripos;
        }


        #endregion
    }
}
