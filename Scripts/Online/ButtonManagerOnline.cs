using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManagerOnline : MonoBehaviour
{

    PlayerMoveOnline playerMoveOnline;
    PlayerFireOnline playerFireOnline;
    Player targetPlayer;

    public static ButtonManagerOnline instance;

    private void Awake()
    {
        
        instance = this;

    }

    public void FindObject()
    {
      
       
            Player[] players = PhotonNetwork.PlayerList;
            

            if(PhotonNetwork.IsMasterClient)
                targetPlayer = players[0];

            else
                targetPlayer = players[1];


            int targetPlayerActorNumber = targetPlayer.ActorNumber;

            PhotonView[] photonViews = FindObjectsOfType<PhotonView>();


            foreach (PhotonView pv in photonViews)
            {
                if (pv.Owner != null && pv.Owner.ActorNumber == targetPlayerActorNumber)
                {
                    playerFireOnline = pv.GetComponent<PlayerFireOnline>();
                    playerMoveOnline = pv.GetComponent<PlayerMoveOnline>();

                    if (playerFireOnline != null)
                    {
                        Debug.Log("Hedef oyuncunun PlayerFireOnline bileþenine eriþildi.");
                    }
                    else
                    {
                        Debug.LogWarning("Hedef oyuncunun PlayerFireOnline bileþenine eriþilemedi.");
                    }
                    break;
                }
            }
        

    
    }


    public void FireForButton()
    {
       
        playerFireOnline.firing = true;    
             
    }

    public void DontFireForButton()
    {
       
        playerFireOnline.firing = false;      

    }

    public void StopForButton()
    {


        playerMoveOnline.temporarySpeed = playerMoveOnline.speed;
        playerMoveOnline.speed = 0;
        playerMoveOnline.rb.drag = playerMoveOnline.stoppingSpeed;


    }

    public void DontStopForButton()
    {

        playerMoveOnline.speed = playerMoveOnline.temporarySpeed;
        playerMoveOnline.rb.drag = .1f;

    }
}
