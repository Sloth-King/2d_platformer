using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [SerializeField] private float jumpForce = 10f;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("PlayerFeet"))
        {
            //apply force to parent of other
            other.transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector2(other.transform.parent.GetComponent<Rigidbody2D>().velocity.x, 0);
            other.transform.parent.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Debug.Log("Bounce");
            Debug.Log(other.transform.parent.name);
        }
    }
}