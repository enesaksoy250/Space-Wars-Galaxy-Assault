using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayerOnline : MonoBehaviour
{
    private Vector3 wantedPosition;
    [SerializeField] float movement_resistance;
    private Camera cam;

    private void Start()
    {

        int layerNum = gameObject.layer;

        if(layerNum == 19)
        {

            Transform child = GameObject.FindWithTag("Camera1").transform.Find("Camera");
            cam = child.gameObject.GetComponent<Camera>();

        }

        else
        {

            Transform child = GameObject.FindWithTag("Camera2").transform.Find("Camera");
            cam = child.gameObject.GetComponent<Camera>();

        }
        

    }

    void LateUpdate()
    {

        wantedPosition = cam.transform.position * movement_resistance;
        wantedPosition.z = transform.position.z;
        transform.position = wantedPosition;

    }
}
