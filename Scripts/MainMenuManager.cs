using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI[] SSAllCoinText;
    [SerializeField] Sprite[] upgradeSprites;
    [SerializeField] GameObject[] upgradePanel,playerPanels, selectButtons, upgradeButtons;
    [SerializeField] Sprite[] levelStars;
    [SerializeField] Button[] languageButtons;
    [SerializeField] Sprite[] languageButtonsSprite; 
    private int panelNumber;
    private UpgradeType upgradeType;
    private int activePanel;
    private int temporaryPanelNum;

    public static MainMenuManager instance;

    private void Awake()
    {

        if(instance == null)
            instance = this;

        else      
           Destroy(gameObject);

       

        //PlayerPrefs.SetFloat("Coin",999999);
        UpgradePanelControl();

    }


    void Start()
    {


         /*  for(int i = 1; i <= 11; i++)
           {

               PlayerPrefs.SetInt("Level" + i + "Star",3);

           }
         
        */
    }

    public enum UpgradeType
    {

        Power,Endurance,Engine

    }

    void LevelControl(UpgradeType upgradeType)
    {
       
        UpgradePanelControl();

        for (int i = 1; i < 6; i++)
        {
            string key = "Panel" + panelNumber + upgradeType + "UpgradeLevel" + i;

            if (!PlayerPrefs.HasKey(key))
            {
                GameObject.Find(upgradeType + "UpgradeLevel").GetComponent<Image>().sprite = upgradeSprites[i - 1];

                if (i == 5)
                {

                    GameObject.Find(upgradeType + "UpgradeButton").GetComponent<Button>().interactable = false;

                }

                PlayerPrefs.Save();
                break;

            }
        }


    }

    void ButtonPriceControl(UpgradeType upgradeType)
    {


        string key = "Panel" + panelNumber + upgradeType + "UpgradeButton";

        if (PlayerPrefs.HasKey(key))
        {

            GameObject.Find(upgradeType + "UpgradeButton").GetComponentInChildren<TextMeshProUGUI>().text
                                                                              = PlayerPrefs.GetFloat(key).ToString();


        }


    }

    public void UpgradeButton(string buttonName)
    {

        UpgradePanelControl();

        float upgradePrice = float.Parse(GameObject.Find(buttonName + "Button").GetComponentInChildren<TextMeshProUGUI>().text);
        float totalCoin = PlayerPrefs.GetFloat("Coin");

        if (totalCoin >= upgradePrice)
        {

            AudioManager.instance.PlayAudio("purchase");
            totalCoin -= upgradePrice;
            PlayerPrefs.SetFloat("Coin", totalCoin);
            SetUpgradedValues(upgradeType);

            for (int i = 1; i < 5; i++)
            {

                if (!PlayerPrefs.HasKey("Panel" + panelNumber + upgradeType + "UpgradeLevel" + i))
                {


                    if (i == 4)
                    {

                        GameObject.Find(buttonName + "Button").GetComponent<Button>().interactable = false;

                    }

                    string key = "Panel" + panelNumber + upgradeType + "UpgradeButton";
                    PlayerPrefs.SetFloat(key, upgradePrice * 2);
                    GameObject.Find(buttonName + "Button").GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetFloat(key).ToString();
                    PlayerPrefs.SetFloat("Panel" + panelNumber + upgradeType + "UpgradeLevel" + i, 1);
                    GameObject.Find(buttonName + "Level").GetComponent<Image>().sprite = upgradeSprites[i];
                    GameObject.Find("AllCoinText").GetComponent<TextMeshProUGUI>().text = totalCoin.ToString();
                    PlayerPrefs.Save();
                    break;

                }

            }


        }

        else
        {

            StartCoroutine(MainMenuPanelManager.instance.OpenAndClosePanel("CoinErrorPanel"));
            print("Bakiye yetersiz");

        }
    }

    void SetUpgradedValues(UpgradeType upgradeType)
    {

        if (upgradeType == UpgradeType.Power)
        {

            string key = "Player" + (panelNumber + 1) + "bulletpower";
            PlayerPrefs.SetFloat(key, PlayerPrefs.GetFloat(key) * 1.05f);

        }

        else if (upgradeType == UpgradeType.Engine)
        {

            string key = "Player" + (panelNumber + 1) + "speed";
            PlayerPrefs.SetFloat(key, PlayerPrefs.GetFloat(key) * 1.05f);

        }

        else
        {

            string key = "Player" + (panelNumber + 1) + "endurance";
            PlayerPrefs.SetFloat(key, PlayerPrefs.GetFloat(key) * 1.05f);

        }
      


    }

    void UpgradePanelControl()
    {

        for (int i = 0; i < upgradePanel.Length; i++)
        {

            if (upgradePanel[i].activeSelf)
            {

                panelNumber = i;
                break;

            }

        }


    }

    public void UpgradeTypeButton(string type)
    {

        if (type == "Endurance")
            upgradeType = UpgradeType.Endurance;

        else if(type == "Power")
            upgradeType = UpgradeType.Power;

        else
            upgradeType = UpgradeType.Engine;

    }

    public void SetButtonInfo()
    {

        
        MainMenuPanelManager.instance.LoadPanel("MainUpgradePanel");
        GameObject.Find("AllCoinText").GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetFloat("Coin").ToString();
        LevelControl(UpgradeType.Power);
        LevelControl(UpgradeType.Engine);
        LevelControl(UpgradeType.Endurance);
        ButtonPriceControl(UpgradeType.Power);
        ButtonPriceControl(UpgradeType.Engine);
        ButtonPriceControl(UpgradeType.Endurance);

    }

    public void SelectSpaceshipButton()
    {

        for (int i = 0; i <= playerPanels.Length; i++)
        {

            if (playerPanels[i].activeSelf == true)
            {

                GameObject selectButton = GameObject.Find("SelectButton" + (i + 1));

                foreach (var button in playerPanels)
                {

                    selectButton.GetComponentInChildren<TextMeshProUGUI>().text = "SELECT";

                }

                selectButton.GetComponentInChildren<TextMeshProUGUI>().text = "SELECTED";
                PlayerPrefs.SetInt("SelectedSpaceship", i + 1);
                break;

            }

        }

        SelectButtonSetText();

    }

    public void SelectButtonSetText()
    {

        int index = PlayerPrefs.GetInt("SelectedSpaceship");

        if (index == 0)
            index = 1;


        foreach (var panel in playerPanels)
            panel.SetActive(false);

        playerPanels[index - 1].SetActive(true);


        string language = PlayerPrefs.GetString("Language");

        foreach (var button in selectButtons)
        {

            button.GetComponentInChildren<TextMeshProUGUI>().text = (language == "English") ? "SELECT" : "SEC";

        }
            

        GameObject selectButton = GameObject.Find("SelectButton" + index+"T");
        selectButton.GetComponent<TextMeshProUGUI>().text = (language == "English") ? "SELECTED" : "SECILI";

    }

    public void CheckCompletedLevels()
    {

        for (int i = 1; i <= 12; i++)
        {

            if (PlayerPrefs.HasKey("Level" + i + "Star"))
            {

                Transform parentGameObject = GameObject.Find("Level" + i + "Button").transform;
                GameObject childGameObject = parentGameObject.Find("AfterGame").gameObject;
                GameObject childGameObjectText = parentGameObject.Find("FirstText").gameObject;
                GameObject childGameObjectImage =parentGameObject.Find("Image").gameObject;
                childGameObjectText.SetActive(false);
                childGameObject.SetActive(true);
                childGameObjectImage.SetActive(false);
                parentGameObject.GetComponent<Button>().interactable = true;

                int starNumber = PlayerPrefs.GetInt("Level" + i + "Star");
                childGameObject.GetComponentInChildren<Image>().sprite = levelStars[starNumber];

                if (!PlayerPrefs.HasKey("Level" + (i + 1) + "Star") && i != 12)
                {

                    Transform nextParent = GameObject.Find("Level" + (i + 1) + "Button").transform;
                    nextParent.Find("FirstText").gameObject.SetActive(true);
                    nextParent.Find("Image").gameObject.SetActive(false);
                    nextParent.gameObject.GetComponent<Button>().interactable = true;
                    break;

                }

            }

        }


    }

    public void PriceButton()
    {

        PlayerPanelControl();
        GameObject priceButton = GameObject.Find("PriceButton" + (activePanel + 1));
        GameObject coinText = GameObject.Find("SSAllCoinText");
        float spaceShipPrice = float.Parse(priceButton.GetComponentInChildren<TextMeshProUGUI>().text);
        float playerCoin = PlayerPrefs.GetFloat("Coin");
       

        if (playerCoin > spaceShipPrice)
        {

            AudioManager.instance.PlayAudio("purchase");
            playerCoin -= spaceShipPrice;
            selectButtons[activePanel].SetActive(true);
            upgradeButtons[activePanel].SetActive(true);
            PlayerPrefs.SetFloat("Coin", playerCoin);
            PlayerPrefs.SetFloat("PurchasedPlayer" + (activePanel + 1), 1);
            coinText.GetComponent<TextMeshProUGUI>().text = playerCoin.ToString();
            priceButton.SetActive(false);


        }

        else
        {
    
            StartCoroutine(MainMenuPanelManager.instance.OpenAndClosePanel("CoinErrorPanel"));
            print("Coin yetersiz");

        }
    }

    private void PlayerPanelControl()
    {

        for (int i = 0; i < playerPanels.Length; i++)
        {

            if (playerPanels[i].activeSelf)
            {


                activePanel = i;
                break;

            }

        }


    }

    public void RightLeftButton()
    {

        AudioManager.instance.PlayAudio("select");
        PlayerPanelControl();
        GameObject pointsText = GameObject.FindWithTag("SSAllCoinText");
        pointsText.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetFloat("Coin").ToString();
        ControlPurchasedPlayership();
        ControlOfUnlockedSpaceship();

    }

    public void GarageButton()
    {

        GameObject.FindWithTag("SSAllCoinText").GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetFloat("Coin").ToString();

    }

    public void ControlOfUnlockedSpaceship()
    {

        temporaryPanelNum = activePanel;

        if (activePanel <= 5)
        {

            UpdatePanelLockStatus(temporaryPanelNum,activePanel+1);    

        }
               

        else if (activePanel == 6 || activePanel == 7)
        {

            if (activePanel == 7)
            {

                temporaryPanelNum = activePanel - 1;

            }

            UpdatePanelLockStatus(temporaryPanelNum, activePanel + 1);
       

        }

      

        else if (activePanel == 8 || activePanel == 9)
        {

            if (activePanel == 9)
            {

                temporaryPanelNum = activePanel - 1;

            }

            UpdatePanelLockStatus(temporaryPanelNum-1,activePanel+1);

        
        }
      

        else if(activePanel==10||activePanel==11)
        {

            if (activePanel == 11)
            {

                temporaryPanelNum = activePanel - 1;

            }

            UpdatePanelLockStatus(temporaryPanelNum-2,activePanel+1);

        }

        else if (activePanel == 12)
        {

            UpdatePanelLockStatus(temporaryPanelNum-3,activePanel+1);

        }

        else if(activePanel == 13)
        {

            UpdatePanelLockStatus(temporaryPanelNum-3,activePanel+1);

        }

        else
        {

            UpdatePanelLockStatus(temporaryPanelNum - 3, activePanel + 1);
          
        }
    }


    private void UpdatePanelLockStatus(int levelStarNumber,int priceButtonNumber)
    {

        if (PlayerPrefs.HasKey("Level" + (levelStarNumber) + "Star"))
        {

            GameObject lockImage = GameObject.Find("LockImage");
            GameObject priceButton = GameObject.Find("PriceButton" + (priceButtonNumber));

            if (lockImage != null)
                lockImage.SetActive(false);

            if (priceButton != null)
                priceButton.GetComponent<Button>().interactable = true;


        }


    }

    public void ControlPurchasedPlayership()
    {


        PlayerPanelControl();

        if (PlayerPrefs.HasKey("PurchasedPlayer" + (activePanel + 1)))
        {

            GameObject priceButton = GameObject.Find("PriceButton" + (activePanel + 1));
           

            if (priceButton != null)
            {

                priceButton.SetActive(false);
              

            }



        }

        else
        {

            if (activePanel != 0)
            {
            
                upgradeButtons[activePanel].SetActive(false);
                selectButtons[activePanel].SetActive(false);

            }


        }

    }

   public void ExitGameButton()
    {

        Application.Quit();

    }
  
   public void LanguageButton()
   {
       
        string languageButtonText = GameObject.FindWithTag("LanguageText").GetComponent<TextMeshProUGUI>().text;

        if(languageButtonText == "ENGLISH")
        {
          
            PlayerPrefs.SetString("Language", "Turkish");
            LocalizationManager.instance.LoadLocalizedText(PlayerPrefs.GetString("Language"));
            GameObject.FindWithTag("LanguageText").GetComponent<TextMeshProUGUI>().text = "TURKCE";

        }

        else if(languageButtonText == "TURKCE")
        {

            PlayerPrefs.SetString("Language","English");
            LocalizationManager.instance.LoadLocalizedText(PlayerPrefs.GetString("Language"));
            GameObject.FindWithTag("LanguageText").GetComponent<TextMeshProUGUI>().text = "ENGLISH";

        }

         UILanguageManager[] uýLanguageManagers = FindObjectsOfType<UILanguageManager>();

         foreach(UILanguageManager manager in uýLanguageManagers)
         {

            manager.UpdateText();

         }

        SceneManager.LoadScene(0);

   }
    
   public void SelectLanguageButton(string buttonName)
   {

        PlayerPrefs.SetString("Language", buttonName);

        int index = (buttonName == "English") ? 0 : 1;

        LocalizationManager.instance.LoadLocalizedText(PlayerPrefs.GetString("Language"));
       
        UILanguageManager[] uýLanguageManagers = FindObjectsOfType<UILanguageManager>();

        foreach (UILanguageManager manager in uýLanguageManagers)
        {

            manager.UpdateText();

        }

        languageButtons[0].GetComponent<Image>().sprite = languageButtonsSprite[1 - index];
        languageButtons[1].GetComponent<Image>().sprite = languageButtonsSprite[index];

    }

}
