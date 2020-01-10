using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private int speed = 10;
    private int jumpForce = 10;
    private enum Grounded { Resting, Jumping, DoubleJumped };
    private Grounded isGrounded = Grounded.Resting;
    private bool doubleJumpUpgradeAcquired;
    private bool sprintUpgradeUpgradeAcquired;


    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

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

        if (Input.GetButtonDown("Sprint") && sprintUpgradeUpgradeAcquired)
        {
            speed = 20;
        }
        else
        {
            speed = 10;
        }

        if (Input.GetButton("Jump") && (isGrounded == Grounded.Resting))
        {
            isGrounded = Grounded.Jumping;
            rb2D.transform.Translate(transform.up * jumpForce * Time.fixedDeltaTime);

            if (doubleJumpUpgradeAcquired && (isGrounded != Grounded.DoubleJumped) && Input.GetButton("Jump"))
            {
                isGrounded = Grounded.DoubleJumped;
                rb2D.transform.Translate(transform.up * jumpForce * Time.fixedDeltaTime);
            }
        }

    }

    void resetGrounded()
    {
        isGrounded = Grounded.Resting;
    }

}
