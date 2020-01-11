using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoesAcquisation : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            Bridge bridge = gameObject.GetComponentInChildren<Bridge>();
            collision.gameObject.SendMessage("walkUpgradeAcquired");
            bridge.GetComponent<BoxCollider2D>().enabled = true;
            bridge.setActiveTransformation();
            GetComponent<Collider2D>().enabled = false;
            Destroy(GetComponentInChildren<Leg>().gameObject);
        }
    }
}
