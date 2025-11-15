using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Skeleton : MonoBehaviour
{
    [Header("AI Settings")]
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    public float attackRange = 1f;
    public int damage = 1;

    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;

    private Vector2 direction;
    private float originalScaleX;

    private bool isDead = false;
    private bool isHit = false;
    private bool isAttacking = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        originalScaleX = transform.localScale.x;
    }

    void Update()
    {
        if (isDead) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (isHit || isAttacking)
        {
            direction = Vector2.zero;
            return;
        }

        if (distance <= attackRange)
        {
            StartAttack();
        }
        else if (distance <= detectionRange)
        {
            WalkToPlayer();
        }
        else
        {
            SetIdle();
        }
    }

    void FixedUpdate()
    {
        if (!isDead && !isHit && !isAttacking && anim.GetBool("isWalk"))
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void SetIdle()
    {
        anim.SetBool("isIdle", true);
        anim.SetBool("isWalk", false);
        direction = Vector2.zero;
    }

    void WalkToPlayer()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isWalk", true);

        direction = (player.position - transform.position).normalized;

        if (direction.x > 0.1f)
            transform.localScale = new Vector3(originalScaleX, transform.localScale.y, 1);
        else if (direction.x < -0.1f)
            transform.localScale = new Vector3(-originalScaleX, transform.localScale.y, 1);
    }

    void StartAttack()
    {
        if (isAttacking) return;

        isAttacking = true;
        direction = Vector2.zero;

        anim.SetBool("isAttack", true);
        anim.SetBool("isWalk", false);
        anim.SetBool("isIdle", false);

        StartCoroutine(DealDamageToPlayer());

        StartCoroutine(StopAttack());
    }

    IEnumerator StopAttack()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("isAttack", false);
        isAttacking = false;
    }

    IEnumerator DealDamageToPlayer()
    {
        yield return new WaitForSeconds(0.25f);
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            PlayerState playerState = player.GetComponent<PlayerState>();
            if (playerState != null)
            {
                playerState.GetHurt();
            }
        }
    }

    public void TakeHit()
    {
        if (isDead || isHit) return;

        isHit = true;
        anim.SetBool("isHit", true);
        anim.SetBool("isWalk", false);

        StartCoroutine(StopHit());
    }

    IEnumerator StopHit()
    {
        yield return new WaitForSeconds(0.3f);
        isHit = false;
        anim.SetBool("isHit", false);
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        anim.SetBool("isDead", true);
        anim.SetBool("isWalk", false);
        anim.SetBool("isAttack", false);
        
        direction = Vector2.zero;
    }
}
