using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private float originalScaleX;

    private PlayerState playerState;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        playerState = GetComponent<PlayerState>();
        originalScaleX = transform.localScale.x;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        if (movement.x > 0.1f)
            transform.localScale = new Vector3(Mathf.Abs(originalScaleX), transform.localScale.y, 1);
        else if (movement.x < -0.1f)
            transform.localScale = new Vector3(-Mathf.Abs(originalScaleX), transform.localScale.y, 1);

        if(playerState != null)
        {
            playerState.SetWalking(movement.sqrMagnitude > 0);
        }
    }

    void FixedUpdate()
    {

        if(playerState == null || (!playerState.IsHurt && !playerState.IsAttacking))
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
