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
        if (PlayerPrefs.GetInt("BossKilled", 0) != 1) 
            transform.localScale=Vector3.zero;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
