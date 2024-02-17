using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingGround : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private bool _isGround;
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
}
