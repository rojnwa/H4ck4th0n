using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleTrap : MonoBehaviour
{
    Collider2D collider;
    BoxCollider2D boxCollider;
    bool bossfightActivated;

    private void Start()
    {
        bossfightActivated = false;
        collider = this.gameObject.GetComponent<PolygonCollider2D>();
        boxCollider = this.gameObject.GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        if (bossfightActivated)
        {
            if(transform.position.y > -1.7f)
            {
                transform.Translate(new Vector3(0, -22f,0)*Time.fixedDeltaTime);
            } else
            {
                bossfightActivated = false;
                collider.isTrigger = false;
            }
        }
    }   

    private void OnTriggerEnter2D() 
    {
        bossfightActivated = true;
        boxCollider.enabled = true;
    }
}
