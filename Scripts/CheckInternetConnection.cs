using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckInternetConnection : MonoBehaviour
{

    private int sceneIndex;

    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(CheckInternet());
    }


    IEnumerator CheckInternet()
    {
        while (true) 
        {
           
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
               
                          
                if(sceneIndex == 0)
                {

                    MainMenuPanelManager.instance.LoadPanel("InternetErrorPanel");

                }

                else
                {

                    GameManager.instance.isPlaying = false;
                    Stopwatch.instance.isRunning = false;
                    PanelManager.instance.LoadPanel("InternetErrorPanel");

                }

            }
          
            else
            {
               
              
              
                if (sceneIndex == 0)
                {

                    MainMenuPanelManager.instance.ClosePanel("InternetErrorPanel");

                }

                else
                {

                    GameManager.instance.isPlaying = true;
                    Stopwatch.instance.isRunning=true;
                    PanelManager.instance.ClosePanel("InternetErrorPanel");

                }
            }
        
            yield return new WaitForSeconds(1f);
      
        }
    }
}
