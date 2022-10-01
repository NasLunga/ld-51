using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 movementInput {get; private set;}
    public bool attackInput {get; private set;}
    public bool rangeAttackInput {get; private set;}
    public Vector2 rangeAttackInputDirection {get; private set;}

    // Update is called once per frame
    void Update()
    {
        float xMovement = Input.GetAxis("Horizontal");
        float yMovement = Input.GetAxis("Vertical");
        movementInput = new Vector2(xMovement, yMovement);

        attackInput = Input.GetKeyDown(KeyCode.Space);

        rangeAttackInput = Input.GetMouseButtonDown(0);
        if (rangeAttackInput) {
            rangeAttackInputDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        } else {
            rangeAttackInputDirection = new Vector2(0f, 0f);
        }
    }
}
