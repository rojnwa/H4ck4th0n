using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : MonoBehaviour
{
    private BoxCollider2D hitBox;

    private BoxCollider2D visBox;

    private Rigidbody2D rb2D;

    [SerializeField]
    private int playerDamage;

    [SerializeField]
    private int health;

    private Animator animator;

    private enum Direction { Left, Right };

    [SerializeField]
    private Direction dir;

    private Vector2 velocity;

    private bool playerIsVisible;

    private bool triggeredWalking, inCooldown, doJumpAttack;

    private Random random;

    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        visBox = transform.GetChild(0).GetComponent<BoxCollider2D>();
        hitBox = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        velocity = new Vector2(1, 0);
        playerIsVisible = false;
        inCooldown = false;
        triggeredWalking = false;
        doJumpAttack = false;
        if (dir == Direction.Left)
            transform.localScale = new Vector2(-1, 1);
        random = new Random();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Fox - Death"))
            {
                animator.Play("Fox - Death");
                StartCoroutine(DeathCountdown());
            }
        }
        else {

            Collider2D[] vHits = Physics2D.OverlapBoxAll(transform.position, visBox.size, 0);
            foreach (Collider2D hit in vHits)
            {

                if (hit.gameObject.GetComponent<Player>() != null)
                {
                    playerIsVisible = true;
                    if (Vector2.Distance(transform.position, hit.gameObject.transform.position) < 4.6 && Vector2.Distance(transform.position, hit.gameObject.transform.position) > 4.5 && Random.value < 0.1)
                    {
                        doJumpAttack = true;
                        Debug.Log("Jump");
                    }
                }
            }

            if (!inCooldown && playerIsVisible && !triggeredWalking)
                TriggerWalking();

            if (doJumpAttack)
            {
                animator.Play("Fox - Attack 2");
                if (dir == Direction.Left)
                {
                    transform.position = new Vector2(transform.position.x - 2, transform.position.y);
                }
                else
                {
                    transform.position = new Vector2(transform.position.x + 2, transform.position.y);
                }


                triggeredWalking = false;
                inCooldown = true;
                doJumpAttack = false;
                StartCoroutine(MovementCooldown());
            }
            else
            if (!inCooldown && playerIsVisible && triggeredWalking && (!animator.GetCurrentAnimatorStateInfo(0).IsName("Fox - Attack 1") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Fox - Attack 2")))
            {

                if (dir == Direction.Left)
                {
                    velocity.x = -1;
                }
                else
                {
                    velocity.x = 1;
                }

                transform.Translate(velocity * Time.deltaTime);

                Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, hitBox.size, 0);

                foreach (Collider2D hit in hits)
                {
                    if (hit == hitBox || hit == visBox)
                        continue;

                    ColliderDistance2D colliderDistance = hit.Distance(hitBox);

                    if (colliderDistance.isOverlapped)
                    {
                        Vector2 v = colliderDistance.pointA - colliderDistance.pointB;

                        if (hit.gameObject.GetComponent<Player>() == null)
                        {
                            if (v.x != 0)
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
                        }
                        else
                        {
                            animator.Play("Fox - Attack 1");
                            triggeredWalking = false;
                            inCooldown = true;
                            StartCoroutine(MovementCooldown());
                        }
                        transform.Translate(v);

                    }
                }
            }
        }
    }

    private void TriggerWalking()
    {
        animator.Play("Fox - Walking");
        triggeredWalking = true;
    }

    private IEnumerator MovementCooldown()
    {

        float duration = 3f;
        float normalizedTime = 0;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
        inCooldown = false;
        TriggerWalking();

    }

    private IEnumerator DeathCountdown()
    {

        float duration = 2f;
        float normalizedTime = 0;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
        Destroy(gameObject);
    }
}
