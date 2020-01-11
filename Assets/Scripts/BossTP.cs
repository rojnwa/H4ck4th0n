using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTP : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<Player>()) {
            other.gameObject.SendMessage("Teleport", "BossScene");
        }
    }
}
