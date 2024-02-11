using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    GameObject spawnPoint;
    Transform respawnPoint;
    List<GameObject> RespawnPoints = new List<GameObject>();
    Animator respawnAnim;
    [SerializeField]
    GameObject camera;

    private void Awake()
    {
        RespawnPoints.Add(spawnPoint);
    }
    void Update(){
        if(transform.position.y <= -20f){
            camera.SetActive(false);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("MoonLevel"))
        {
            GetComponent<Rigidbody2D>().gravityScale *=0.16f;
        }
        if (other.gameObject.CompareTag("Respawn") && !RespawnPoints.Contains(other.gameObject))
        {
            respawnAnim = other.gameObject.GetComponent<Animator>();
            RespawnPoints.Add(other.gameObject);
            respawnAnim.Play("Unlock");
            other.gameObject.GetComponent<AudioSource>().Play();
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            respawnPoint = RespawnPoints[RespawnPoints.Count - 1].transform;
            //tkt lance l'anim
            transform.position = respawnPoint.position;
            camera.SetActive(true);
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(0, 0);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("MoonLevel"))
        {
            GetComponent<Rigidbody2D>().gravityScale /= 0.16f;
        }
    }
}
