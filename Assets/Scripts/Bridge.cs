using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bridge : MonoBehaviour
{
    private bool activeTransformation;
    private Tilemap childMap;
    // Start is called before the first frame update
    void Start()
    {
        activeTransformation = false;
        childMap = gameObject.GetComponentInChildren<TileScript>().gameObject.GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        if (activeTransformation && childMap.color.a<1)
        {
            childMap.color = new Color(childMap.color.r, childMap.color.g, childMap.color.b, Mathf.Clamp(childMap.color.a+Time.deltaTime, 0, 1));
        }
    }

    public void setActiveTransformation()
    {
        activeTransformation = true;
    }
}
