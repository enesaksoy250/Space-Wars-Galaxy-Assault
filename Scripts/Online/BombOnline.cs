using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombOnline : MonoBehaviour
{
    [SerializeField] float bombPower, bombHealth;
    [SerializeField] int targetPlayer;
    private Animator animator;
    private GameObject player;
    private bool isLoop = true;
    private float initialBombHealth;
    PhotonView photonView;


    private void Awake()
    {

        initialBombHealth = bombHealth;
        photonView = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();

    }

    private void Start()
    {
  
        InvokeRepeating(nameof(PlayerListControl),0,.1f);
    
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {

            DamageToPlayer();
            animator.Play("ExplosionForStation");

            if (photonView != null && photonView.IsMine)
                Invoke(nameof(DestroyBomb),.75f);

        }
  
    }

    public void ChangeBombHealth(float damage)
    {

        bombHealth -= damage;

        if (bombHealth <= 0 && isLoop)
        {

            isLoop = false;
            AudioManager.instance.PlayAudio("explosion");
            OnlineGameManager.instance.UpdateScoreText(targetPlayer,initialBombHealth);
            float distance = Mathf.Abs(Vector2.Distance(player.transform.position, transform.position));


            if (distance <= 6)
            {

                DamageToPlayer();

            }

            animator.Play("ExplosionForStation");
            Invoke(nameof(DestroyBomb), .75f);

        }

    }

    void DamageToPlayer()
    {


        if (photonView.IsMine)
        {

            Vector2 distance = (player.transform.position - transform.position).normalized;
            player.GetComponent<Rigidbody2D>().AddForce(distance * 250);

            PhotonView targetPhotonView = player.GetPhotonView();
            targetPhotonView.RPC("TakeDamage", RpcTarget.All, bombPower, targetPlayer);
            //PhotonNetwork.Instantiate("Explosion", transform.position, Quaternion.identity);
            //PhotonNetwork.Destroy(gameObject);


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
            
            photonView.RPC("RequestDestroy", RpcTarget.MasterClient);
        }

        else
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

}
