using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {


    private BoxCollider2D boxCollider;

    private Rigidbody2D rb2D;

    [SerializeField]
    private float playerDamage = 0.1f;

    [SerializeField]
    private int health;

    private Animator animator;

    private enum Direction { Left, Right };

    [SerializeField]
    private Direction dir;

    [SerializeField]
    private bool isFlying;

    private Vector2 velocity;

    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        velocity = new Vector2(0, 0);
        if (!isFlying)
            velocity.y = -1;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<Player>() != null) {
            other.gameObject.GetComponent<Player>().SendMessage("GetDamage", playerDamage);
        } else if (other.gameObject.GetComponent<Sword>() != null) {
            health -= 5;
        } else {
                if (dir == Direction.Left) {
                    dir = Direction.Right;
                } else {
                    dir = Direction.Left;
                }
        }
    }
    // Update is called once per frame
    void Update() {

        if (health <= 0)
            Destroy(gameObject);

        if (dir == Direction.Left) {
            velocity.x = -1;
        } else {
            velocity.x = 1;
        }

        transform.Translate(velocity * Time.deltaTime);
    }
}
