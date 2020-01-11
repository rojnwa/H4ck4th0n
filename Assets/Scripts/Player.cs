using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private Rigidbody2D rb2D;
    private Collider2D col2D;
    private int speed = 5;
    private int jumpForce = 7;
    private enum Grounded { Resting, Jumping, DoubleJumped };
    private Grounded isGrounded = Grounded.Resting;
    private bool doubleJumpUpgradeAcquired;
    private bool sprintUpgradeUpgradeAcquired;
    private float distToGround;
    private bool dropPressed;
    public CamHelper camHelper;
    private Animator animator;

    void Start() {
        col2D = GetComponent<Collider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        distToGround = col2D.bounds.extents.y;
    }


    void Update() {

    }


    void FixedUpdate() {

        if (Input.GetButton("Left")) {
            rb2D.transform.Translate(-transform.right * speed * Time.fixedDeltaTime);
            camHelper.transform.localPosition = new Vector3(-5, 0, 0);
        }

        if (Input.GetButton("Right")) {
            rb2D.transform.Translate(transform.right * speed * Time.fixedDeltaTime);
            camHelper.transform.localPosition = new Vector3(5, 0, 0);
        }

        if (Input.GetButton("Fire1")) {
            animator.SetTrigger("Attack");
        }

        if (Input.GetButtonDown("Jump") && (isGrounded == Grounded.Resting)) {
            animator.SetTrigger("Jump");
            isGrounded = Grounded.Jumping;
            rb2D.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);

            if (doubleJumpUpgradeAcquired && (isGrounded != Grounded.DoubleJumped) && Input.GetButtonDown("Jump")) {
                animator.SetTrigger("DoubleJump");
                isGrounded = Grounded.DoubleJumped;
                rb2D.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }
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
