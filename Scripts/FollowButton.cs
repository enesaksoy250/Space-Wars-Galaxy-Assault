using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowButton : MonoBehaviour
{


    public Vector3 position;


    private void Update()
    {

        position = GetComponent<RectTransform>().localPosition;


    }





}
