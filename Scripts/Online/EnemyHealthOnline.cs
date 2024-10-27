using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealthOnline : MonoBehaviour
{

    [SerializeField] float health;
    Animator animator;
    EnemyFireOnline enemyFireOnline;
    private float initialHealth;
    PhotonView pw;

    private void Awake()
    {

        pw = GetComponent<PhotonView>();
        initialHealth = health;
        animator = GetComponent<Animator>();
        enemyFireOnline = gameObject.GetComponent<EnemyFireOnline>();

    }

   

    public void ChangeEnemyHealth(float damage)
    {

        health -= damage;

        if (health <= 0)
        {

            EnemyDie();

        }

    }

    private void EnemyDie() {


        AudioManager.instance.PlayAudio("explosion");      
        OnlineGameManager.instance.UpdateScoreText(enemyFireOnline.targetPlayer, initialHealth);
        GetComponent<PolygonCollider2D>().enabled = false;
        animator.Play("BigExplosion");
        Invoke(nameof(DestroyEnemy), .75f);    

    }

   
    [PunRPC]
    public void RequestDestroy()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void DestroyEnemy()
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
