using Photon.Pun.UtilityScripts;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;

public class CountdownScript : MonoBehaviour
{

    private const int countdownTime=3;
    private TextMeshProUGUI countdownText;
    PhotonView pw;

    private void Awake()
    {
    
        pw = GetComponent<PhotonView>();    

    }

    void Start()
    {
      
        GameObject parent = GameObject.FindWithTag("Countdown");
        Transform child = parent.transform.Find("Countdown");
        countdownText = child.gameObject.GetComponent<TextMeshProUGUI>();
  
    }

    public void StartCountdown()
    {

        if(PhotonNetwork.IsMasterClient)
           pw.RPC("StartCountdownRPC", RpcTarget.All);

    }

    [PunRPC]
    void StartCountdownRPC()
    {
        StartCoroutine(CountdownCoroutine());
    }

    IEnumerator CountdownCoroutine()
    {

        int currentTime = countdownTime;
        yield return new WaitForSeconds(6);
        countdownText.gameObject.SetActive(true);

        while (currentTime > 0)
        {
            countdownText.text = currentTime.ToString();
            yield return new WaitForSeconds(1);
            currentTime--;
        }

        countdownText.text = "Start!";
        yield return new WaitForSeconds(1);
        countdownText.gameObject.SetActive(false);
        OnlineGameManager.instance.player1Start = true;
        OnlineGameManager.instance.player2Start = true;
        StopWatchOnline.instance.StartCountdown();


    }
}
