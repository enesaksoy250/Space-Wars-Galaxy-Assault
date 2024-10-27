using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    GameObject player;


    private void Awake()
    {

        player = GameObject.FindWithTag("Player");

    }

    
    void Update()
    {
        
        transform.position = player.transform.position;

    }
}
