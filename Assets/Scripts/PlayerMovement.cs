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
        int y = 0;
        int x = 0;
        if (movement.y > 0) {
            y = 1;
        } else if (movement.y < 0) {
            y = -1;
        } else if (movement.x > 0) {
            x = 1;
        } else if (movement.x < 0) {
            x = -1;
        }
        
        // Only change face when moving
        if (x != 0 || y != 0) {
            facedDirection = new Vector2(x, y);
        }

        // Adjust animator
        int face = 0;
        int rotation = 0;
        if (facedDirection.y == 1) {
            face = 1;
        } else if (facedDirection.y == -1) {
            face = 3;
        } else if (facedDirection.x == 1) {
            face = 2;
        } else if (facedDirection.x == -1) {
            face = 2;
            rotation = 180;
        }
        animator.SetInteger("Face", face);
        transform.rotation = Quaternion.Euler(0, rotation, 0);
    }

    void Move()
    {
        Vector2 movement = playerInput.movementInput;
        movement *= movementSpeed;

        // Only move on one axis at a time
        if (movement.y != 0) {
            movement.x = 0;
        }

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
