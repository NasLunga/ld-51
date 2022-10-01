using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    public float movementSpeed = 3;
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        playerInput = gameObject.GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
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
    }
}
