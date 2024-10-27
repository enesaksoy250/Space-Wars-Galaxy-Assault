using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyFireOnline : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject[] firePositions;
    [SerializeField] float bulletSpeed, bulletLifetime, firingRate;
    public int targetPlayer;
    public bool isFiringToPlayer;
    private bool findPlayer = false;
    private bool isStart=false;
    GameObject player;
    PlayerHealthOnline playerHealthOnline;
    Coroutine firingCoroutinePlayer;
    Rigidbody2D rb;
    PhotonView pw;

    private void Awake()
    {

        pw = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();

    }

    void Start()
    {


        InvokeRepeating(nameof(PlayerListControl), 0, .1f);
           

    }

   

    private void Update()
    {

       
        if (pw.IsMine && findPlayer)
        {

            FireToPlayer();

        }

    }

    void FireToPlayer()
    {
   
         GetStartValue();

        if ((playerHealthOnline.health > 0) && (isFiringToPlayer)  && firingCoroutinePlayer == null && isStart)
        {

            firingCoroutinePlayer = StartCoroutine(FireToPlayerCoroutine());
       
        }

        else if ((playerHealthOnline.health <= 0 || !isFiringToPlayer || !isStart) && firingCoroutinePlayer != null)
        {

            StopCoroutine(firingCoroutinePlayer);
            firingCoroutinePlayer = null;

        }
    
    }

    private void GetStartValue()
    {

        isStart = targetPlayer == 1 ? OnlineGameManager.instance.player1Start : OnlineGameManager.instance.player2Start;

    }

    IEnumerator FireToPlayerCoroutine()
    {
     
        while (true)
        {

            foreach (GameObject firePoint in firePositions)
            {

                GameObject instance = PhotonNetwork.Instantiate(bulletPrefab.name, firePoint.transform.position, transform.rotation);
                instance.GetComponent<BulletForOnline>().targetPlayer = targetPlayer;
                //Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();  

            }

            AudioManager.instance.PlayAudio("enemyFire");
            yield return new WaitForSeconds(firingRate);


        }
    }


    private float GetEnemyVelocity()
    {

        float x = Mathf.Clamp(Mathf.Abs(rb.velocity.x), 1, 2);
        float y = Mathf.Clamp(Mathf.Abs(rb.velocity.y), 1, 2);

        return Mathf.Max(x, y);

    }

   
    public void FindPlayers()
    {

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject p in players)
        {

            int actorNumber = p.GetComponent<PhotonView>().Owner.ActorNumber;

            if (targetPlayer == actorNumber)
            {

                player = p;
                playerHealthOnline = player.GetComponent<PlayerHealthOnline>();

            }

        }

        findPlayer = true;

    }

    private void PlayerListControl()
    {

        if(ServerManager.GetPlayerListLength() == 2)
        {

            FindPlayers();
            CancelInvoke(nameof(PlayerListControl));

        }

    }

}
