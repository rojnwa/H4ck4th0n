using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private Collider2D col2D;
    private int speed = 10;
    private int jumpForce = 7;
    private enum Grounded { Resting, Jumping, DoubleJumped };
    private Grounded isGrounded = Grounded.Resting;
    private bool doubleJumpUpgradeAcquired;
    private bool sprintUpgradeUpgradeAcquired;
    private float distToGround;


    void Start()
    {
        col2D = GetComponent<Collider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        distToGround = col2D.bounds.extents.y;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<GroundScript>())
        {
            isGrounded = Grounded.Resting;
        }
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        if (Input.GetButton("Left"))
        {
            rb2D.transform.Translate(-transform.right * speed * Time.fixedDeltaTime);
        }

        if (Input.GetButton("Right"))
        {
            rb2D.transform.Translate(transform.right * speed * Time.fixedDeltaTime);
        }

        speed = (Input.GetButton("Sprint") && sprintUpgradeUpgradeAcquired) ? 20 : 10;

        if (Input.GetButtonDown("Jump") && (isGrounded == Grounded.Resting))
        {
            isGrounded = Grounded.Jumping;
            rb2D.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);

            if (doubleJumpUpgradeAcquired && (isGrounded != Grounded.DoubleJumped) && Input.GetButtonDown("Jump"))
            {
                isGrounded = Grounded.DoubleJumped;
                rb2D.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }
        }

    }

    void resetGrounded()
    {
        isGrounded = Grounded.Resting;
    }

}
