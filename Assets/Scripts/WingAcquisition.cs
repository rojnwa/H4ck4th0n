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
            Destroy(this.gameObject,0);
        }
        if (collision.gameObject.GetComponent<GroundScript>()){
            GetComponent<Rigidbody2D>().constraints=RigidbodyConstraints2D.FreezeAll;
        }
    }
}
