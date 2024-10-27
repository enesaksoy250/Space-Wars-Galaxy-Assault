using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnlineGameManager : MonoBehaviour
{


    [Header("Player Stations")]
    [SerializeField] GameObject[] player1Stations;
    [SerializeField] GameObject[] player2Stations;
    [Space(10)]
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject losePanel;
    [SerializeField] TextMeshProUGUI score1Text,score2Text;
    [SerializeField] float rewardPoints;
    StartPoints startPoints;
    Stopwatch stopwatch;
    PhotonView pw;
    private float score1=0,score2=0;
    public bool player1Start=false;
    public bool player2Start=false;


    public static OnlineGameManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
     
        pw = GetComponent<PhotonView>();
        startPoints=FindObjectOfType<StartPoints>();
        stopwatch = FindObjectOfType<Stopwatch>();   
        
    }


    public void UpdateScoreText(int targetPlayer,float score)
    {

        pw.RPC("RPC_UpdateScoreText", RpcTarget.All,targetPlayer,score);

    }

    
    [PunRPC]
    private void RPC_UpdateScoreText(int targetPlayer, float score)
    {

        switch (targetPlayer)
        {

            case 1:
                score1 += score;
                score1Text.text = score1.ToString();
                break;

            case 2:
                score2 += score;
                score2Text.text = score2.ToString();
                break;


        }

    }

    public void CheckForWinner()
    {

        if (score1 >= score2)   //Buraya beraberlik durumu eklenecek.
        {

            Invoke(nameof(ShowAd), 1);
            Invoke(nameof(CallSegpFunction1),2);
     
        }
       
        else if (score2 > score1)
        {

            Invoke(nameof(ShowAd), 1);
            Invoke(nameof(CallSegpFunction2), 2);
     
        }

       
    }

    private void ShowAd()
    {

        AdManager.instance.ShowInterstitialAd();

    }
  
    private void ShowEndGamePanels(int winnerPlayer)
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.IsLocal)
            {
                if (winnerPlayer == PhotonNetwork.LocalPlayer.ActorNumber)
                {
                    winPanel.SetActive(true);
                    UpdatePlayerPointsAndUI(winPanel,rewardPoints);
                    DataBaseManager.instance.UpdateFirebaseInfo("onlineWins", 1);
                  
                   
                }
              
                else
                {
                    losePanel.SetActive(true);
                    UpdateGameTimeAndPoints(losePanel);

                }
            }
        }
     
   
    }

  
    private void UpdatePlayerPointsAndUI(GameObject panel, float points)
    {
        
        GameObject localPlayerObject = GetLocalPlayerObject();
       
        if (localPlayerObject != null)
        {
            var playerPoints = localPlayerObject.GetComponent<PlayerPoints>();
            playerPoints.ChangeScore(points);
            UpdateGameTimeAndPoints(panel);
        }
    }

    private void UpdateGameTimeAndPoints(GameObject panel)
    {

        if(panel == winPanel)
        {

            Transform rewardPointsText = FindDeepChild(panel.transform, "PointsGainedText");
            rewardPointsText.gameObject.GetComponent<TextMeshProUGUI>().text = rewardPoints.ToString();

        }

      
        Transform totalPoints = FindDeepChild(panel.transform, "TotalPointsText");
        totalPoints.gameObject.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetFloat("Coin").ToString();

        Transform gameTime = FindDeepChild(panel.transform, "GameTimeText");
        gameTime.gameObject.GetComponent<TextMeshProUGUI>().text = StopWatchOnline.instance.GetElapsedTime().ToString();
   
    }

    public void RestartGame(GameObject player)
    {

        int actorNumber = player.GetComponent<PhotonView>().Owner.ActorNumber;

        //DestroyEnemy(actorNumber);
        pw.RPC("DestroyEnemy", RpcTarget.All, actorNumber);
     
        player.transform.position = startPoints.SetStartPoint(player);
        player.GetComponent<SpriteRenderer>().enabled = true;
        player.GetComponent<Animator>().Play("Start");
        player.GetPhotonView().RPC("FullHealth", RpcTarget.All, actorNumber);
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        player.transform.rotation = Quaternion.identity;
      
        int playerIndex = player.GetComponent<PlayerHealthOnline>().GetPlayerIndex();

        if (playerIndex == 1 || playerIndex == 2)
        {
                 instance.GetType()
                .GetField($"player{playerIndex}Start")
                .SetValue(instance, true);
        }
  
        
    }

    [PunRPC]
    private void DestroyEnemy(int actorNumber)
    {

    
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");


            foreach (GameObject enemy in enemies)
            {

                PhotonView enemyPhotonView = enemy.GetPhotonView();

                if (enemyPhotonView != null && enemyPhotonView.IsMine && enemy.GetComponent<EnemyFireOnline>().targetPlayer == actorNumber)
                {

                    PhotonNetwork.Destroy(enemy);

                }

            }
        
    }
  
    private GameObject GetLocalPlayerObject()
    {
      
        foreach (var obj in FindObjectsOfType<PhotonView>())
        {
            if (obj.Owner != null && obj.Owner.IsLocal && obj.gameObject.CompareTag("Player"))
            {
                return obj.gameObject;
            }
            
        }
    
        return null;
  
    }

    public static Transform FindDeepChild(Transform parent, string name)
    {
        
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;
          
            var result = FindDeepChild(child, name);
            
            if (result != null)
                return result;
        }
        
        return null;
    
    }

    public void LoadWinPanel(int winnerPlayer)
    {

        ShowEndGamePanels(winnerPlayer);

    }

    private void CallSegpFunction1()
    {

        ShowEndGamePanels(1);

    }

    private void CallSegpFunction2()
    {

        ShowEndGamePanels(2);

    }


    [PunRPC]
    private void RPC_SetUsernameText(string username1,string username2)
    {

        GameObject.FindWithTag("NameText0").GetComponent<TextMeshProUGUI>().text = username1;
        GameObject.FindWithTag("NameText1").GetComponent<TextMeshProUGUI>().text = username2;

    }

    public void SetUsernameText(string username1,string username2)
    {

        pw.RPC("RPC_SetUsernameText", RpcTarget.All,username1,username2);

    }
}
