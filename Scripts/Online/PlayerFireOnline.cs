using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireOnline : MonoBehaviour
{


    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject[] firePositions;
    [SerializeField] float projectileSpeed, projectileLifetime, firingRate;
    public float bulletPower;
    public bool firing;
    private bool start=false;
    Coroutine firingCoroutine;
    PlayerHealthOnline playerHealthOnline;
    PhotonView pw;
    GameObject instance;
    private int targetPlayer;
   
    private void Awake()
    {

        pw = GetComponent<PhotonView>(); 
        playerHealthOnline = GetComponent<PlayerHealthOnline>();

    }


    private void Start()
    {

        if (pw.IsMine)
        {

            if (PhotonNetwork.IsMasterClient)
            {
                
                targetPlayer = 2;

            }

            else
            {

                targetPlayer = 1;

            }

        }

    }
    void Update()
    {

          GetStartValue();

        if (pw.IsMine && start) {


            Fire();

        }

        else
        {

            if(firingCoroutine != null)
            {

                StopCoroutine(firingCoroutine);
                firingCoroutine = null;

            }

        }

    }

    private void GetStartValue()
    {

        int playerIndex = playerHealthOnline.GetPlayerIndex();

        start = playerIndex == 1 ? OnlineGameManager.instance.player1Start : OnlineGameManager.instance.player2Start;

    }

    void Fire()
    {
        if (firing && firingCoroutine == null && playerHealthOnline.health > 0)
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }

        else if (firingCoroutine != null && (!firing || playerHealthOnline.health < 0))
        {

            StopCoroutine(firingCoroutine);
            firingCoroutine = null;

        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {


            foreach (GameObject firePoint in firePositions)
            {


                instance = PhotonNetwork.Instantiate(projectilePrefab.name,firePoint.transform.position,transform.rotation);
                instance.GetComponent<BulletForOnline>().targetPlayer = targetPlayer;
                instance.GetComponent<BulletForOnline>().damage = bulletPower;
                instance.GetComponent<BulletForOnline>().projectileSpeed = projectileSpeed;
          

            }

            AudioManager.instance.PlayAudio("playerFire");
            yield return new WaitForSeconds(firingRate);
        }

    }

  

    void BulletPowerControl()
    {

        if (!PlayerPrefs.HasKey(gameObject.name + "bulletpower"))
        {

            PlayerPrefs.SetFloat(gameObject.name + "bulletpower", bulletPower);

        }

        else
        {

            bulletPower = PlayerPrefs.GetFloat(gameObject.name + "bulletpower");

        }


    }
}
