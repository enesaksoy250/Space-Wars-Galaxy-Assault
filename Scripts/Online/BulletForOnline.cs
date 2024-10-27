using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class BulletForOnline : MonoBehaviour
{

    public float damage;
    public int targetPlayer;
    PhotonView pw;
    Rigidbody2D rb;
    public float projectileSpeed;

    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        pw = GetComponent<PhotonView>();
   
    }

    private void Start()
    {
        

        if (rb != null)
        {
            rb.velocity = transform.up * projectileSpeed;
        }

        
        Invoke(nameof(DestroyBullet), 1f);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(pw != null && pw.IsMine)
        {

            switch (collision.gameObject.tag)
            {

                case "Player":

                    if (!gameObject.CompareTag("PlayerBullet"))
                    {

                        PhotonView targetPhotonView = collision.gameObject.GetComponent<PhotonView>();

                        if (targetPhotonView != null)
                        {

                            int actorNumber = targetPhotonView.Owner.ActorNumber;

                            if (actorNumber == targetPlayer)
                            {

                                targetPhotonView.RPC("TakeDamage", RpcTarget.All, damage, targetPlayer);
                                PhotonNetwork.Instantiate("Explosion", transform.position, Quaternion.identity);
                                DestroyBullet();

                            }


                        }

                    }

                    break;

                case "Enemy":

                    if (!gameObject.CompareTag("EnemyBullet"))
                    {

                        collision.GetComponent<EnemyHealthOnline>().ChangeEnemyHealth(damage);
                        PhotonNetwork.Instantiate("Explosion", transform.position, Quaternion.identity);
                        DestroyBullet();

                    }

                    break;

                case "SpaceStation":

                    if (gameObject.CompareTag("PlayerBullet"))
                    {

                        collision.GetComponent<SpaceStationOnline>().ChangeStationHealth(damage);
                        PhotonNetwork.Instantiate("Explosion", transform.position, Quaternion.identity);
                        DestroyBullet();

                    }

                    break;

                case "Asteroid":

                    if (gameObject.CompareTag("PlayerBullet"))
                    {

                        collision.GetComponent<Asteroid>().ChangeAsteroidHealth(damage);
                        PhotonNetwork.Instantiate("Explosion", transform.position, Quaternion.identity);
                        DestroyBullet();

                    }

                    break;

                case "Bomb":

                    if (gameObject.CompareTag("PlayerBullet"))
                    {

                        collision.GetComponent<BombOnline>().ChangeBombHealth(damage);
                        PhotonNetwork.Instantiate("Explosion", transform.position, Quaternion.identity);
                        DestroyBullet();

                    }

                    break;

                case "BombSpaceship":

                    if (gameObject.CompareTag("PlayerBullet"))
                    {

                        collision.GetComponent<BombSpaceshipOnline>().ChangeHealth(damage);
                        PhotonNetwork.Instantiate("Explosion", transform.position, Quaternion.identity);
                        DestroyBullet();

                    }

                    break;

                case "SpaceCraft":

                    PhotonNetwork.Instantiate("Explosion", transform.position, Quaternion.identity);
                    DestroyBullet();
                    break;

            }

        }

    }

    private void DestroyBullet()
    {

        if(gameObject != null && pw != null && pw.IsMine)
          PhotonNetwork.Destroy(gameObject);

    }

}