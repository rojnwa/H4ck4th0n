using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    private Rigidbody2D rb2D;
    private Collider2D col2D;
    private int speed = 5;
    private int jumpForce = 7;
    private enum Grounded { Resting, Jumping, DoubleJumped };
    private Grounded isGrounded = Grounded.Resting;
    private bool doubleJumpUpgradeAcquired;
    private bool sprintUpgradeUpgradeAcquired;
    private bool dropPressed;
    [SerializeField] private CamHelper camHelper;
    private Animator animator;
    [SerializeField] private Sword sword;
    private PolygonCollider2D swordCol;
    [SerializeField] private float health = 1f;
    [SerializeField] private Slider healthbar;

    void Start() {
        col2D = GetComponent<Collider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        swordCol = sword.GetComponent<PolygonCollider2D>();
    }


    void Update() {

        if (Input.GetButtonDown("Fire1")) {
            animator.SetTrigger("Attack");
            ToggleSwordCollider();
            Invoke("ToggleSwordCollider", 0.3f);
        }

        if (doubleJumpUpgradeAcquired && (isGrounded == Grounded.Jumping) && Input.GetButtonDown("Jump")) {
            isGrounded = Grounded.DoubleJumped;
            rb2D.velocity=Vector2.zero;
            rb2D.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            animator.SetTrigger("DoubleJump");
        }

        if (Input.GetButtonDown("Jump") && (isGrounded == Grounded.Resting)) {
            animator.SetTrigger("Jump");
            isGrounded = Grounded.Jumping;
            rb2D.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }

        healthbar.value = health;
    }

    void GetDamage(float damage) {
        health -= damage;
    }

    void jumpUpgradeAcquired()
    {
        doubleJumpUpgradeAcquired = true;
        var fireWingsGameObject = GetComponentInChildren<FireWings>().gameObject;
        fireWingsGameObject.transform.localScale=fireWingsGameObject.GetComponent<FireWings>().oScale;
    }

    void ToggleSwordCollider() {
        swordCol.enabled = !swordCol.enabled;
    }

    void FixedUpdate() {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        if (Input.GetButton("Left")) {
            rb2D.transform.Translate(-transform.right * speed * Time.fixedDeltaTime);
            //camHelper.transform.localPosition = new Vector3(-5, 0, 0);
            //transform.eulerAngles = new Vector3(0, 180, 0);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 180, 0), Time.fixedDeltaTime * 10);
        }

        if (Input.GetButton("Right")) {
            rb2D.transform.Translate(transform.right * speed * Time.fixedDeltaTime);
            //camHelper.transform.localPosition = new Vector3(5, 0, 0);
            //transform.eulerAngles = new Vector3(0, 0, 0);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 0, 0), Time.fixedDeltaTime * 10);
        }

        speed *= (Input.GetButton("Sprint") && sprintUpgradeUpgradeAcquired) ? 2 : 1;

    }


    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.GetComponent<GroundScript>()) {
            isGrounded = Grounded.Resting;
        }
    }

    void OnCollisionStay2D(Collision2D other) {
        if (Input.GetButtonDown("Down") && other.gameObject.GetComponent<Dropable>()) {
            DropThrough();
            Invoke("DropThrough", 0.5f);
        }

    }

    void DropThrough() {
        col2D.enabled = !col2D.enabled;
    }

    void resetGrounded() {
        isGrounded = Grounded.Resting;
    }

}
