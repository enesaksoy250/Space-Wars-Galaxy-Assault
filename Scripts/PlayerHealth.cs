using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Notifications.Android;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    TextMeshProUGUI healthText;
    Animator animator;
    GameManager gameManager;
    Coroutine timerCoroutine;
    public float health; 
    private float initialHealth;
    private float gameDuration;
    private float time=0;    
    private bool isLoop=true;
    [HideInInspector] public bool shield = false;

    public static PlayerHealth instance;
    
    void Awake()
    {

        instance = this;      
        healthText=GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>();
        gameManager=FindObjectOfType<GameManager>();
        animator = GetComponent<Animator>();
        EnduranceControl();
        initialHealth = health;
        healthText.text=100.ToString();

    }

   
    public void ChangeHealth(float damage)
    {

        if(health > 0 && gameManager.isPlaying && !shield)
        {

            health -= damage;
            

            if (health <= 0 && isLoop)
            {

                health = 0;
                isLoop = false;
                Die();

            }

            else if(health > 0 && health < 1)
            {

                health = 1;

            }

            healthText.text = Mathf.RoundToInt(health / initialHealth * 100).ToString();

            if (timerCoroutine != null)
            {

                StopCoroutine(timerCoroutine);

            }

            timerCoroutine = StartCoroutine(Timer());


        }

   
   
    }

    IEnumerator Timer()
    {

        while(health > 0)
        {

            yield return new WaitForSeconds(1);
            time++;
            

            if (time == 10)
            {

                time = 0;

                while((int)((health / initialHealth) * 100)<=100)
                {

                    health += 1;
                    healthText.text = ((int)((health / initialHealth) * 100)).ToString();
                    yield return new WaitForSeconds(.1f);
                  
                    if((int)((health / initialHealth) * 100)>=100)
                    {

                        StopCoroutine(timerCoroutine);
                        timerCoroutine = null;
                        

                    }

                }


            }
        }


    }

    public void PowerUpHealth()
    {

        health += initialHealth/2;

        if(health > initialHealth)
        {

            health = initialHealth;

        }

        healthText.text = Mathf.RoundToInt((health/initialHealth)*100).ToString();

    }

    private void Die()
    {

        AudioManager.instance.PlayAudio("gameOver");
        DataBaseManager.instance.UpdateFirebaseInfo("totalLoss", 1);
       
        Stopwatch.instance.isRunning = false;
        gameDuration=Stopwatch.instance.GetGameDuration();
     
        animator.Play("Explosion");
        healthText.text = health.ToString();
       
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        int dieCount = PlayerPrefs.GetInt("Die") + 1;
        PlayerPrefs.SetInt("Die",dieCount);

        if (dieCount % 3 == 0)
        {

            AdManager.instance.ShowInterstitialAd();

        }

        Invoke(nameof(LoadPanel), 2f);

        foreach (Transform child in transform)
        {

            if (child.name == "EngineEffect")
            {

                child.gameObject.SetActive(false);

            }


        }

    }

    public void CloseEffect()
    {

        animator.gameObject.SetActive(false);

    }

    void LoadPanel()
    {

        gameManager.LoadGameOverPanel();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Asteroid"))
        {

            float mass = collision.gameObject.GetComponent<Rigidbody2D>().mass;

            if(mass == 1)
            {

                mass = 5;

            }

            ChangeHealth(mass / 5);

        }

    }

    void EnduranceControl()
    {
        
      
        if (!PlayerPrefs.HasKey(gameObject.name + "endurance"))
        {

            PlayerPrefs.SetFloat(gameObject.name + "endurance", health);

        }

        else
        {

            health = PlayerPrefs.GetFloat(gameObject.name + "endurance");
           
        }


    }
}
