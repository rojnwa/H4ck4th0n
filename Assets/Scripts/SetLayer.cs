using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLayer : MonoBehaviour
{
    public int sortingOrder;
    public string sortingLayerName;
    void Start()
    {
        if (GetComponent<ParticleSystem>())
        {
            GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = "Player";
            GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = 3;
        }
        if (GetComponent<MeshRenderer>())
        {
            GetComponent<MeshRenderer>().sortingLayerName = "Player";
            GetComponent<MeshRenderer>().sortingOrder = 3;
        }
    }
}
