using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveOnline : MonoBehaviour
{

    [SerializeField] float moveSpeed, stoppingDistance, chasingDistance, smallestStoppingDistance, largestStoppingDistance;
    GameObject player;
    Rigidbody2D rb;
    EnemyFireOnline enemyFireOnline;
    PlayerHealthOnline playerHealthOnline;
    PhotonView pw;
    private float distanceToPlayer;
    private bool findPlayer=false;
    public int targetPlayer;


    void Awake()
    {
      
        rb = GetComponent<Rigidbody2D>();
        enemyFireOnline = gameObject.GetComponent<EnemyFireOnline>();
        pw = gameObject.GetComponent<PhotonView>();
    }


    void Start()
    {


        InvokeRepeating(nameof(PlayerListControl),0,.1f);
    

    }

  
    void Update()
    {

        if (findPlayer && pw.IsMine)
        {

            float health = playerHealthOnline.health;

            if (health > 0)
                distanceToPlayer = Mathf.Abs(Vector2.Distance(transform.position, player.transform.position));


            if (distanceToPlayer <= chasingDistance)
            {

                MoveToPlayer();

            }
               

            else
            {

                enemyFireOnline.isFiringToPlayer = false;
                rb.velocity = Vector2.zero;

            }
             

        }      

    }

    void MoveToPlayer()
    {

        SetRotationToPlayer();

        float distanceToPlayer = Mathf.Abs(Vector2.Distance(transform.position, player.transform.position));

        Vector2 center1 = player.transform.position;
        Vector2 center2 = transform.position;
        float angle = Mathf.Atan2(center2.y - center1.y, center2.x - center1.x) * Mathf.Rad2Deg;

        if (distanceToPlayer < chasingDistance && distanceToPlayer > largestStoppingDistance)
        {

            transform.rotation = Quaternion.Euler(0, 0, angle + 90);
            Vector2 distance = (player.transform.position - transform.position).normalized;
            rb.velocity = distance * moveSpeed;
            enemyFireOnline.isFiringToPlayer = false;

        }

        else if (distanceToPlayer <= smallestStoppingDistance)
        {

            //rb.velocity=player.GetComponent<Rigidbody2D>().velocity;            
            rb.velocity = Vector2.zero;
           enemyFireOnline.isFiringToPlayer = true;
        }


    }

    void SetRotationToPlayer()
    {

        Vector2 center1 = player.transform.position;
        Vector2 center2 = transform.position;
        float angle = Mathf.Atan2(center2.y - center1.y, center2.x - center1.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);

    }

    private void FindPlayers()
    {

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject p in players)
        {


            int actorNumber = p.GetComponent<PhotonView>().Owner.ActorNumber;
            

            if (actorNumber == targetPlayer)
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
