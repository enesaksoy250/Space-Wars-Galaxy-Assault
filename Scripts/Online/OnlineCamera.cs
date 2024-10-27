using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OnlineCamera : MonoBehaviour
{


    [SerializeField] Vector3 offset;
    [SerializeField] float cameraChangeSpeed;
    private const float followSpeed = .1f;
    [HideInInspector]public Transform target; 
    private bool isLoop=true;
    private float rotateZ;
    Coroutine currentCoroutine;
    private string currentCoroutineType;
   

    private void LateUpdate()
    {

        if (target != null)
        {

            //transform.position = target.position + offset;
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed);
            SetCameraPosition();

        }
            
    }

    private void SetCameraPosition()
    {

        rotateZ = target.rotation.eulerAngles.z;

        if (rotateZ > 90 && rotateZ < 270)
        {
            StartCoroutineIfNotRunning(LowerTheCamera(), "LowerTheCamera");
        }
        
        else
        {
            StartCoroutineIfNotRunning(RaiseCamera(), "RaiseCamera");
        }
    }

    private void StartCoroutineIfNotRunning(IEnumerator coroutine, string coroutineType)
    {
        if (currentCoroutineType != coroutineType)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
        
            currentCoroutine = StartCoroutine(coroutine);
            currentCoroutineType = coroutineType;
        }
    }
    IEnumerator RaiseCamera()
    {

        isLoop = false;

        while (offset.y <= 5)
        {
           
           
            yield return new WaitForSeconds(.01f);
            offset += new Vector3(0,cameraChangeSpeed, 0);        
       
        }
       
        offset.y = 5;
        isLoop = true;   
   
    }
    
    IEnumerator LowerTheCamera()
    {

        isLoop = false;

        while (offset.y >= -5)
        {
                    
            yield return new WaitForSeconds(.01f);
            offset -= new Vector3(0,cameraChangeSpeed, 0);
          
        }
       
        offset.y = -5;
        isLoop = true;
    }

}
