using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackDamage : MonoBehaviour
{
    [Header("If this is CiclopBullet")]
    [SerializeField]
    private ParticleSystem C_Bullet_Effect;

    [Header("If this is Golem")]
    [SerializeField]
    private ParticleSystem Golem_Effect;

    [Header("If this is Skeleton")]
    [SerializeField]
    private ParticleSystem Skeleton_Effect;

    [Header("If this is CommonEnemy")]
    [SerializeField]
    private ParticleSystem Common_Effect;

    [Header("Common Properties")]
    [SerializeField]
    private int damage;
    [SerializeField]
    private AudioSource monsterAudio;
    [SerializeField]
    private AudioClip monsterAttackSound;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            other.GetComponent<TankDemo.Tank_Health>().TakeDamage(damage);
            monsterAudio.PlayOneShot(monsterAttackSound);
            if (C_Bullet_Effect != null)
            {
                C_Bullet_Effect.Play();
            }
            if(Golem_Effect != null)
            {
                Golem_Effect.Play();
            }
            if(Skeleton_Effect!=null)
            {
                Skeleton_Effect.Play();
            }
            if(Common_Effect != null)
            {
                Common_Effect.Play();
            }
        }
    }
}
