using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicle : MonoBehaviour
{
    public float timeToGetPosition;
    public float timeToPause;
    public float positionSpeed;
    public float speed;
    private float startTime;
    private Rigidbody2D rb;
    private bool hasPlayerPos = false;
    private Transform playerPos;
    private Quaternion originalRotation;


    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        startTime = Time.time;
        originalRotation = transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Time.time < startTime + timeToGetPosition){
            transform.Translate(Vector3.right * positionSpeed * Time.fixedDeltaTime);
        }

        if (Time.time > startTime + timeToGetPosition && Time.time < startTime + timeToGetPosition + timeToPause)
        {
            var player = Object.FindObjectOfType<Player>().gameObject;
            playerPos = player.transform;
            transform.LookAt(playerPos,Vector3.up);
            transform.Rotate(new Vector3(0, -90, 0), Space.Self);
            var targetRotation = transform.rotation;
            var begintime = startTime + timeToGetPosition; 
            var endtime = begintime + timeToPause;
            var v = Time.time - begintime / timeToPause;
            var lerpprogress = Mathf.Clamp(v, 0, 1);
            transform.rotation = Quaternion.Lerp(originalRotation, targetRotation, lerpprogress);
        }
        
        if(Time.time > startTime+timeToGetPosition+timeToPause) {
            transform.Translate(Vector3.right*speed*Time.fixedDeltaTime);
        }
        
    }
}
