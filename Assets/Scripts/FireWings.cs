using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWings : MonoBehaviour
{
    public Vector3 oScale;
    // Start is called before the first frame update
    void Start()
    {
        oScale=transform.localScale;
        transform.localScale=Vector3.zero;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
