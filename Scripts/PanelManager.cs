using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelManager : MonoBehaviour
{

    [SerializeField] GameObject[] mainPanels;

    public static PanelManager instance;

    private void Awake()
    {
        
       instance = this;

    }

    private void Start()
    {


        if (!PlayerPrefs.HasKey("FirstGame")&&SceneManager.GetActiveScene().buildIndex==1)
        {

            Invoke(nameof(OpenInfoPanel), 1.25f);

        }

    }

    private void OpenInfoPanel()
    {

        Time.timeScale = 0;
        LoadPanel("InfoPanel");
        PlayerPrefs.SetInt("FirstGame", 1);

    }

    public void LoadPanel(string panelName, bool setActive)
    {

        foreach (GameObject gameObject in mainPanels)
        {

            if (gameObject.name == panelName)
            {

                gameObject.SetActive(setActive);

                if (gameObject.name == "MainPausePanel" && setActive == true || gameObject.name == "MainSettingsPanel" && setActive == true)
                {

                    Time.timeScale = 0;
                    Stopwatch.instance.StopTime();

                }

            }

        }

    }

    public void CloseAllPanel()
    {

        foreach (GameObject gameObject in mainPanels)
        {

        
            gameObject.SetActive(false);
            Time.timeScale = 1;
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            
            if(buildIndex != 0)
            {

                Stopwatch.instance.StartTime();

            }
           

        }

    }

    public void LoadPanel(string panelName)
    {

        foreach (GameObject gameObject in mainPanels)
        {

            if (gameObject.name == panelName)
            {           

                gameObject.SetActive(true);

                if (gameObject.name == "MainPausePanel" || gameObject.name == "MainSettingsPanel"||gameObject.name=="InfoPanel")
                {

                    Time.timeScale = 0;
                    Stopwatch.instance.StopTime();

                }

            }

        }


    }
   
    public void ClosePanel(string panelName)
    {


        foreach (GameObject gameObject in mainPanels)
        {

            if (gameObject.name == panelName)
            {

                gameObject.SetActive(false);

                if (gameObject.name == "MainPausePanel"||gameObject.name=="InfoPanel")
                {

                    Time.timeScale = 1;
                    Stopwatch.instance.StartTime();

                }

            }

        }

    }

   
   
}
