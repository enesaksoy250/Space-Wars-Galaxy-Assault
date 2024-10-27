using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveButton : MonoBehaviour/*, IDragHandler, IEndDragHandler , IPointerDownHandler*/
{

    Vector3 startPosition;
    Vector2 parentStartPosition;
  
    FollowButton followButton;
    Transform highestParent;
   
    private CircleCollider2D circleCollider;
    private bool dragX=false;

    private void Awake()
    {

        followButton = FindObjectOfType<FollowButton>();
        circleCollider = GetComponentInParent<CircleCollider2D>();

    }

    private void Start()
    {

         highestParent = GetHighestParent(transform);
         startPosition = transform.position;
         parentStartPosition = highestParent.position;

    }


    private void Update()
    {
        
        if(Input.touchCount > 0)
        {

            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = touch.position;

            if(touchPosition.x < Screen.width / 2)
            {

                dragX = true;

                if (touch.phase == TouchPhase.Began)
                {
                
                   highestParent.position = touchPosition;

                }
              
                else if (touch.phase == TouchPhase.Moved)
                {

                    Vector3 newPosition = touchPosition;             
                    Vector3 constrainedPosition = circleCollider.ClosestPoint(newPosition);                  
                    transform.position = constrainedPosition;

                }
                else if (touch.phase == TouchPhase.Ended)
                {

                    dragX = false;
                    highestParent.position = parentStartPosition;
                    followButton.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);

                }


            }

            else
            {

                if (touch.phase == TouchPhase.Moved && dragX)
                {
                  
                    Vector3 newPosition = touchPosition;                    
                    Vector3 constrainedPosition = circleCollider.ClosestPoint(newPosition);
                 
                    if (transform.position.x < Screen.width / 2)
                    {
                        transform.position = constrainedPosition;
                    }
                
                }


                else if (touch.phase == TouchPhase.Ended)
                {

                    dragX = false;
                    highestParent.position = parentStartPosition;
                    followButton.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);

                }

            }

        }

    }

    /*
    public void OnDrag(PointerEventData eventData)
    {

        Vector3 newPosition = circleCollider.ClosestPoint(transform.position + (Vector3)eventData.delta);
        transform.position = newPosition;

    }



    public void OnEndDrag(PointerEventData eventData)
    {

        transform.position = startPosition;
        followButton.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);

    }

    public void OnPointerDown(PointerEventData eventData)
    {

      

    }
    */
    Transform GetHighestParent(Transform child)
    {
      
        Transform currentParent = child.parent;
     
        while (currentParent != null)
        {
            
            if (currentParent.parent.name == "PowerUpBottomPanel")
            {
                return currentParent; 
            }
            currentParent = currentParent.parent;
        }

        return null;
    }
}
