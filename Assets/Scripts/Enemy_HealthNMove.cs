using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System;

namespace Enemy
{
    public class Enemy_HealthNMove : MonoBehaviour
    {
        #region Variables
        public event Action onDeath;
        public event Action onPool;


        [Header("Health Properties")]
        public float StartingHealth = 100f;
        public Slider Slider;
        public Image FillImage;
        public GameObject damageText;
        public Canvas canvas;
        public float damageHeight = 3f;
        public GameObject minimapicon;
        private float CurrentHealth;
        bool Dead = false;

        [Header("NavMesh Properties")]
        public LayerMask targetLayer;
        private GameObject target;
        private NavMeshAgent pathFinder;


        [Header("Attack Properties")]
        public BoxCollider meleeArea;
        public bool isAttack;
        public bool isChase;
        public GameObject bullet;
        public float dashforce;
        public float dashrange;


        [Header("Mosnter Sound")]
        public AudioSource monsterAudio;
        public AudioClip monsterDeathSound;

        [Header("Ghost EffectPrefab")]
        [SerializeField]
        private ParticleSystem G_effect;

        [Header("Element EffectPrefab")]
        [SerializeField]
        private ParticleSystem E_effect;
        [SerializeField]
        private AudioClip E_bombsound;

        [Header("Die EffectPrefab")]
        [SerializeField]
        private ParticleSystem DieEffect;

        //Monster Type
        public enum Type { A, B, C, D, E };
        public Type enemyType;

        Rigidbody rb;
        BoxCollider boxCollider;
        Animator animator;

        private bool hasTarget
        {
            get
            {
                if (target != null)
                    return true;

                return false;
            }
        }
        #endregion

        #region Builtin Methods


        void Awake()
        {
            target = FindObjectOfType<TankDemo.Tank_Health>().gameObject;
            pathFinder = GetComponent<NavMeshAgent>();
            onDeath += Die;
            rb = GetComponent<Rigidbody>();
            boxCollider = GetComponent<BoxCollider>();
            animator = GetComponent<Animator>();
            Slider.maxValue = StartingHealth;
        }

        private void OnEnable()
        {
            pathFinder.enabled = true;
            boxCollider.enabled = true;
            CurrentHealth = StartingHealth;
            Dead = false;
            SetHealthUI();
            StartCoroutine(UpdatePath());
            isChase = true;
            minimapicon.SetActive(true);
            animator.SetTrigger("Walk");

        }

        private void Update()
        {
            Slider.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        }

        private void FixedUpdate()
        {
            if (!Dead)
            {
                Targeting();
                FreezeVelocity();
            }
        }

        #endregion

        #region Custom Methods

        public void Damage(float amount)
        {
            CurrentHealth -= amount;
            Vector3 dmgpos = canvas.transform.position + new Vector3(0, damageHeight, 0);
            GameObject dmg = Instantiate(damageText, dmgpos, damageText.transform.rotation);
            dmg.GetComponent<DamageText>().damage = (int)amount;
            dmg.transform.SetParent(canvas.transform);
            dmg.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
            SetHealthUI();

            if (CurrentHealth <= 0f && !Dead)
            {
                onDeath();
                Invoke("ReturnPool", 3f);
            }
        }

        private void ReturnPool()
        {
            onPool();
        }
        private void SetHealthUI()
        {
            Slider.value = CurrentHealth;
        }

        private void Targeting()
        {
            float targetRadius = 0;
            float targetRange = 0;

            switch(enemyType)
            {
                case Type.A:
                    targetRadius = 1.5f;
                    targetRange = 3f;
                    break;
                case Type.B:
                    targetRadius = 1f;
                    targetRange = dashrange;
                    break;
                case Type.C:
                    targetRadius = 0.5f;
                    targetRange = 30f;
                    break;
                case Type.D:
                    targetRadius = 3f;
                    targetRange = 30f;
                    break;
                case Type.E:
                    targetRadius = 1f;
                    targetRange = 3f;
                    break;
            }

            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, targetLayer);
            //Debug.DrawRay(transform.position, transform.forward * targetRange, Color.green);
            if(rayHits.Length > 0 && !isAttack)
            {
                StartCoroutine(Attack());
            }
        }


