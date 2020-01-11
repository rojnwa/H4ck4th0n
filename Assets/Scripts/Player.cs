using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private float iFrames;

    void Start() {
        health = PlayerPrefs.GetFloat("Health", 1f);
        col2D = GetComponent<Collider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        swordCol = sword.GetComponent<PolygonCollider2D>();
        if (PlayerPrefs.GetInt("BossKilled", 0) == 1) {
            jumpUpgradeAcquired();
        }

    }

    void Die() {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Menu");
    }

    void Update() {
        if (health <= 0) Die();

        if (iFrames > 0) {
            iFrames -= Time.deltaTime;
            float flashtime = 0.08333f;
            if (Time.time % flashtime < flashtime / 2) {
                foreach (var sr in gameObject.GetComponentsInChildren<SpriteRenderer>()) {
                    sr.enabled = true;
                }
            } else {
                foreach (var sr in gameObject.GetComponentsInChildren<SpriteRenderer>()) {
                    sr.enabled = false;
                }
            }
        } else {
            foreach (var sr in gameObject.GetComponentsInChildren<SpriteRenderer>()) {
                sr.enabled = true;
            }
        }

        if (Input.GetButtonDown("Fire1")) {
            animator.SetTrigger("Attack");
            ToggleSwordCollider();
            Invoke("ToggleSwordCollider", 0.3f);
        }

        if (doubleJumpUpgradeAcquired && (isGrounded == Grounded.Jumping) && Input.GetButtonDown("Jump")) {
            isGrounded = Grounded.DoubleJumped;
            rb2D.velocity = Vector2.zero;
            rb2D.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            animator.SetTrigger("DoubleJump");
        }

        if (Input.GetButtonDown("Jump") && (isGrounded == Grounded.Resting)) {
            animator.SetTrigger("Jump");
            isGrounded = Grounded.Jumping;
            rb2D.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }

        if (Input.GetButtonUp("Left") || Input.GetButtonUp("Right"))
        {
            animator.SetBool("Walks", false);
        }

        healthbar.value = health;
    }

    void GetDamage(float damage) {
        if (iFrames <= 0) {
            health -= damage;
            iFrames = 2f;
        }
    }

    void walkUpgradeAcquired() {
        sprintUpgradeUpgradeAcquired = true;
        Leg[] legs = GetComponentsInChildren<Leg>();
        for (int i = 0; i < legs.Length; i++) {
            legs[i].GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
        }
    }

    void jumpUpgradeAcquired() {
        doubleJumpUpgradeAcquired = true;
        var fireWingsGameObject = GetComponentInChildren<FireWings>().gameObject;
        fireWingsGameObject.transform.localScale = fireWingsGameObject.GetComponent<FireWings>().oScale;
    }

    void ToggleSwordCollider() {
        swordCol.enabled = !swordCol.enabled;
    }

    void FixedUpdate() {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        if (Input.GetButton("Left")) {
            animator.SetBool("Walks", true);
            rb2D.transform.Translate(-transform.right * speed * Time.fixedDeltaTime);
            //camHelper.transform.localPosition = new Vector3(-5, 0, 0);
            //transform.eulerAngles = new Vector3(0, 180, 0);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 180, 0), Time.fixedDeltaTime * 10);
        }

        if (Input.GetButton("Right")) {
            animator.SetBool("Walks", true);
            rb2D.transform.Translate(transform.right * speed * Time.fixedDeltaTime);
            //camHelper.transform.localPosition = new Vector3(5, 0, 0);
            //transform.eulerAngles = new Vector3(0, 0, 0);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 0, 0), Time.fixedDeltaTime * 10);
        }

        speed = (Input.GetButton("Sprint") && sprintUpgradeUpgradeAcquired) ? 10 : 5;

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

    void Teleport(string location) {

        PlayerPrefs.SetFloat("Health", health);
        PlayerPrefs.Save();
        SceneManager.LoadScene(location);
    }

    void resetGrounded() {
        isGrounded = Grounded.Resting;
    }

}
