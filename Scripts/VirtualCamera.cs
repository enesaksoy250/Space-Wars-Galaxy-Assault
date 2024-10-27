using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VirtualCamera : MonoBehaviour
{

    GameObject player;
    CinemachineVirtualCamera virtualCamera;
    private bool isLoop;
    private float screenY;
    float rotateZ;

    void Start()
    {

      
        isLoop = true;
        virtualCamera=GetComponent<CinemachineVirtualCamera>();
        player = GameObject.FindWithTag("Player");
        virtualCamera.Follow = player.transform;

        
    
    }
 
    void Update()
    {

        if(player != null)
        {

             rotateZ = player.transform.rotation.eulerAngles.z;

        }

       
        screenY = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY;


        if (rotateZ > 90 && rotateZ < 270)
        {
        
            StartCoroutine(Decrease());

        }

        else
        {
        
            StartCoroutine(Increase());

        }

    }

    IEnumerator Increase()
    {

        while (screenY < .7f && isLoop)
        {

            isLoop=false;
            yield return new WaitForSeconds(.01f);
            screenY += .01f;
            virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = screenY;
            isLoop =true;

            if (screenY > .7f)
            {

                virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = .7f;

            }

        }



    }

    IEnumerator Decrease()
    {

      

        while (screenY > .3f && isLoop)
        {

            isLoop = false;
            yield return new WaitForSeconds(.01f);
            screenY -= .01f;
            virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = screenY;
            isLoop = true;

            if (screenY < .3f)
            {

                virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = .3f;

            }

        }


    }

}
