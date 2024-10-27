using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuPanelManager : MonoBehaviour
{

    [SerializeField] GameObject[] mainPanels, spaceshipSelectPanels, upgradePanels;

    public static MainMenuPanelManager instance;

    private void Awake()
    {

        if (instance == null)
            instance = this;

        else
            Destroy(gameObject);


    }


    public void LoadPanel(string panelName)
    {

        foreach (GameObject panel in mainPanels)
        {

            if (panel.name == panelName)
            {


                if (panel.name == "MainUpgradePanel")
                    UpgradePanelSettings();



                else if (panel.name != "ImageSelectPanel" && panel.name != "AdPanel" && panel.name != "WelcomePanel" &&
                        panel.name != "LoginPanel")
                    CloseAllPanel();


                panel.SetActive(true);
                AudioManager.instance.PlayAudio("select");

                break;

            }

        }


    }


    private void UpgradePanelSettings()
    {

        for (int i = 0; i < spaceshipSelectPanels.Length; i++)
        {

            if (spaceshipSelectPanels[i].activeSelf)
            {

                foreach (GameObject panel in upgradePanels)
                {

                    if (panel.activeSelf)
                    {

                        panel.SetActive(false);
                        break;

                    }


                }


                upgradePanels[i].SetActive(true);

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
                break;


            }

        }

    }

    public void LoadNextSelectPanel()
    {

        for (int i = 0; i < spaceshipSelectPanels.Length - 1; i++)
        {

            if (spaceshipSelectPanels[i].activeSelf)
            {

                spaceshipSelectPanels[i].SetActive(false);
                spaceshipSelectPanels[i + 1].SetActive(true);
                break;
            }



        }


    }

    public void LoadPreviousSelectPanel()
    {

        for (int i = 0; i < spaceshipSelectPanels.Length; i++)
        {

            if (spaceshipSelectPanels[i].activeSelf && i != 0)
            {

                spaceshipSelectPanels[i].SetActive(false);
                spaceshipSelectPanels[i - 1].SetActive(true);
                break;

            }


        }


    }

    public void CloseAllPanel()
    {

        foreach (GameObject gameObject in mainPanels)
        {

            gameObject.SetActive(false);
            Time.timeScale = 1;

        }

    }

    public IEnumerator OpenAndClosePanel(string panelName)
    {

        foreach (GameObject gameObject in mainPanels)
        {

            if (gameObject.name == panelName)
            {

                AudioManager.instance.PlayAudio("error");
                gameObject.SetActive(true);
                yield return new WaitForSeconds(1f);
                gameObject.SetActive(false);
                break;

            }

        }

    }
}
