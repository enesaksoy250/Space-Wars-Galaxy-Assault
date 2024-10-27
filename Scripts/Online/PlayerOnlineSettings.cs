using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerOnlineSettings : MonoBehaviour
{
  
    PhotonView pw;
    StartPoints startPoints;
    PlayerFireOnline fireOnline;
    private string userName;

    void Start()
    {


        FindObject();
        ButtonManagerOnline.instance.FindObject();
     

        if (pw.IsMine)
        {

            transform.position = startPoints.SetStartPoint(gameObject);
            Invoke(nameof(CloseLoadingPanel), 1f);

        }

      
    }

   
    public void GetUsername(Action<string> onUsernameReceived)
    {

          DataBaseManager.instance.GetUsername(receivedUsername =>
          {

                userName = receivedUsername;
                onUsernameReceived?.Invoke(userName);

          });
   
    }

    public void SetRotation(Quaternion rotation)
    {

        transform.rotation = rotation;

    }
    private void CloseLoadingPanel()
    {

        GameObject.FindWithTag("LoadingPanel").SetActive(false);
      
    }
   
    private void FindObject()
    {

        fireOnline = GetComponent<PlayerFireOnline>();
        startPoints = FindObjectOfType<StartPoints>();
        pw = GetComponent<PhotonView>();

    }

}
