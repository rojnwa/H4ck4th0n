using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private int speed = 10;
    private int jumpForce = 10;
    private enum Grounded { Resting, Jumping };
    private Grounded isGrounded = Grounded.Resting;


    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

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

        if (Input.GetButton("Jump") && (isGrounded == Grounded.Resting))
        {
            rb2D.transform.Translate(transform.up * jumpForce * Time.fixedDeltaTime);
        }

    }

}
