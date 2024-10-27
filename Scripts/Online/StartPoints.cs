using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StartPoints : MonoBehaviour
{

    [SerializeField] Transform[] spawnPoints;
   
    public Vector3 SetStartPoint(GameObject player)
    {

        int actorNumber = player.GetComponent<PhotonView>().Owner.ActorNumber;

        if (actorNumber == 1)
        {


            return spawnPoints[0].position;


        }

        else
        {


            return spawnPoints[1].position;
        
        
        }

  

    }


}
