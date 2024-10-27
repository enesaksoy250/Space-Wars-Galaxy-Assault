using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPoints : MonoBehaviour
{


    TextMeshProUGUI pointsText;
    public bool isSuperPoints;
    private float playerScore;
    private int buildIndex;

    public static PlayerPoints instance;

    private void Awake()
    {
        
        instance = this;
        //PlayerPrefs.SetFloat("Coin",0);
        playerScore = 0;
        isSuperPoints = false;
        buildIndex = SceneManager.GetActiveScene().buildIndex;
      
        if(buildIndex != 13)
        {

            pointsText = GameObject.FindWithTag("PointsText").GetComponent<TextMeshProUGUI>();
            UpdateScoreText();

        }


    }

    private void UpdateScoreText()
    {

        pointsText.text = PlayerPrefs.GetFloat("Coin").ToString();

    }


    public void ChangeScore(float score)
    {

        float multiplier = isSuperPoints ? 2 : 1;
        float newScore = score * multiplier;
        PlayerPrefs.SetFloat("Coin", PlayerPrefs.GetFloat("Coin") + newScore);
        playerScore += newScore;

        if (buildIndex != 13)
        {
            UpdateScoreText();
        }

    }

    public float GetPlayerScore()
    {

        return playerScore;

    }
}
