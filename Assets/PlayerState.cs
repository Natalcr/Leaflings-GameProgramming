using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerState : MonoBehaviour
{
    private Animator anim;

    public bool IsHurt { get; private set; } = false;
    public bool IsAttacking { get; private set; } = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (IsHurt) return;

        if (Input.GetKeyDown(KeyCode.J) && !IsAttacking)
        {
            StartAttack();
        }
    }

    public void SetWalking(bool walking)
    {
        if (!IsHurt && !IsAttacking)
        {
            anim.SetBool("isWalking", walking);
        }
    }


    private void StartAttack()
    {
        IsAttacking = true;
        anim.SetTrigger("Attack");
        anim.SetBool("isAttacking", true);


        Invoke(nameof(EndAttack), 0.5f);
    }

    private void EndAttack()
    {
        IsAttacking = false;
        anim.SetBool("isAttacking", false);
    }

    public void GetHurt()
    {
        Debug.Log("Player is Hurt");

        if (IsHurt) return;

        IsHurt = true;
        IsAttacking = false;

        anim.SetTrigger("Hurt");
        anim.SetBool("isHurt", true);

        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);

        Invoke(nameof(StopHurt), 0.35f);
    }

    private void StopHurt()
    {
        Debug.Log("Player Stop Get Hurt");

        IsHurt = false;
        anim.SetBool("isHurt", false);
    }
}
