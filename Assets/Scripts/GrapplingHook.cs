using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] private GameObject target;
    public bool isHooking = false;
    public bool isThrowing = false;
    [SerializeField] private float grabDistance = 3f;
    [SerializeField] private GameObject line;
    private Vector3 desiredPosition = Vector3.zero;
    [SerializeField] private TouchingDirections touchingDirections;
    void Update()
    {
        GrappingHook();
    }
    void GrappingHook()
    {
        //Quand le joueur maintient la touche E il peut viser un point pour le grapin et lorsqu'il lache la touche E il est attiré vers le point visé
        
        //Attendre que le joueur appuie sur la touche E
        if (Input.GetKeyDown(KeyCode.E) && !isHooking && !isThrowing)
        {
            isThrowing = true;
            //Récupérer la position du point visé


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
        //si le joueur lache la touche E alors qu'il est hook ou si le joueur est arrivé à destination
        if (Input.GetKeyUp(KeyCode.E) && isHooking || transform.position == desiredPosition || touchingDirections.isGround)
        {
            isHooking = false;
            
            target.SetActive(false);
            line.SetActive(false);
        }else if(Input.GetKeyUp(KeyCode.E) && !isHooking && target.activeSelf)
        {
            isHooking = true;
        }
        

        if(isHooking)
        {
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
                desiredPosition = hit.point;
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

    void HideLine()
    {
        if(!isHooking)
            line.SetActive(false);
    }

}
