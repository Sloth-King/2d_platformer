using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D rb; 
    Animator anim;
    [SerializeField] private TouchingDirections touchingDirections;
    [SerializeField] private TouchingGround touchingGround;
    public bool isFacingRight = true;

    private bool _isMoving = false;

    public bool IsMoving{get
        {
            return _isMoving;
        }
    private set
        {
            _isMoving = value;
            anim.SetBool("IsMoving", value);
        }
    }

    public float walkSpeed = 5f;

    public float jumpImpulse = 10f;
    private bool canDoubleJump = true;
    [SerializeField]
    float doubleJumpImpulse = 5f;

    [SerializeField]
    float dashStr = 20f;
    private bool canDash = true;
    [SerializeField]
    private float dashCd = 15f;
    [SerializeField]
    private float dashTimer = 1f;
    [SerializeField]
    private TrailRenderer trail;
    [SerializeField]
    private float wallJumpDrift = 2f;




    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        //rb.velocity = new Vector2(moveInput.x * walkSpeed, moveInput.y);
        Vector3 mvDirection = new Vector3(moveInput.x,0, 0);
        // Si le joueur appuie sur la touche de saut et qu'il peut sauter (il est sur le sol) alors il saute pendant que la touche est appuyée
        mvDirection = transform.position + mvDirection * walkSpeed * Time.deltaTime;
        transform.position = mvDirection;
        anim.SetFloat("y_velocity", rb.velocity.y);
        if((GetComponent<AudioSource>().clip == Resources.Load<AudioClip>("30_Jump_03") || GetComponent<AudioSource>().clip == Resources.Load<AudioClip>("56_Attack_03")) && touchingGround.isGround && !GetComponent<AudioSource>().isPlaying){
                GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("45_Landing_01");
                GetComponent<AudioSource>().Play();
            }
        if(IsMoving && touchingGround.isGround){
            anim.SetBool("IsMoving",true);
            if(!GetComponent<AudioSource>().isPlaying){
                GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("03_Step_grass_03");
                GetComponent<AudioSource>().Play();
            } 
        }

        if(touchingDirections.isTouchingLeftWall || touchingDirections.isTouchingRightWall){
            if(rb.velocity.y < 0){
                rb.velocity = new Vector2(0,rb.velocity.y*0.9f);
                anim.SetBool("IsOnWall",true);
            }
        }else{
            anim.SetBool("IsOnWall",false);
        }

    }

    void FacingDirection(Vector2 moveInput){
        if(moveInput.x > 0){
            transform.localScale = new Vector3(1,1,1);
            isFacingRight = true;
        }else if(moveInput.x < 0){
            transform.localScale = new Vector3(-1,1,1);
            isFacingRight = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context){
        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero;
        FacingDirection(moveInput);        
    }

    public void OnJump(InputAction.CallbackContext context){
        if(context.performed && touchingDirections.isTouchingLeftWall || context.performed && touchingDirections.isTouchingRightWall){
            anim.SetTrigger("Jump");
            if(touchingDirections.isTouchingRightWall){
                rb.velocity = new Vector2(-wallJumpDrift,jumpImpulse);
            }else{
                rb.velocity = new Vector2(wallJumpDrift,jumpImpulse);
            }
            
            GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("30_Jump_03");
            GetComponent<AudioSource>().Play();
            return;
        }
        if(context.performed && touchingGround.isGround){
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
            anim.SetTrigger("Jump");
            canDoubleJump = true;
            GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("30_Jump_03");
            GetComponent<AudioSource>().Play();
            
        }
        if(canDoubleJump && !touchingGround.isGround && context.performed){
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpImpulse);
            anim.SetTrigger("Jump");
            canDoubleJump = false;
            GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("30_Jump_03");
            GetComponent<AudioSource>().Play();
        }
    }


    private IEnumerator Dash(InputAction.CallbackContext context){
        if(context.performed){

            //Activate rb's FreezeRotationY to avoid the player to rotate while dashing
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("56_Attack_03");
            GetComponent<AudioSource>().Play();
            if(canDoubleJump) {
                canDoubleJump = false;
                Invoke("ResetDoubleJump", dashTimer);
            }
            Vector2 oldVelocity = rb.velocity;
            rb.velocity = new Vector2(transform.localScale.x * dashStr, 0);
            trail.emitting = true;
            if(touchingGround.isGround) anim.SetBool("IsMoving",true);
            else anim.SetTrigger("Jump");

            yield return new WaitForSeconds(dashTimer);
            //Deactivate rb's FreezeRotationY to allow the player to rotate again
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.velocity = oldVelocity;
            trail.emitting = false;
            anim.SetBool("IsMoving",false);
            yield return new WaitForSeconds(dashCd);
            canDash = true;
        }
    }
    void ResetDoubleJump(){
        canDoubleJump = true;
    }

    public void OnDash(InputAction.CallbackContext context){
        if(context.performed && canDash && !GetComponent<GrapplingHook>().isHooking){
            StartCoroutine(Dash(context));
            canDash = false;
        }
    }

}
