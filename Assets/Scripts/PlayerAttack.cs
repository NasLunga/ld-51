using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    private BoxCollider2D coll;
    private Weapon weapon;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = gameObject.GetComponent<PlayerInput>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        coll = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        InitiateAttack();
    }

    public void SetWeapon(Weapon w) {
        if (weapon != null) {
            GameObject.Destroy(weapon.gameObject);
        }
        weapon = Instantiate(w, gameObject.transform);
    }


    void InitiateAttack()
    {
        // Check if weapon is on cooldown
        if (!weapon.canAttack) {
            return;
        }

        if (playerInput.attackInput && weapon.GetType() == typeof(MeleeWeapon)) {
            weapon.Attack();
        } else if (playerInput.rangeAttackInput && weapon.GetType() == typeof(RangedWeapon)) {
            weapon.Attack();
        }
    }   
}
