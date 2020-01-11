using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : MonoBehaviour
{
    private BoxCollider2D hitBox;

    private BoxCollider2D visBox;

    private Rigidbody2D rb2D;

    [SerializeField]
    private float playerDamage = 0.1f;

    [SerializeField]
    private int health;

    private Animator animator;

    private enum Direction { Left, Right };

    [SerializeField]
    private Direction dir;

    private Vector2 velocity;

    private bool playerIsVisible;

    private bool triggeredWalking, inCooldown, anPlayed, walking, inCd;

    private Random random;

    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        visBox = transform.GetChild(0).GetComponent<BoxCollider2D>();
        hitBox = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        velocity = new Vector2(1, 0);
        inCooldown = false;
        triggeredWalking = false;
        inCd = false;
        anPlayed = false;
        if (dir == Direction.Left)
            transform.localScale = new Vector2(-1, 1);
        random = new Random();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player>() == null && other.gameObject.GetComponent<Sword>() == null)
        {
            if (dir == Direction.Left)
            {
                dir = Direction.Right;
                transform.localScale = new Vector2(1, 1);
            }
            else
            {
                dir = Direction.Left;
                transform.localScale = new Vector2(-1, 1);
            }
        }
        if (other.gameObject.GetComponent<Player>() != null)
        {
            other.gameObject.GetComponent<Player>().SendMessage("GetDamage", playerDamage);
            if (!inCooldown)
            {
                animator.Play("Fox - Attack " + (int)Random.Range(1, 3));
                triggeredWalking = false;
                StartCoroutine(MovementCooldown());
            }
        }
        else if (other.gameObject.GetComponent<Sword>() != null)
        {
            if (other.transform.rotation.z != 0)
            {
                health -= 5;
            }

        }
    }


    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            if (!anPlayed)
            {
                Debug.Log("death");
                animator.Play("Fox - Death");
                StartCoroutine(DeathCountdown());
                walking = false;
                anPlayed = true;
            }
        }
        if (dir == Direction.Left)
        {
            velocity.x = -1;
        }
        else
        {
            velocity.x = 1;
        }

        if (!walking || !triggeredWalking)
            velocity.x = 0;

        transform.Translate(velocity * Time.deltaTime);

        if (!inCd)
        {
            StartCoroutine(WalkingCountdown());
        }

    }

    private void TriggerWalking()
    {
        animator.Play("Fox - Walking");
        triggeredWalking = true;
    }

    private IEnumerator MovementCooldown()
    {
        inCooldown = true;
        float duration = 2f;
        float normalizedTime = 0;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
        inCooldown = false;
        TriggerWalking();

    }

    private IEnumerator WalkingCountdown()
    {
        inCd = true;
        float duration = 2f;
        if (walking)
            duration = duration * 3;
        float normalizedTime = 0;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
        walking = !walking;
        if (walking)
        {
            TriggerWalking();
        }
        else
        {
            animator.Play("Fox - Idle");
        }

        inCd = false;
    }

    private IEnumerator DeathCountdown()
    {

        float duration = 1.5f;
        float normalizedTime = 0;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
        Destroy(gameObject);
    }
}