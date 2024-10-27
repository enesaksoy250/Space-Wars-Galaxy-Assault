using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSpaceStation : MonoBehaviour
{


    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletLifetime, firingRate, bulletSpeed, fireDistance;
    GameObject player;
    Coroutine firingCoroutine;

    void Start()
    {

        player = GameObject.FindWithTag("Player");

    }


    void Update()
    {

        FireToPlayer();

    }

    void FireToPlayer()
    {

        float playerHealth = PlayerHealth.instance.health;
        float distance = Mathf.Abs(Vector2.Distance(player.transform.position, transform.position));

        if (distance <= fireDistance && firingCoroutine == null && playerHealth > 0)
        {

            firingCoroutine = StartCoroutine(Fire());

        }

        else if ((distance > fireDistance || playerHealth <= 0) && firingCoroutine != null)
        {

            StopCoroutine(firingCoroutine);
            firingCoroutine = null;

        }


    }

    IEnumerator Fire()
    {

        while (true)
        {

            Vector3 direction = player.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            GameObject instance = Instantiate(bulletPrefab, transform.position, Quaternion.AngleAxis(angle + 90, Vector3.forward));
            Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 distance = (player.transform.position - transform.position).normalized;
                rb.AddForce(distance * bulletSpeed);
            }

            Destroy(instance, bulletLifetime);
            yield return new WaitForSeconds(firingRate);

        }

    }

}



