using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    public ContactFilter2D contactFilter;
    public float groundDistance = 0.05f;
    
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private bool _isGround;
    public bool isTouchingLeftWall;
    public bool isTouchingRightWall;
    Rigidbody2D rb;
    CapsuleCollider2D touchingCollider;
    

    public bool isGround{get
        {
            return _isGround;
        } 
        private set
        {
            _isGround = value;
            anim.SetBool("IsGround", value);
        }
    }
   
    void Update(){
        checkRaycastRightWall();
        checkRaycastLeftWall();
    }

   void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Ground")){
            isGround = true;
        }
   }
   void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.CompareTag("Ground")){
            isGround = false;
        }
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
