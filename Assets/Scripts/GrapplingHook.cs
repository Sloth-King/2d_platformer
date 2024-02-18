using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] public GameObject target;
    public bool isHooking = false;
    public bool isThrowing = false;
    [SerializeField] private float grabDistance = 3f;
    [SerializeField] public GameObject line;
    private Vector3 desiredPosition = Vector3.zero;
    [SerializeField] private TouchingGround touchingGround;

    Vector3 distanceBetweenHitAndItemGrabbed;
    private GameObject itemGrabbed;
    void Update()
    {
        if((transform.position == desiredPosition || touchingGround.isGround) && isHooking)
        {
            isHooking = false;
            
            target.SetActive(false);
            line.SetActive(false);
        }

        if(isHooking)
        {
            desiredPosition = itemGrabbed.transform.position + distanceBetweenHitAndItemGrabbed;
            target.transform.position = Vector3.MoveTowards(target.transform.position, desiredPosition, 0.5f);
            if (Input.mouseScrollDelta.y > 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, desiredPosition, 0.1f);
            }
            if (Input.mouseScrollDelta.y < 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, desiredPosition, -0.1f);
            }
            if(!line.activeSelf)
            {
                line.SetActive(true);
            }
            line.GetComponent<LineRenderer>().SetPosition(0, transform.position);
            line.GetComponent<LineRenderer>().SetPosition(1, target.transform.position);
            //Réduire sa largeur
            line.GetComponent<LineRenderer>().startWidth = 0.1f;
            line.GetComponent<LineRenderer>().endWidth = 0.1f;
        }
    }
    public void GrappingHook(InputAction.CallbackContext context)
    {
        
        //If the player start to press the context button and is not already hooking or throwing we start the throwing process
        if (context.started && !isHooking && !isThrowing)
        {
            isThrowing = true;

            //We set the target position to the player position
            target.transform.position = transform.position;

            if(GetComponent<PlayerController>().isFacingRight)
            {
                desiredPosition = transform.position + new Vector3(grabDistance, grabDistance, 0);
            }else
            {
                desiredPosition = transform.position + new Vector3(-grabDistance, grabDistance, 0);
            }
            desiredPosition.z = 0;
            StartCoroutine(Grab(desiredPosition));
        }
        
        //If the player release the context button and is hooking we stop the hooking process
        if((context.canceled &&  isHooking))
        {
            isHooking = false;
            
            target.SetActive(false);
            line.SetActive(false);
        }else if(context.performed && !isHooking && target.activeSelf)
        {
            isHooking = true;
        }
    }


    //Coroutine to move the target to the desired position
    IEnumerator Grab(Vector3 desiredPosition){
        RaycastHit2D hit;
        if(!line.activeSelf)
        {
            line.SetActive(true);
        }
        while(target.transform.position != desiredPosition && !isHooking)
        {
            target.transform.position = Vector3.MoveTowards(target.transform.position, desiredPosition, 0.5f);
            line.GetComponent<LineRenderer>().SetPosition(0, transform.position);
            line.GetComponent<LineRenderer>().SetPosition(1, target.transform.position);
            //Réduire sa largeur
            line.GetComponent<LineRenderer>().startWidth = 0.1f;
            line.GetComponent<LineRenderer>().endWidth = 0.1f;
            hit = Physics2D.Raycast(target.transform.position, transform.position - target.transform.position, grabDistance);
            if (hit.collider != null && hit.collider.gameObject != gameObject && hit.collider.gameObject.CompareTag("Ground"))
            {
                isHooking = true;
                itemGrabbed = hit.collider.gameObject;
                //Desired position is the point of impact of the obstacle in reference of itemGrabbed
                desiredPosition = hit.point;
                distanceBetweenHitAndItemGrabbed = desiredPosition - itemGrabbed.transform.position;

            }

            yield return new WaitForSeconds(0.005f);
        }
        if (isHooking)
        {
            target.SetActive(true);
            target.transform.position = Vector3.MoveTowards(target.transform.position, desiredPosition, 0.5f);
            //couleur qui change en vert si le point visé est le point d'impact de l'obstacle
            target.GetComponent<SpriteRenderer>().color = Color.green;
        }else
        {
            target.SetActive(false);
        }

        Invoke("HideLine", 0.1f);

        isThrowing = false;

    }

    //Hide the line of the grappling hook
    void HideLine()
    {
        if(!isHooking)
            line.SetActive(false);
    }

}
