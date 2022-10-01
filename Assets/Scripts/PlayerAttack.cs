using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    public int damage;
    public float reload;
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

    public MeleeWeapon(int dmg, float rel, float r)
    {
        isMelee = true;
        damage = dmg;
        reload = rel;
        reach = r;
    }

    public void Attack (GameObject enemy) {
        enemy.SendMessage("DecreaseHp", damage);
    }
}

public class RangeWeapon : Weapon
{
    public GameObject particlePrefab;
    public float particleVelocity;

    public RangeWeapon(int dmg, float rel, float vel, GameObject part)
    {
        damage = dmg;
        reload = rel;
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

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAttack : MonoBehaviour
{
    private PlayerInput playerInput;
    private Vector2 facedDirection;
    private BoxCollider2D coll;
    private Weapon weapon;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = gameObject.GetComponent<PlayerInput>();
        coll = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        AdjustFace();
        InitiateAttack();
    }

    public void SetWeapon(Weapon w) {
        weapon = w;
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
            x = 0;
        }
        
        // Only change face when moving
        if (x != 0 || y != 0) {
            facedDirection = new Vector2(x, y);
        }
    }

    void InitiateAttack()
    {
        // Check if weapon is on cooldown
        if (!weapon.canAttack) {
            return;
        }

        if (playerInput.attackInput && weapon.isMelee) {
            AttackMelee();
            AfterAttack();            
        } else if (playerInput.rangeAttackInput && !weapon.isMelee) {
            AttackRange();
            AfterAttack();
        }
    }

    void AfterAttack()
    {
        weapon.StartCoolDown();
        Invoke("ResetWeaponCoolDown", weapon.reload);
    }

    void AttackMelee() {
        MeleeWeapon sword = (MeleeWeapon) weapon;

        Vector2 rayStart = gameObject.transform.position;
        Vector2 rayDirection = facedDirection;
        RaycastHit2D hitInfo;
        int layerMask = LayerMask.GetMask("Enemies");

        hitInfo = Physics2D.Raycast(rayStart, rayDirection, sword.reach, layerMask);

        if (hitInfo.collider != null) {
            sword.Attack(hitInfo.collider.gameObject);
        }
    }

    void AttackRange() {
        RangeWeapon gun = (RangeWeapon) weapon;

        Vector2 particleStart = (Vector2) gameObject.transform.position + facedDirection;
        Vector2 particleDirection = playerInput.rangeAttackInputDirection - particleStart;
        particleDirection.Normalize();

        gun.Attack(particleStart, particleDirection);
    }

    void ResetWeaponCoolDown() {
        weapon.ResetCoolDown();
    }
}
