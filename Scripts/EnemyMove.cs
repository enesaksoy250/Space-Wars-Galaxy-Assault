using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMove : MonoBehaviour
{

    [SerializeField] float moveSpeed, stoppingDistance,smallestStoppingDistance, largestStoppingDistance;
    GameObject player;
    Rigidbody2D rb;
    EnemyFire enemyFire;
    

    void Awake()
    {      
        rb = GetComponent<Rigidbody2D>();
   
    }

    private void Start()
    {

        enemyFire = gameObject.GetComponent<EnemyFire>();
        player = GameObject.FindWithTag("Player");

    }

    void Update()
    {

        MoveToPlayer();  
    
    }

  

  
    void MoveToPlayer()
    {

        SetRotationToPlayer();

        float distanceToPlayer = Mathf.Abs(Vector2.Distance(transform.position, player.transform.position));

        Vector2 center1 = player.transform.position;
        Vector2 center2 = transform.position;
        float angle = Mathf.Atan2(center2.y - center1.y, center2.x - center1.x) * Mathf.Rad2Deg;

        if(distanceToPlayer>largestStoppingDistance)
        {

            transform.rotation = Quaternion.Euler(0, 0, angle+90);
            Vector2 distance = (player.transform.position - transform.position).normalized;
            rb.velocity = distance * moveSpeed;
            enemyFire.isFiringToPlayer = false;

        }

        else if (distanceToPlayer <= smallestStoppingDistance)
        {
        
            rb.velocity = Vector2.zero;
            enemyFire.isFiringToPlayer = true;
        }


    }

   

    void SetRotationToPlayer()
    {

        Vector2 center1 = player.transform.position;
        Vector2 center2 = transform.position;
        float angle = Mathf.Atan2(center2.y - center1.y, center2.x - center1.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);

    }

   
}
