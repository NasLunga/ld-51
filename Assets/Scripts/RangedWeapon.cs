using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    public GameObject particlePrefab;
    public float particleVelocity;

    public override void Attack ()
    {
        BeforeAttack();
        LaunchParticle();
        AfterAttack();
    }

    void LaunchParticle()
    {
        //     RangeWeapon gun = (RangeWeapon) weapon;
        GameObject player = GameManager.instance.player;
        Vector2 particleStart = (Vector2) player.transform.position + player.GetComponent<PlayerMovement>().facedDirection;
        Vector2 particleDirection = player.GetComponent<PlayerInput>().rangeAttackInputDirection - particleStart;
        particleDirection.Normalize();


        GameObject particle = GameObject.Instantiate(particlePrefab, particleStart, Quaternion.identity);
        ParticleController particleController = particle.GetComponent<ParticleController>();
        particleController.damage = damage;
        particleController.sourceTag = "Player";
        particleController.targetTag = "Enemy";
        particle.GetComponent<Rigidbody2D>().velocity = particleDirection * particleVelocity;
    }
}
