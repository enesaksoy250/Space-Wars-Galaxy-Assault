using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyFire : MonoBehaviour
{

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject[] firePositions;  
    [SerializeField] float bulletSpeed, bulletLifetime,firingRate;
    GameObject player;
    Coroutine firingCoroutinePlayer;
    public bool isFiringToPlayer=false;
    private Rigidbody2D rigidbody;

    private void Start()
    {
 
        rigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");

    }

    private void Update()
    {
    
       FireToPlayer();  

    }

   
    void FireToPlayer()
    {


        float playerHealth = PlayerHealth.instance.health;
        bool isPlaying = GameManager.instance.isPlaying;
        
        if ((playerHealth >0) && (isFiringToPlayer) && firingCoroutinePlayer == null && isPlaying)
        {
         
            firingCoroutinePlayer = StartCoroutine(FireToPlayerCoroutine());
    
        }
        
        else if ((playerHealth <=0 || !isFiringToPlayer||!isPlaying) && firingCoroutinePlayer != null)
        {
        
            StopCoroutine(firingCoroutinePlayer);
            firingCoroutinePlayer = null;
        
        }
    }

   

    IEnumerator FireToPlayerCoroutine()
    {
        while (true)
        {

                foreach (GameObject firePoint in firePositions)
                {
                    
                    GameObject instance = Instantiate(bulletPrefab, firePoint.transform.position, transform.rotation);
                    Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();

                    if (rb != null)
                    {
                        Vector2 distance = (player.transform.position - transform.position).normalized;
                        rb.AddForce(distance * bulletSpeed * GetEnemyVelocity());
                    }

                    
                    Destroy(instance, bulletLifetime);

                }
                                     
                AudioManager.instance.PlayAudio("enemyFire");
              
            yield return new WaitForSeconds(firingRate);
            
         
        }
    }

  
 
    private float GetEnemyVelocity()
    {

        float x = Mathf.Clamp(Mathf.Abs(rigidbody.velocity.x), 1, 2);
        float y = Mathf.Clamp(Mathf.Abs(rigidbody.velocity.y), 1, 2);

        return Mathf.Max(x, y);

    }


}
