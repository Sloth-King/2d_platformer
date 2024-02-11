using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heghog : MonoBehaviour
{
    public float walkSpeed = 5f;
    Rigidbody2D rb;
    int rotationNum = 0;
    int sleepTime;
    Animator anim;
    private bool isSleeping = false;
    float timeDistance = 0;
    private bool isWaitingToSleep = false;


    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        sleepTime = UnityEngine.Random.Range(2, 5);
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(rotationNum == sleepTime && !isWaitingToSleep){
            float minTimeDistance = timeDistance/6f;
            isWaitingToSleep = true;
            float timeToWait = UnityEngine.Random.Range(minTimeDistance,timeDistance);
            StartCoroutine(Sleep(timeToWait));
        }
    }

    IEnumerator Sleep(float time){
        if(time>0){
            yield return new WaitForSeconds(time);
        }
        isSleeping = true;
        GetComponent<AudioSource>().Stop();
        rb.velocity = new Vector2(0, rb.velocity.y);
        anim.Play("sleep");
        yield return new WaitForSeconds(2);
        rotationNum = 0;
        sleepTime = UnityEngine.Random.Range(2, 5);
        anim.Play("hog_walk");
        isSleeping = false;
        isWaitingToSleep = false;
    }

    private void FixedUpdate(){
        if(!isSleeping){ 
            if(!GetComponent<AudioSource>().isPlaying){
                GetComponent<AudioSource>().Play();
            }
            rb.velocity = new Vector2(walkSpeed * Vector2.right.x, rb.velocity.y);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("HedgehogLim"))
        {
            walkSpeed *= -1;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            rotationNum++;
            if(timeDistance.Equals(0)){
                if(rotationNum == 1)
                {
                    //calculate the time it takes for rotationNum to reach 2
                    timeDistance = Time.time;
                }
                if(rotationNum == 2)
                {
                    timeDistance = Time.time - timeDistance;
                }
            }
        }

    }


}
