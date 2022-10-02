using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public float reach;

    public override void Attack ()
    {
        BeforeAttack();
        DealDamage();
        AfterAttack();
    }

    void DealDamage()
    {
        GameObject player = GameManager.instance.player;
        Vector2 rayStart = player.transform.position;
        Vector2 rayDirection = player.GetComponent<PlayerMovement>().facedDirection;

        RaycastHit2D hitInfo;
        int layerMask = LayerMask.GetMask("Enemies");

        hitInfo = Physics2D.Raycast(rayStart, rayDirection, reach, layerMask);

        if (hitInfo.collider != null) {
            hitInfo.collider.gameObject.SendMessage("DecreaseHp", damage);
        }
    }
}
