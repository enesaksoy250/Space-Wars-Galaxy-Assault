using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveOnline : MonoBehaviour
{
    public float speed, rotationSpeed, stoppingSpeed, temporarySpeed;
    private bool start=false;
    private FollowButton buttonFollow;
    public Rigidbody2D rb;
    PhotonView pw;
    PlayerHealthOnline playerHealthOnline;
  
    private void Awake()
    {

        playerHealthOnline = GetComponent<PlayerHealthOnline>();
        pw = GetComponent<PhotonView>();
        buttonFollow = FindObjectOfType<FollowButton>();
        rb = GetComponent<Rigidbody2D>();
        SpeedControl();

    }


    private void Update()
    {

        GetStartValue();

        if (pw.IsMine && start)
        {

            rb.AddForce(buttonFollow.position.normalized * speed * Time.deltaTime);

            if (buttonFollow.position != new Vector3(0, 0, 0))
            {

                float angle = Mathf.Atan2(buttonFollow.position.y, buttonFollow.position.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            }


        }


    }

    private void GetStartValue()
    {

        int playerIndex = playerHealthOnline.GetPlayerIndex();
        start = playerIndex == 1 ? OnlineGameManager.instance.player1Start : OnlineGameManager.instance.player2Start;

    }
    void SpeedControl()
    {

        if (!PlayerPrefs.HasKey(gameObject.name + "speed"))
        {

            PlayerPrefs.SetFloat(gameObject.name + "speed", speed);

        }

        else
        {

            speed = PlayerPrefs.GetFloat(gameObject.name + "speed");

        }


    }
}
