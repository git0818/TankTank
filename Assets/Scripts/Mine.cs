using System.Collections;
using System.Collections.Generic;
using TankDemo;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public GameObject bombeffect;
    public float ExplosionRadius = 10f;
    Tank_Input tankinput;

    private void Start()
    {
        tankinput = FindObjectOfType<Tank_Input>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius, LayerMask.GetMask("Enemy"));

            foreach (Collider collider in colliders)
            {
                Rigidbody targetrb = collider.GetComponent<Rigidbody>();
                if (!targetrb)
                    continue;

                Enemy.Enemy_HealthNMove targetHealth = targetrb.GetComponent<Enemy.Enemy_HealthNMove>();
                if (!targetHealth)
                    continue;

                float damage = 100f;
                targetHealth.Damage(damage);
            }
            GameManager.instance.audiosource.PlayOneShot(GameManager.instance.MineBoomSound);
            tankinput.Shake();
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z);
            var hiteff = Instantiate(bombeffect, pos, transform.rotation);
            hiteff.transform.position = transform.position;
            //hiteff.transform.rotation = transform.rotation;
            Destroy(gameObject);
        }

    }
}
