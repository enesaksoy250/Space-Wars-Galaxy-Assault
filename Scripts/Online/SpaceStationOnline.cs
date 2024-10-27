using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpaceStationOnline : MonoBehaviour
{

    [SerializeField] GameObject[] spaceShips;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] Image healthBar;
    [SerializeField] float outputRange;
    [SerializeField] float stationHealth;
    [SerializeField] int whichPlayer;
    Animator animator;
    PhotonView pw;
    private float initialStationHealth;    
    public bool createShip=true;
    private bool coroutineBool=true,loop=true;
    private bool isStart = false;
  
  

    private void Awake()
    {
   
        pw = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        initialStationHealth = stationHealth;
        
    }

   

    private void Update()
    {

        
          GetStartValue();

        if (isStart && coroutineBool && PhotonNetwork.IsMasterClient)
        {

            StartCoroutine(CreateSpaceshipCoroutine());
            coroutineBool = false;

        }

    }

    private void GetStartValue()
    {

        isStart = whichPlayer == 1 ? OnlineGameManager.instance.player1Start : OnlineGameManager.instance.player2Start;

    }


     IEnumerator CreateSpaceshipCoroutine()
     {

        while (true)
        {
                     
             int randomNumber = Random.Range(0,4);
             string shipName = spaceShips[randomNumber].name;
                           
             PhotonNetwork.Instantiate(shipName,spawnPoints[0].position, Quaternion.identity);
          
             yield return new WaitForSeconds(outputRange);

        }        
    }

    public void ChangeStationHealth(float damage)
    {

        stationHealth -= damage;
        healthBar.transform.parent.gameObject.SetActive(true);
        healthBar.fillAmount = (stationHealth / initialStationHealth);

        if (stationHealth <= 0 && loop)
        {

            loop = false;
            OnlineGameManager.instance.UpdateScoreText(whichPlayer, initialStationHealth);        
            StopCoroutine(CreateSpaceshipCoroutine());
            animator.Play("ExplosionForStation");        
            Invoke(nameof(DestroyStation),.75f);

        }



    }

    [PunRPC]
    public void RequestDestroy()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void DestroyStation()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            pw.RPC("RequestDestroy", RpcTarget.MasterClient);
        }

        else
        {

            PhotonNetwork.Destroy(gameObject);

        }
    }

   
}