        private void Die()
        {
            StopAllCoroutines();
            if (DieEffect != null)
            {
                DieEffect.Play();
            }
            monsterAudio.PlayOneShot(monsterDeathSound);
            animator.SetTrigger("Death");
            Dead = true;
            pathFinder.enabled = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            boxCollider.enabled = false;
            minimapicon.SetActive(false);
            switch (enemyType)
            {
                case Type.A:
                    GameManager.instance.AddScore(100);
                    break;
                case Type.B:
                    GameManager.instance.AddScore(500);
                    break;
                case Type.C:
                    GameManager.instance.AddScore(200);
                    break;
                case Type.D:
                    GameManager.instance.AddScore(50);
                    break;
                case Type.E:
                    GameManager.instance.AddScore(300);
                    break;

            }
            //Destroy(gameObject, 3f);
        }

        private void FreezeVelocity()
        {
            if(isChase)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
        private IEnumerator UpdatePath()
        {
            while (!Dead)
            {
                if (hasTarget)
                {
                    if (pathFinder.enabled)
                    {
                        pathFinder.SetDestination(target.transform.position);
                        pathFinder.isStopped = !isChase;
                    }
                }
                else
                    {
                    if (pathFinder.enabled)
                        pathFinder.isStopped = true;
                    }
                    yield return new WaitForSeconds(0.25f);
            }
        }
        private Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance)
        {
            Vector3 randomPos = UnityEngine.Random.insideUnitSphere * distance + center;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, distance, NavMesh.AllAreas);

            return hit.position;
        }

        IEnumerator StartShake()
        {
            while (true)
            {
                transform.localScale *= 1.01f;
                yield return new WaitForSeconds(0.01f);
            }
        }

        TankDemo.Tank_Input tankinput;
        private IEnumerator Attack()
        {
            if (!Dead)
            {
                isChase = false;
                isAttack = true;

                switch(enemyType)
                {
                    case Type.A:
                        animator.SetTrigger("Attack");
                        yield return new WaitForSeconds(0.2f);
                        meleeArea.enabled = true;

                        yield return new WaitForSeconds(0.5f);
                        meleeArea.enabled = false;
                        break;
                    case Type.B:
                        yield return new WaitForSeconds(0.3f);
                        animator.SetTrigger("Run");
                        rb.AddForce(transform.forward * dashforce, ForceMode.Impulse);
                        meleeArea.enabled = true;
                        yield return new WaitForSeconds(0.5f);
                        meleeArea.enabled = false;
                        animator.SetTrigger("Idle 2");
                        rb.velocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                        yield return new WaitForSeconds(2f);
        
                        break;
                    case Type.C:
                        animator.SetTrigger("Hit");
                        yield return new WaitForSeconds(0.2f);
                        Vector3 newpos = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
                        GameObject instantBullet = Instantiate(bullet, newpos, transform.rotation);
                        Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
                        if (rigidBullet != null)
                            Destroy(instantBullet, 5f);
                        yield return new WaitForSeconds(0.3f);
                        rigidBullet.velocity = transform.forward * 20f;

                        animator.SetTrigger("Idle");
                        yield return new WaitForSeconds(2f);

                        break;

                    case Type.D:
                        Vector3 teleportPos;
                        while (true)
                        {
                            teleportPos = GetRandomPointOnNavMesh(target.transform.position, 3f);
                            Debug.Log(teleportPos);
                            if (!float.IsInfinity(teleportPos.x)) break;
                        }
                        yield return new WaitForSeconds(0.2f);
                        if (G_effect != null)
                        {
                            G_effect.Play();
                        }
                        transform.position = teleportPos;
                        
                        animator.SetTrigger("Attack");
                        yield return new WaitForSeconds(0.2f);
                        meleeArea.enabled = true;
                        yield return new WaitForSeconds(0.5f);
                        meleeArea.enabled = false;
                        animator.SetTrigger("Hit");
                        yield return new WaitForSeconds(2f);
                        break;
                    case Type.E:
                        Vector3 s = transform.localScale;
                        animator.SetTrigger("Idle");
                        rb.velocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                        StartCoroutine("StartShake");
                        yield return new WaitForSeconds(1f);
                        meleeArea.enabled = true;
                        if (E_effect != null)
                        {
                            E_effect.Play();
                        }
                        StopCoroutine("StartShake");
                        transform.localScale = s;
                        Invoke("elementmeleeArea", 0.5f);
                        tankinput = FindObjectOfType<TankDemo.Tank_Input>();
                        tankinput.Shake();
                        monsterAudio.PlayOneShot(E_bombsound);
                        Damage(100);
                        yield return new WaitForSeconds(0.1f);
                        break;

                }

                //yield return new WaitForSeconds(1f);
                isChase = true;
                isAttack = false;
                animator.SetTrigger("Walk");
            }

        }

        void elementmeleeArea()
        {
            meleeArea.enabled = false;
        }
        #endregion
    }
}
