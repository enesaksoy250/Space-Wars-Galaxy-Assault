using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceCraft : MonoBehaviour
{

    [SerializeField] float spaceCraftHealth;
    Animator animator;
    private bool isLoop; 

    private void Awake()
    {
      
        isLoop = true;
        animator = GetComponent<Animator>();

    }
  
   
    public void ChangeSpaceCraftHealth(float damage)
    {

        spaceCraftHealth -= damage;

        if(spaceCraftHealth <= 0&&isLoop)
        {

            isLoop=false;
            animator.Play("BigExplosion");
            PlayerPoints.instance.ChangeScore(GetComponent<Rigidbody2D>().mass*2);
            //playerPoints.ChangeScore(GetComponent<Rigidbody2D>().mass*2);
            Destroy(gameObject, .75f);


        }

    } 

 

}
