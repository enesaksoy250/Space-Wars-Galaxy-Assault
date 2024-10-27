using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stopwatch : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    [SerializeField] float time;
    public bool isRunning=false;
    private float initialGameTime;
    private bool loadAd=false;

    public static Stopwatch instance;   

    private void Awake()
    {

        instance = this;
        initialGameTime = time;
        UpdateTimeText();       
    
    }

    void Update()
    {
     
        if (isRunning)
        {          
               
            time -= Time.deltaTime;


             if(!loadAd)
             {

                 loadAd = true;
                 AdManager.instance.LoadInterstitialAd();
             
             }

             if(time <= 0)
             {
               
                  isRunning = false;
                  time = 0;
                  GameManager gameManager = FindObjectOfType<GameManager>();
                  gameManager.isPlaying = false;
                  gameManager.EndGameControl();
                                
             } 
                                                             
            UpdateTimeText();
             
        }
    }

    public void StartTime()
    {
        isRunning = true;
    }

    public void StopTime()
    {

        isRunning = false;

    }
    public void Reset()
    {
        time = 0f;
        UpdateTimeText();
    }

    void UpdateTimeText()
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }

    public void UpdateTime(float time)
    {

        this.time += time;
        UpdateTimeText();
    }

    public float GetGameDuration()
    {

        return initialGameTime-time;

    }

}
