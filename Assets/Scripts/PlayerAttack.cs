using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAttack : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    private BoxCollider2D coll;
    private Weapon weapon;

    // Start is called before the first frame update
    void Start()
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
        weapon = w;
    }


    void InitiateAttack()
    {
        // Check if weapon is on cooldown
        if (!weapon.canAttack) {
            return;
        }

        if (playerInput.attackInput && weapon.isMelee) {
            AttackMelee();
        } else if (playerInput.rangeAttackInput && !weapon.isMelee) {
            AttackRange();
        } else {
            return;
        }

        playerMovement.Stun(weapon.animationDuration);
        StartCoroutine(ReloadWeapon());
    }

    void AttackMelee() {
        MeleeWeapon sword = (MeleeWeapon) weapon;

        Vector2 rayStart = gameObject.transform.position;
        Vector2 rayDirection = playerMovement.facedDirection;

        sword.Attack(rayStart, rayDirection);
    }

    void AttackRange() {
        RangeWeapon gun = (RangeWeapon) weapon;

        Vector2 particleStart = (Vector2) gameObject.transform.position + playerMovement.facedDirection;
        Vector2 particleDirection = playerInput.rangeAttackInputDirection - particleStart;
        particleDirection.Normalize();

        gun.Attack(particleStart, particleDirection);
    }

    IEnumerator ReloadWeapon() {
        weapon.StartCoolDown();
        yield return new WaitForSeconds(weapon.reload);
        weapon.ResetCoolDown();
    }
}


public abstract class Weapon
{
    public int damage;
    public float reload;
    public float animationDuration;
    public bool isMelee;
    public bool canAttack = true;
    private PlayerAttack holderPlayer;

    public void StartCoolDown() {
        canAttack = false;
    }

    public void ResetCoolDown() {
        canAttack = true;
    }
}

public class MeleeWeapon : Weapon
{
    public float reach;

    public MeleeWeapon(int dmg, float rel, float animation, float r)
    {
        damage = dmg;
        animationDuration = animation;
        reload = rel;
        isMelee = true;
        reach = r;
    }

    public void Attack (Vector2 rayStart, Vector2 rayDirection) {
        RaycastHit2D hitInfo;
        int layerMask = LayerMask.GetMask("Enemies");

        hitInfo = Physics2D.Raycast(rayStart, rayDirection, reach, layerMask);

        if (hitInfo.collider != null) {
            hitInfo.collider.gameObject.SendMessage("DecreaseHp", damage);
        }
    }
}

public class RangeWeapon : Weapon
{
    public GameObject particlePrefab;
    public float particleVelocity;

    public RangeWeapon(int dmg, float rel, float animation, float vel, GameObject part)
    {
        damage = dmg;
        reload = rel;
        animationDuration = animation;
        isMelee = false;
        particleVelocity = vel;
        particlePrefab = part;
    }

    public void Attack (Vector2 start, Vector2 direction) {
        GameObject particle = GameObject.Instantiate(particlePrefab, start, Quaternion.identity);
        ParticleController particleController = particle.GetComponent<ParticleController>();
        particleController.damage = damage;
        particleController.sourceTag = "Player";
        particleController.targetTag = "Enemy";
        particle.GetComponent<Rigidbody2D>().velocity = direction * particleVelocity;
    }
}
