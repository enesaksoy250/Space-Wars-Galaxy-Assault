using UnityEngine;
using System.Collections;

public class ParallaxLayer : MonoBehaviour {

	Vector3 wantedPosition;
	public float movement_resistance; 
	Camera cam;

    private void Awake()
    {

		cam = GameObject.Find("Camera").GetComponent<Camera>();

    }

    void FixedUpdate () {
		
		  wantedPosition = cam.transform.position * movement_resistance;
	      wantedPosition.z = transform.position.z;
		  transform.position = wantedPosition;
			
	}
}
