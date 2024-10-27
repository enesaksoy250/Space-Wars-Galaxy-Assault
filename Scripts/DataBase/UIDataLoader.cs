using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDataLoader : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI userNameText;
    [SerializeField] private TextMeshProUGUI totalTimeText, totalDestructionText, totalLossText;
    [SerializeField] TextMeshProUGUI[] userNamesText, WinsText;


    void Start()
    {

        Invoke(nameof(PrintTopTenUsers), 1);   

    }

 

    public void SetInformation()
    {

        userNameText.text = UserFirebaseInformation.instance.userName;
        totalDestructionText.text = UserFirebaseInformation.instance.totalDestruction.ToString();
        totalTimeText.text = UserFirebaseInformation.instance.gameTime;
        totalLossText.text = UserFirebaseInformation.instance.totalLoss.ToString();

    }

    public void SetLeaderBoard()
    {

        for (int i = 0; i < 10; i++)
        {

            string language = PlayerPrefs.GetString("Language");
            userNamesText[i].text = UserFirebaseInformation.instance.usernameArray[i];
            string wins = UserFirebaseInformation.instance.winsArray[i].ToString();
            WinsText[i].text = (language == "English") ? wins + " win" : wins + " galibiyet";

        }


    }

   private void PrintTopTenUsers()
   {

        DataBaseManager.instance.PrintTopTenUsers();

   }

}
