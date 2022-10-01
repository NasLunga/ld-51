using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public Vector2 facedDirection {get; private set;}
    private PlayerInput playerInput;
    public float movementSpeed = 3;
    private Rigidbody2D rb2d;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        playerInput = gameObject.GetComponent<PlayerInput>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AdjustFace();
        Move();
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
        // movement.Normalize();
        movement *= movementSpeed;

        // Only move on one axis at a time
        if (movement.y != 0) {
            movement.x = 0;
        }

        rb2d.velocity = movement;

        // Adjust animator
        if (movement.magnitude > 0) {
            animator.SetBool("Move", true);
        } else {
            animator.SetBool("Move", false);
        }
    }
}
