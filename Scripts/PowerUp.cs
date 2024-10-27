
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

 
    [SerializeField] float powerUpTime;
    GameObject player;
    GameObject disabledObject;  
    public GameObject powerUpPrefab;
   
    private void Start()
    {
        
        player = GameObject.FindWithTag("Player");

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {

            GameObject parentObject = GameObject.Find("Main" + gameObject.name);
          

            if (parentObject != null)
            {
                
                Transform childTransform=parentObject.transform.Find(gameObject.name);
               

                if (childTransform != null)
                {

                    AudioManager.instance.PlayAudio("pickup");
                    disabledObject = childTransform.gameObject;
                    disabledObject.SetActive(true);
                    PowerUpControl();
                    gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    gameObject.GetComponent<CircleCollider2D>().enabled = false;
                    Destroy(gameObject, powerUpTime+1);
                    

                }
            }

        }

    }

   
    private void PowerUpControl()
    {

        switch (gameObject.name) 
        {
            case "PowerUpSpeed":
                PlayerMove.instance.speed *= 2;
                Invoke(nameof(DownSpeed), powerUpTime);
                break;
                              
            case "PowerUpHealth":
                PlayerHealth.instance.PowerUpHealth();
                Invoke(nameof(ClosePowerUpHealthImage), powerUpTime);
                break;
          
            case "PowerUpShield":
                PlayerHealth.instance.shield = true;
                GameObject parentObject = GameObject.FindWithTag("Player");
                Transform child = parentObject.transform.Find("Shield");
                child.transform.gameObject.SetActive(true);
                Invoke(nameof(DestroyShield), powerUpTime);
                 break;
                    
            case "PowerUpCoins":
                PlayerPoints.instance.isSuperPoints = true;
                Invoke(nameof(CloseCoinUpImage), powerUpTime);
                break;

            case "PowerUpTime":
                Stopwatch.instance.UpdateTime(15);
                Invoke(nameof(ClosePowerUpTimeImage), powerUpTime);
                break;
           
        }



    }
  

   

    void DownSpeed()
    {

        PlayerMove.instance.speed /= 2;
        disabledObject.SetActive(false);
        

    }

    void ClosePowerUpHealthImage()
    {


        disabledObject.SetActive(false);


    }

  
    void ClosePowerUpFireImage()
    {

  
        PlayerFire.instance.bulletFire = true;
        PlayerFire.instance.missileFire = false;
        disabledObject.SetActive(false);

    }

     void DestroyShield()
     {
     
        PlayerHealth.instance.shield = false;
        GameObject.Find("Shield").SetActive(false);        
        disabledObject.SetActive(false);

     }
  
    void CloseCoinUpImage()
    {

        disabledObject.SetActive(false);
        PlayerPoints.instance.isSuperPoints = false;

    }

    private void ClosePowerUpTimeImage()
    {

        disabledObject.SetActive(false);

    }
}
