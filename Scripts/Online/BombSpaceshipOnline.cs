using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombSpaceshipOnline : MonoBehaviour
{

    [SerializeField] float health, power, chasingDistance, moveSpeed;
    [SerializeField] int targetPlayer;
    GameObject player;
    Animator animator;
    Rigidbody2D rb;
    private bool isLoop=true, explosion=true;
    private bool findPlayer = false;
    private static float saveHealth;
    private bool isStart=false;
    PhotonView pw;


    private void Awake()
    {

        saveHealth = health;
        pw = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    void Start()
    {
        InvokeRepeating(nameof(PlayerListControl), 0, .1f);
    }


    void Update()
    {


         GetStartValue();

        if (findPlayer && pw.IsMine && isStart)
        {
          
            float distanceToPlayer = Mathf.Abs(Vector2.Distance(transform.position, player.transform.position));

            if (distanceToPlayer <= chasingDistance)
            {

                MoveToPlayer();

            }

            else
            {

                rb.velocity = Vector2.zero;

            }

        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player") && isLoop && explosion)
        {

            Explosion();

        }

    }

    private void GetStartValue()
    {

         isStart = targetPlayer == 1 ? OnlineGameManager.instance.player1Start : OnlineGameManager.instance.player2Start;

    }
    void MoveToPlayer()
    {

        SetRotationToPlayer();

        float distanceToPlayer = Mathf.Abs(Vector2.Distance(transform.position, player.transform.position));

        Vector2 center1 = player.transform.position;
        Vector2 center2 = transform.position;
        float angle = Mathf.Atan2(center2.y - center1.y, center2.x - center1.x) * Mathf.Rad2Deg;

        if (distanceToPlayer < chasingDistance)
        {

            transform.rotation = Quaternion.Euler(0, 0, angle + 90);
            Vector2 distance = (player.transform.position - transform.position).normalized;
            rb.velocity = distance * moveSpeed;


        }


    }

    void SetRotationToPlayer()
    {

        Vector2 center1 = player.transform.position;
        Vector2 center2 = transform.position;
        float angle = Mathf.Atan2(center2.y - center1.y, center2.x - center1.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);

    }

    public void ChangeHealth(float damage)
    {


        health -= damage;

        if (health <= 0 && explosion)
        {

            explosion = false;
            AudioManager.instance.PlayAudio("explosion");
            animator.Play("ExplosionForStation");
            OnlineGameManager.instance.UpdateScoreText(targetPlayer, saveHealth);
            Animator[] animators = GetComponentsInChildren<Animator>();

            if (animators.Length > 0)
            {

                foreach (Animator anim in animators)
                {

                    anim.StopPlayback();

                }

            }

            
              Invoke(nameof(DestroyBomb),.75f);

        }


    }

    private void Explosion()
    {

        if (pw.IsMine)
        {

            isLoop = false;
            AudioManager.instance.PlayAudio("explosion");
            rb.velocity = Vector3.zero;
            animator.Play("ExplosionForStation");

            Vector2 distance = (player.transform.position - transform.position).normalized;
            player.GetComponent<Rigidbody2D>().AddForce(distance * 250);

            PhotonView targetPhotonView = player.GetPhotonView();
            targetPhotonView.RPC("TakeDamage", RpcTarget.All, power, targetPlayer);
                 
            Invoke(nameof(DestroyBomb), .75f);


        }
        

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

            }

        }

        findPlayer = true;

    }

    private void PlayerListControl()
    {

        if (ServerManager.GetPlayerListLength() == 2)
        {

            Invoke(nameof(FindPlayers),3);
            CancelInvoke(nameof(PlayerListControl));

        }

    }

    [PunRPC]
    public void RequestDestroy()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void DestroyBomb()
    {
        if (!PhotonNetwork.IsMasterClient)
        {

           pw.RPC("RequestDestroy", RpcTarget.MasterClient);
        }

        else
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

}
