using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 facedDirection {get; private set;}
    public bool canMove = true;
    private PlayerInput playerInput;
    public float movementSpeed = 3;
    private Rigidbody2D rb2d;
    private Animator animator;
    private float stunDuration = 0;

    void Awake()
    {
        StartCoroutine(ManageStunLock());
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        playerInput = gameObject.GetComponent<PlayerInput>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove) {
            AdjustFace();
            Move();
        } else {
            Freeze();
        }
    }

    void AdjustFace()
    {
        Vector2 movement = playerInput.movementInput;
        Vector2 newFacedDirection = new Vector2(0f, 0f);
        int face;
        int rotation = 0;

        if (Mathf.Abs(movement.x) >= Mathf.Abs(movement.y)) {
            face = 2;
            if (movement.x > 0) {
                newFacedDirection.Set(1f, 0f);
            } else {
                newFacedDirection.Set(-1f, 0f);
                rotation = 180;
            }
        } else {
            if (movement.y > 0) {
                face = 1;
                newFacedDirection.Set(0f, 1f);
            } else {
                face = 3;
                newFacedDirection.Set(0f, -1f);
            }
        }
        
        // Only change face when moving
        if (newFacedDirection.magnitude != 0) {
            facedDirection = newFacedDirection;
        }

        // Adjust animator
        animator.SetInteger("Face", face);
        transform.rotation = Quaternion.Euler(0, rotation, 0);
    }

    void Move()
    {
        Vector2 movement = playerInput.movementInput;
        movement.Normalize();
        movement *= movementSpeed;

        // Only move on one axis at a time
        // if (movement.y != 0) {
        //     movement.x = 0;
        // }

        rb2d.velocity = movement;

        // Adjust animator
        if (movement.magnitude > 0) {
            animator.speed = 1;
        } else {
            animator.speed = 0;
        }
    }

    void Freeze() {
        rb2d.velocity = new Vector2(0f, 0f);
    }

    public void Stun(float duration)
    {
        stunDuration += duration;
    }

    IEnumerator ManageStunLock()
    {
        // Sets canMove to false when stunDuration is gt zero
        while (true) {
            if (stunDuration > 0) {
                canMove = false;
            }
            float seconds = stunDuration;
            stunDuration = 0;
            yield return new WaitForSeconds(seconds);
            canMove = true;
        }
    }
}
