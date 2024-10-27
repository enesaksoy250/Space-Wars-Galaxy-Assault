using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpaceship : MonoBehaviour
{

    
    [SerializeField] float health, power,chasingDistance, moveSpeed;
    GameObject player;
    Animator animator;
    Rigidbody2D rb;
    private bool isLoop=true,explosion=true;
    private float initialHealth;
 
    
    private void Awake()
    {
       
        initialHealth = health;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();    

    }

    void Start()
    {

        player = GameObject.FindWithTag("Player");

    }

    
    void Update()
    {

        float distanceToPlayer = Mathf.Abs(Vector2.Distance(transform.position, player.transform.position));

        if(distanceToPlayer <= chasingDistance)
        {

            MoveToPlayer();

        }

        
    }

  
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player") && isLoop && explosion)
        {
            Explosion();
        }

        else if (collision.gameObject.CompareTag("Shield") && isLoop && explosion)
        {
            animator.Play("ExplosionForStation");
            Destroy(gameObject,.75f);
        }

    }

    void MoveToPlayer()
    {

        SetRotationToPlayer();

        float distanceToPlayer = Mathf.Abs(Vector2.Distance(transform.position, player.transform.position));

        Vector2 center1 = player.transform.position;
        Vector2 center2 = transform.position;
        float angle = Mathf.Atan2(center2.y - center1.y, center2.x - center1.x) * Mathf.Rad2Deg;

        if (distanceToPlayer < chasingDistance)
        {

            transform.rotation = Quaternion.Euler(0, 0, angle + 90);
            Vector2 distance = (player.transform.position - transform.position).normalized;
            rb.velocity = distance * moveSpeed;
           

        }


    }

    void SetRotationToPlayer()
    {

        Vector2 center1 = player.transform.position;
        Vector2 center2 = transform.position;
        float angle = Mathf.Atan2(center2.y - center1.y, center2.x - center1.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);

    }

    public void ChangeHealth(float damage)
    {

        
        health -= damage;

        if(health <= 0 && explosion)
        {

            explosion = false;
            AudioManager.instance.PlayAudio("explosion");
            animator.Play("ExplosionForStation");
            PlayerPoints.instance.ChangeScore(initialHealth);
            Animator[] animators = GetComponentsInChildren<Animator>(); 

            if(animators.Length > 0)
            {

                foreach (Animator anim in animators)
                {

                    anim.StopPlayback();

                }

            }
      

            Destroy(gameObject, .75f);

        }


    }


    private void Explosion()
    {


        isLoop = false;
        AudioManager.instance.PlayAudio("explosion");
        rb.velocity = Vector3.zero;
        animator.Play("ExplosionForStation");
        Vector2 distance = (player.transform.position - transform.position).normalized;
        player.GetComponent<Rigidbody2D>().AddForce(distance * 250);
        PlayerHealth.instance.ChangeHealth(power);
        Destroy(gameObject, .75f);

    }
}
