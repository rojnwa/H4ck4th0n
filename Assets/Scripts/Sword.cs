using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {

    private Collider2D col2D;
    private float currentDamage;

    void Start() {
        col2D = GetComponent<Collider2D>();

    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<Hittable>()) {
            other.gameObject.SendMessage("Hurt", currentDamage);
        }
    }
}
