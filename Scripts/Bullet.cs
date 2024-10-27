using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{

    [SerializeField] float bulletPower;
    [SerializeField] GameObject explosionEffect;
    SpaceCraft spaceCraft;

    private void Awake()
    {

        spaceCraft=FindObjectOfType<SpaceCraft>();
        
    }

    private void Start()
    {

        if (gameObject.CompareTag("PlayerBullet"))
        {

            bulletPower = PlayerFire.instance.bulletPower;

        }
            
        Destroy(gameObject,5f);

    }

   

    private void OnTriggerEnter2D(Collider2D collision)
    {


        switch (collision.tag)
        {

            case "Player":

                if (!gameObject.CompareTag("PlayerBullet"))
                {

                    Instantiate(explosionEffect, transform.position, Quaternion.identity);
                    PlayerHealth.instance.ChangeHealth(bulletPower);
                    Destroy(gameObject);

                }

                break;

            case "Shield":

                if (!gameObject.CompareTag("PlayerBullet"))
                {

                    Instantiate(explosionEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);

                }
                
                break;

            case "Enemy":

                if (!gameObject.CompareTag("EnemyBullet"))
                {

                    collision.GetComponent<EnemyHealth>().ChangeEnemyHealth(bulletPower);
                    Instantiate(explosionEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);

                }

                break;

            case "Asteroid":

                if (gameObject.CompareTag("PlayerBullet"))
                {

                    collision.GetComponent<Asteroid>().ChangeAsteroidHealth(bulletPower);
                    Instantiate(explosionEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);

                }
                
                break;

            case "SpaceStation":

                if (gameObject.CompareTag("PlayerBullet"))
                {

                    collision.GetComponent<SpaceStation>().ChangeStationHealth(bulletPower);
                    Instantiate(explosionEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);

                }
                
                break;

            case "SpaceCraft":

                if (gameObject.CompareTag("PlayerBullet"))
                {

                    spaceCraft.ChangeSpaceCraftHealth(bulletPower);
                    Instantiate(explosionEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);

                }
                
                break;

            case "Bomb":

                if (gameObject.CompareTag("PlayerBullet"))
                {

                    collision.GetComponent<Bomb>().ChangeBombHealth(bulletPower);
                    Instantiate(explosionEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);

                }
                
                break;

            case "BombSpaceship":

                if (gameObject.CompareTag("PlayerBullet"))
                {

                    collision.GetComponent<BombSpaceship>().ChangeHealth(bulletPower);
                    Instantiate(explosionEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);

                }

                break;

        }


    }


}
