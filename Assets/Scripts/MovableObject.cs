using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector2 direction;

    private void Start()
    {
        direction = Vector2.left;
    }
    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Limit")
        {
            direction *= -1;
        }
    }
}
