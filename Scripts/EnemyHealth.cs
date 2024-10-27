using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField] float health;
    [SerializeField] float destructionScore;
    Animator animator;
 
    void Awake()
    {
             
        animator = GetComponent<Animator>();

    }


    public void ChangeEnemyHealth(float damage)
    {

        health -= damage;

        if(health <= 0)
        {

            EnemyDie();

        }

    }


    private void EnemyDie()
    {

        AudioManager.instance.PlayAudio("explosion");
        GetComponent<PolygonCollider2D>().enabled = false;
        PlayerPoints.instance.ChangeScore(destructionScore);
        animator.Play("BigExplosion");
        Destroy(gameObject,.75f);
        DataBaseManager.instance.UpdateFirebaseInfo("totalDestruction", 1);
   
    }


}
