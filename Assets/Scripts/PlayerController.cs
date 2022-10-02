using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAttack))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public int maxHp = 100;
    public int hp {get; private set;}

    void Awake()
    {
        hp = maxHp;
    }

    public void DecreaseHp(int loss) {
        maxHp -= loss;
    }
}
