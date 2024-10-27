using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bomb : MonoBehaviour
{

    [SerializeField] float bombPower, bombHealth;
    Animator animator;
    GameObject player;
    private bool isLoop;
    private float initialHealth;

    private void Start()
    {

        player = GameObject.FindWithTag("Player");
        initialHealth = bombHealth;
        isLoop = true;
        animator = GetComponent<Animator>();   

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.CompareTag("Player"))
        {

            DamageToPlayer();          
            animator.Play("ExplosionForStation");
            Destroy(gameObject, 0.75f);
           

        }
    }

    public void ChangeBombHealth(float damage)
    {

        bombHealth -= damage;

        if (bombHealth <= 0 && isLoop)
        {

            isLoop = false;
            AudioManager.instance.PlayAudio("explosion");
            float distance = Mathf.Abs(Vector2.Distance(player.transform.position,transform.position));
            PlayerPoints.instance.ChangeScore(initialHealth);

            if(distance <= 5)
            {

                DamageToPlayer();

            }

            animator.Play("ExplosionForStation");
            Destroy(gameObject, .75f);

        }

    }

    void DamageToPlayer()
    {
        
        Vector2 distance=(player.transform.position - transform.position).normalized;
        player.GetComponent<Rigidbody2D>().AddForce(distance*250);
        PlayerHealth.instance.ChangeHealth(bombPower);

    }

}
