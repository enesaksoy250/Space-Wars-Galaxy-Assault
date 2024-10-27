using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StopWatchOnline : MonoBehaviourPunCallbacks
{

    public TextMeshProUGUI timerText;
    public const int countDownDuration = 180;
    private double gameStartTime;
    public bool isGameStarted = false;
    private bool loadAd = false;
    private double elapsedTime;

    PhotonView pv;

    public static StopWatchOnline instance;

    private void Awake()
    {
        instance = this;
        pv = GetComponent<PhotonView>(); 
    }

   
    void Update()
    {

        if (isGameStarted)
        {

            if(!loadAd)
            {

                loadAd=true;
                AdManager.instance.LoadInterstitialAd();

            }

            elapsedTime = PhotonNetwork.Time - gameStartTime;
            double remainingTime = countDownDuration - elapsedTime;

            if(remainingTime <= 0)
            {

                isGameStarted = false;
                remainingTime = 0;
                OnlineGameManager.instance.player1Start = false;
                OnlineGameManager.instance.player2Start = false;
                OnlineGameManager.instance.CheckForWinner();

            }

            UpdateUITimer((int)remainingTime);
        }

    }

    public void StartCountdown()
    {
        if (PhotonNetwork.IsMasterClient)
        {            
            gameStartTime = PhotonNetwork.Time;
            pv.RPC("StartGameForAll", RpcTarget.All, gameStartTime);
        }
    }

    [PunRPC]
    private void StartGameForAll(double startTime)
    {
        gameStartTime = startTime;
        isGameStarted = true;
    }


    private void UpdateUITimer(int remainingTime)
    {
        int minutes = remainingTime / 60;
        int seconds = remainingTime % 60;
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public int GetElapsedTime()
    {

        return (int)elapsedTime;

    }
}
