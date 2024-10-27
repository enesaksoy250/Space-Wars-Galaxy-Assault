using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed, rotationSpeed, stoppingSpeed;
    [HideInInspector] public float temporarySpeed;
    private FollowButton buttonFollow;
    [HideInInspector] public Rigidbody2D rb;

    public static PlayerMove instance;

    private void Awake()
    {
        
        instance = this;
        buttonFollow = FindObjectOfType<FollowButton>();
        rb = GetComponent<Rigidbody2D>();
        SpeedControl();
        
    }

  
    private void FixedUpdate()
    {
       
        rb.AddForce(buttonFollow.position.normalized * speed * Time.fixedDeltaTime);

        if (buttonFollow.position != new Vector3(0,0,0))
        {

            float angle = Mathf.Atan2(buttonFollow.position.y, buttonFollow.position.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        }
      /*
        if (rb.velocity.magnitude > 20)
        {
            rb.velocity = rb.velocity.normalized * 20;
        }
      */
    }

    void SpeedControl()
    {

        if (!PlayerPrefs.HasKey(gameObject.name + "speed"))
        {

            PlayerPrefs.SetFloat(gameObject.name + "speed", speed);
            print(speed);

        }

        else
        {

            speed = PlayerPrefs.GetFloat(gameObject.name + "speed");
            print(speed);
          
        }


    }
    
}
