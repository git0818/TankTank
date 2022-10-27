using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace m_Camera
{   public class Camera_Follow : MonoBehaviour
    {
        #region Variables
        [Header("Follow Target")]
        public GameObject target;
        public TankDemo.Tank_Controller controller;
        private float Xlook = 0;
        public FloatingJoystick R_joystick;
        #endregion

        void Update()
        {
            Xlook = R_joystick.Horizontal*10f;
            transform.position = new Vector3(target.transform.position.x + Xlook,
                target.transform.position.y + 15f, target.transform.position.z -15f);
            transform.rotation = Quaternion.Euler(330f, Xlook, 0);
              
        }
    }
}
