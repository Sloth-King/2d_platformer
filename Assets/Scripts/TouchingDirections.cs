using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    public ContactFilter2D contactFilter;
    public float groundDistance = 0.1f;
    public bool isTouchingLeftWall;
    public bool isTouchingRightWall;
    Rigidbody2D rb;
    CapsuleCollider2D touchingCollider;
   
    void Update(){
        checkRaycastRightWall();
        checkRaycastLeftWall();
    }

   void checkRaycastRightWall(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, groundDistance, LayerMask.GetMask("Ground"));
        isTouchingRightWall = hit.collider != null;
   }

    void checkRaycastLeftWall(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, groundDistance, LayerMask.GetMask("Ground"));
        isTouchingLeftWall = hit.collider != null;
    }
    // Update is called once per frame
}
