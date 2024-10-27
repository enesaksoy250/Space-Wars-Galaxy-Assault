using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    PhotonView pw;

    private void Awake()
    {     
        pw = GetComponent<PhotonView>();
    }


    void Start()
    {


        PhotonView pw = GetComponent<PhotonView>();

        if (GameObject.FindWithTag("Camera1").transform.Find("Camera").GetComponent<OnlineCamera>().target == null)
        {
            SetupCamera("Camera1", pw);
        }
        else
        {
            SetupCamera("Camera2", pw);
        }


    }


    void SetupCamera(string cameraTag, PhotonView pw)
    {
        Transform child = GameObject.FindWithTag(cameraTag).transform.Find("Camera");
        GameObject cam = child.gameObject;
        cam.GetComponent<OnlineCamera>().target = gameObject.transform;
        cam.SetActive(pw.IsMine);
    }


}
