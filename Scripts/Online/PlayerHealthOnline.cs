using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerHealthOnline : MonoBehaviour
{

    private float initialHealth;
    public float health;
    private int playerIndex;
    PhotonView photonView;
    Animator animator;
    PlayerMoveOnline playerMoveOnline;
    StartPoints startPoints;
    PhotonView pw;

    private void Awake()
    {
       
        pw = GetComponent<PhotonView>();
        playerMoveOnline = GetComponent<PlayerMoveOnline>();
        animator = GetComponent<Animator>();
        photonView = GetComponent<PhotonView>();
        startPoints = FindObjectOfType<StartPoints>();
        initialHealth = health;
     
    }

    private void Start()
    {
        
        playerIndex = GetPlayerIndex();
  
    }

    public int GetPlayerIndex()
    {
      
        Player player = photonView.Owner;
        Player[] playerList = PhotonNetwork.PlayerList;

        for (int i = 0; i < playerList.Length; i++)
        {
            if (playerList[i] == player)
            {
                return i+1;
            }
        }

        return -1;
    }

    [PunRPC]
    public void TakeDamage(float damage,int targetPlayer)
    {
           
        if(health > 0)
        {

            health -= damage;
     

            if (health <= 0)
            {
                health = 0;
                Die();
            }

            else if(health > 0 && health < 1)
            {

                health = 1;

            }

            int newHealth = Mathf.RoundToInt(health / initialHealth*100);
            GameObject.FindWithTag("ServerManager").GetComponent<ServerManager>().UpdateHealthText(newHealth, targetPlayer);

        }          
        
    }

    [PunRPC]
    public void FullHealth(int targetPlayer)
    {

        health = initialHealth;
        int newHealth = (int)((health / initialHealth) * 100);
        GameObject.FindWithTag("ServerManager").GetComponent<ServerManager>().UpdateHealthText(newHealth, targetPlayer);

    }


    private void Die()
    {

        if (pw.IsMine)
        {

            if (playerIndex == 1 || playerIndex == 2)
            {
                OnlineGameManager.instance.GetType()
                    .GetField($"player{playerIndex}Start")
                    .SetValue(OnlineGameManager.instance, false);
            }

            AudioManager.instance.PlayAudio("gameOver");
            animator.Play("Explosion");
            Invoke(nameof(RestartGame), 2f);


            foreach (Transform child in transform)
            {

                if (child.name == "EngineEffect")
                {

                    child.gameObject.SetActive(false);

                }


            }

        }

      

    }

    private void RestartGame()
    {
     
       OnlineGameManager.instance.RestartGame(gameObject);
                
    }

    public void CloseEffect()
    {

        GetComponent<SpriteRenderer>().enabled = false;

    }


   
}
