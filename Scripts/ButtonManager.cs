using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{

    public void FireForButton()
    {
     
       PlayerFire.instance.isFiring = true;     

    }

    public void DontFireForButton()
    {

        PlayerFire.instance.isFiring = false;
  
    }

    public void StopForButton()
    {


        PlayerMove.instance.temporarySpeed = PlayerMove.instance.speed;
        PlayerMove.instance.speed = 0;
        PlayerMove.instance.rb.drag = PlayerMove.instance.stoppingSpeed;


    }

    public void DontStopForButton()
    {

        PlayerMove.instance.speed = PlayerMove.instance.temporarySpeed;
        PlayerMove.instance.rb.drag = .1f;

    }


}
