using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject[] spaceStations;
    [SerializeField] Sprite[] starImages;
    [SerializeField] TextMeshProUGUI destroyedStationText, totalStationText;  
    GameObject[] players;
    private int numberOfDestroyedStation=0;
    private int numberOfSpaceStations;
    private bool isLoop=true;
    public bool isPlaying=false;
    private int gameDuration,starNumber,sceneIndex;
   
    public static GameManager instance;

    private void Awake()
    {
        
        instance = this;
        totalStationText.text = spaceStations.Length.ToString();
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SelectSpaceship();
       
    }

    
    void Start()
    {

         Invoke(nameof(CloseLoadingPanel), 1);     
         numberOfSpaceStations=spaceStations.Length;

    }

    private void Update()
    {
        
        if(numberOfDestroyedStation == spaceStations.Length && isLoop)
        {

            isLoop=false;
            WinGame();

        }

    }


    public void EndGameControl()
    {

        if(numberOfDestroyedStation != 0)
        {

            WinGame();

        }

        else
        {

            LoadGameOverPanel();

        }
       

    }


    private void SelectSpaceship()
    {

        GameObject playerObject = GameObject.Find("Players");
        int childCount = playerObject.transform.childCount;
        players = new GameObject[childCount];

        for(int i = 0; i<childCount; i++)
        {

            players[i] = playerObject.transform.GetChild(i).gameObject;

        }

        int index = PlayerPrefs.GetInt("SelectedSpaceship");
        int indexNumber = index == 0 ? 0 : index - 1;     
        players[indexNumber].SetActive(true);


    }

    private void LoadWinPanel()
    {

        PanelManager.instance.LoadPanel("MainWinPanel", true);
        SetStar();
        int index = SceneManager.GetActiveScene().buildIndex;

        if(PlayerPrefs.GetInt("Level"+index+"Star") < starNumber)
           PlayerPrefs.SetInt("Level"+index+"Star",starNumber);

        GameObject.Find("GameTimeText").GetComponent<TextMeshProUGUI>().text =gameDuration.ToString();
        GameObject.Find("PointsGainedText").GetComponent<TextMeshProUGUI>().text =PlayerPoints.instance.GetPlayerScore().ToString();
        GameObject.Find("TotalPointsText").GetComponent<TextMeshProUGUI>().text =PlayerPrefs.GetFloat("Coin").ToString();
        

    }

    public void LoadGameOverPanel()
    {

       
        Stopwatch.instance.isRunning = false;
        gameDuration = (int)Stopwatch.instance.GetGameDuration();
        PanelManager.instance.LoadPanel("MainGameOverPanel", true);
        GameObject.Find("GameTimeText").GetComponent<TextMeshProUGUI>().text = gameDuration.ToString();
        GameObject.Find("PointsGainedText").GetComponent<TextMeshProUGUI>().text = PlayerPoints.instance.GetPlayerScore().ToString();
        GameObject.Find("TotalPointsText").GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetFloat("Coin").ToString();
        DataBaseManager.instance.UpdateFirebaseInfo("gameTime", gameDuration);

    }

    public void IncreaseNumber()
    {

        numberOfDestroyedStation++;
        destroyedStationText.text= numberOfDestroyedStation.ToString();

    }

    private void SetStar()
    {

        GameObject starImage = GameObject.Find("WinStarImage");
       

        if(sceneIndex == 1)
        {

            starImage.GetComponent<Image>().sprite = starImages[3];
            starNumber = 3;

        }

        else if (sceneIndex == 2)
        {
            
            if(numberOfDestroyedStation == 1)
            {

                starImage.GetComponent<Image>().sprite = starImages[1];
                starNumber = 1;

            }

            else
            {

                starImage.GetComponent<Image>().sprite = starImages[3];
                starNumber = 3;

            }

        }

        else
        {

            starImage.GetComponent<Image>().sprite = starImages[numberOfDestroyedStation];
            starNumber = numberOfDestroyedStation;


        }




    }

    private void WinGame()
    {
     
        Stopwatch.instance.isRunning = false;
        gameDuration = (int)Stopwatch.instance.GetGameDuration();
        DataBaseManager.instance.UpdateFirebaseInfo("gameTime", gameDuration);
        isPlaying = false;
        isLoop = false;
        Invoke(nameof(ShowAd), 1);
        Invoke(nameof(LoadWinPanel), 2);

    }

    private void ShowAd()
    {

        AdManager.instance.ShowInterstitialAd();

    }

    private void CloseLoadingPanel()
    {

        GameObject.FindWithTag("LoadingPanel").gameObject.SetActive(false);
        Stopwatch.instance.isRunning = true;
        isPlaying = true;

    }
    
    public void InfoButton()
    {

        PanelManager.instance.LoadPanel("InfoPanel");
        Time.timeScale = 0;
        

    }

}
