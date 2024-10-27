using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Asteroid : MonoBehaviour
{

    Animator animator;
    [SerializeField] float health;
    private float initialHealth;
    private int sceneIndex;
    public int targetPlayer;

    private void Awake()
    {
       
        initialHealth = health;
        animator = GetComponent<Animator>();

        if (!PlayerPrefs.HasKey(gameObject.name)) 
        {

            PlayerPrefs.SetFloat(gameObject.name, health);

        }

        sceneIndex = SceneManager.GetActiveScene().buildIndex;

    }

   
    public void ChangeAsteroidHealth(float damage)
    {

        health -= damage;

        if(health <= (PlayerPrefs.GetFloat(gameObject.name)/2))
        {

            animator.Play(gameObject.name);

        }

        if(health <= 0)
        {

            if (sceneIndex != 13)
                PlayerPoints.instance.ChangeScore(initialHealth);
            
     
            Destroy(gameObject);

        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {

    
            ChangeAsteroidHealth(20);
      
        }


    }

}
