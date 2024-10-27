using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{
   
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject[] firePositions;
    [SerializeField] float projectileSpeed, projectileLifetime, firingRate; 
    [HideInInspector] public bool isFiring,missileFire;
    [HideInInspector] public bool bulletFire=true;
    public float bulletPower;
    Coroutine firingCoroutine;
    
    public static PlayerFire instance;

    private void Awake()
    {
       
        instance = this;
        BulletPowerControl();
      
    }

    void Update()
    {
        
       Fire();
        
    }

    void Fire()
    {

        float playerHealth = PlayerHealth.instance.health;
        bool isPlaying = GameManager.instance.isPlaying;
        
        if (isFiring && firingCoroutine == null && playerHealth >0 && isPlaying)
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }

        else if(firingCoroutine!=null && (!isFiring || playerHealth < 0 || !isPlaying)) 
        {

            StopCoroutine(firingCoroutine);
            firingCoroutine = null;

        }
    }
  
    IEnumerator FireContinuously()
    {
        while (true)
        {

          
                foreach (GameObject firePoint in firePositions)
                {


                    GameObject instance = Instantiate(projectilePrefab,
                                               firePoint.transform.position,
                                               transform.rotation);

                    Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.velocity = transform.up * projectileSpeed;
                    }

               
                    Destroy(instance, projectileLifetime);

                }

                AudioManager.instance.PlayAudio("playerFire");
                yield return new WaitForSeconds(firingRate);
           
        }
        
    }

  
    void BulletPowerControl()
    {

        if (!PlayerPrefs.HasKey(gameObject.name + "bulletpower"))
        {

            PlayerPrefs.SetFloat(gameObject.name + "bulletpower", bulletPower);

        }

        else
        {

            bulletPower = PlayerPrefs.GetFloat(gameObject.name + "bulletpower");
            print("bullet power ="+bulletPower);

        }


    }

}
