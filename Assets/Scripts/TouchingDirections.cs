using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    public ContactFilter2D contactFilter;
    public float groundDistance = 0.05f;
    Animator anim;
    [SerializeField]
    private bool _isGround;
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

    RaycastHit2D[] groundHits = new RaycastHit2D[16];
    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        touchingCollider = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    { 
        
    }

    void FixedUpdate(){
        isGround = touchingCollider.Cast(Vector2.down, contactFilter, groundHits, groundDistance) > 0;
    }

    // Update is called once per frame
}
