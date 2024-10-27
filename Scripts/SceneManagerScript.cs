using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{

   
    public void Play()
    {

        AudioManager.instance.PlayAudio("gameStart");

        for (int i = 1; i <= 12; i++)
        {
            if (!PlayerPrefs.HasKey("Level" + i + "Star"))
            {
                SceneManager.LoadScene(i);
                return;
            }
        }

        SceneManager.LoadScene(1);

    }
    
     



    public void LoadMainMenu()
    {   
        
        if(SceneManager.GetActiveScene().buildIndex == 13)
        {

            ServerManager.instance.LeaveRoom();

        }

        else
        {

            SceneManager.LoadScene(0);
            Destroy(ServerManager.instance.gameObject,.1f);

        }

        Time.timeScale = 1.0f;

    }

    public void RestartCurrentLevel()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
      
    }

    public void LoadNextLevel()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        Time .timeScale = 1.0f;
    
    }

    public void LoadLevel(int levelIndex)
    {

        AudioManager.instance.PlayAudio("gameStart");
        SceneManager.LoadScene(levelIndex);

    }

   
}
