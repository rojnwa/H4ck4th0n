﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleStraight : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start() {
       Destroy(this.gameObject, 10);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.right*speed*Time.fixedDeltaTime);
    }
}
