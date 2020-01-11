using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingAcquisition : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            collision.gameObject.SendMessage("jumpUpgradeAcquired");
        }
    }
}
