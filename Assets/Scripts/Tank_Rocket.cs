using Enemy;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace TankDemo
{
    public class Tank_Rocket : MonoBehaviour
    {
        #region Variables
        public LayerMask TankMask;
        public float MaxDamage;
        public float MaxLifeTime = 2f;
        public float ExplosionRadius = 5f;
        public GameObject hiteffect;
        public GameObject fireTrail;
        private bool check = false;
        private Rigidbody rb;
        Vector3 LookDir;

        #endregion


        #region Builtin Methods

        void OnEnable()
        {
            check = false;
            rb = GetComponent<Rigidbody>();
            MaxDamage = GameManager.instance.T_Attack;
            Debug.Log("데미지" + MaxDamage);
            StartCoroutine("C_destroyrocket");
            //Destroy(gameObject, MaxLifeTime);
            StartCoroutine("firetrail");
            Debug.Log("추가최소뎀 " + GameManager.instance.plusminattack);
        }

        IEnumerator firetrail()
        {
            fireTrail.SetActive(false);
            yield return new WaitForSeconds(0.01f);
            fireTrail.SetActive(true);
        }

        IEnumerator C_destroyrocket()
        {
            yield return new WaitForSeconds(MaxLifeTime);
            Rocket_ObjectPool.ReturnObject(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.gameObject);
            if (other.gameObject.CompareTag("Enemy"))
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius, TankMask);

                foreach (Collider collider in colliders)
                {
                    Rigidbody targetrb = collider.GetComponent<Rigidbody>();
                    if (!targetrb)
                        continue;

                    //targetrb.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);
                    //TankDemo.Tank_Health targetHealth = targetrb.GetComponent<TankDemo.Tank_Health>();
                    Enemy.Enemy_HealthNMove targetHealth = targetrb.GetComponent<Enemy.Enemy_HealthNMove>();
                    if (!targetHealth)
                        continue;

                    float damage = CalculateDamage(targetrb.position);
                    //targetHealth.TakeDamage(damage);
                    targetHealth.Damage(damage);
                }
                var hiteff = RocketHitEffect_ObjectPool.GetObject();
                hiteff.transform.position = transform.position;
                hiteff.transform.rotation = transform.rotation;
                DestroyRocket();
            }
            else if (other.gameObject.CompareTag("Map"))
            {
                var hiteff = RocketHitEffect_ObjectPool.GetObject();
                hiteff.transform.position = transform.position;
                hiteff.transform.rotation = transform.rotation;
                DestroyRocket();
            }
            else return;
            //Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            if (GameManager.instance.guided == true)
            {
                RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 30f, Vector3.up, 0f, LayerMask.GetMask("Enemy"));
                foreach (RaycastHit hit in rayHits)
                {
                    Enemy_HealthNMove e = hit.collider.GetComponent<Enemy_HealthNMove>();
                    Rigidbody r = hit.collider.GetComponent<Rigidbody>();
                    if (e != null && r != null)
                    {
                        if (check == false)
                        {
                            check = true;
                            rb.velocity *= 0.5f;
                        }
                        Vector3 grab = new Vector3(hit.transform.position.x, hit.transform.position.y + 0.5f, hit.transform.position.z);
                        transform.position = Vector3.MoveTowards(transform.position, grab, Time.deltaTime * 30f);
                        Vector3 rocketLookDir = hit.transform.position - transform.position;
                        rocketLookDir.Normalize();
                        Debug.Log(rocketLookDir);
                        Quaternion q = Quaternion.LookRotation(rocketLookDir, Vector3.up);
                        Quaternion p = Quaternion.Euler(new Vector3(0, 270f, 0));
                        transform.rotation = q * p;
                        //transform.rotation = Quaternion.Euler(rocketLookDir.x, rocketLookDir.y, rocketLookDir.z);
                        //transform.rotation = Quaternion.LookRotation(rocketLookDir);
                    }
                }
            }
            else return;
        }
        #endregion

        #region Custom Methods


        private float CalculateDamage(Vector3 targetPos)
        {
            Vector3 explosionToTarget = targetPos - transform.position;

            float explosionDis = explosionToTarget.magnitude;
            float relativeDis = (ExplosionRadius - explosionDis) / ExplosionRadius;
            float damage = relativeDis * MaxDamage * 3f;
            int plusdmg = GameManager.instance.plusminattack;
            return damage = Mathf.Max(1f + plusdmg, damage);
        }

        void DestroyRocket()
        {
            Rocket_ObjectPool.ReturnObject(this);
        }
        #endregion
    }
}