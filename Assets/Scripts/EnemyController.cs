using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyAttack))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class EnemyController : MonoBehaviour
{
    public float damageBlinkDuration = 0.1f;
    public float damageAnimationDuration = 0.5f;
    public int maxHp = 1000;
    public int hp {get; private set;}
    public float spawnVelocity = 3f;
    public GameObject spawnPoint;
    public AudioClip spawnSound;
    public AudioClip takeDamangeSound;
    public AudioClip deathSound;
    public EnemyState state {get; private set;}
    private AudioSource audioSource;
    private EnemyMovement enemyMovement;
    private EnemyAttack enemyAttack;
    public HPBarController hpBar;
    private SpriteRenderer spriteRenderer;

    void Awake() {
        hp = maxHp;
        enemyMovement = gameObject.GetComponent<EnemyMovement>();
        enemyAttack = gameObject.GetComponent<EnemyAttack>();
        audioSource = gameObject.GetComponent<AudioSource>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        hpBar.setPercents(1f);
    }

    void InitiateSpawn() {
        StartCoroutine(Spawn());
        state = EnemyState.Spawning;
    }

    void Update() {
        DecideAction();
    }

    IEnumerator Spawn()
    {
        SoundManagerGameplay.instance.PlaySingle(spawnSound);

        gameObject.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        enemyMovement.StartMovement(new Vector2(0f, spawnVelocity));
        
        float height = gameObject.GetComponent<SpriteRenderer>().bounds.size.y * 0.25f;

        while (gameObject.transform.position.y < spawnPoint.transform.position.y + height) {
            enemyMovement.FaceToDirection(new Vector2(0f, -1f));
            yield return null;
        }
        enemyMovement.StopMovement();
        gameObject.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
        GameObject.Destroy(spawnPoint);
        SetState(EnemyState.Standby);
        GameManager.instance.SetState(GameState.Battle);
    }

    public void SetState(EnemyState newState) {
        state = newState;
    }

    void DecreaseHp(int loss)
    {
        hp -= loss;
        hpBar.setPercents((float) hp / (float) maxHp);

        SoundManagerGameplay.instance.PlaySingle(takeDamangeSound);
        StartCoroutine(RedBlinkAnimation());

        if (hp < 0) {
            Die();
        }
    }

    public IEnumerator RedBlinkAnimation() {
        float totalTimeElapsed = 0;
        bool red = true;
        while (totalTimeElapsed <= damageAnimationDuration) {
            if (red) {
                spriteRenderer.color = new Color(1, 0, 0);
            } else {
                spriteRenderer.color = new Color(1, 1, 1);
            }
            yield return new WaitForSeconds(damageBlinkDuration);
            totalTimeElapsed += damageBlinkDuration;
            red = !red;
        }
        spriteRenderer.color = new Color(1, 1, 1);
    }

    void DecideAction()
    {
        if (state != EnemyState.AttackingPlayer) {
            DecideAttack();
        }

        if (state == EnemyState.Standby) {
            DecideMovement();
        }
    }

    void DecideAttack()
    {
        Vector3 pos = gameObject.transform.position;
        GameObject player = GameManager.instance.player;
        PlayerController pc = player.GetComponent<PlayerController>();
        float distanceToPlayer = (player.transform.position - pos).magnitude;
        
        if (distanceToPlayer < enemyAttack.reach * 0.9) {
            bool shouldAttack = false;
            // If player is immobile in front of enemy, attack
            if (!player.GetComponent<PlayerMovement>().canMove) {
                shouldAttack = true;
            }

            // If player has range weapon, attack
            if (GameManager.instance.weaponState == WeaponState.RangedWeapon) {
                shouldAttack = true;
            }

            // If player is low hp, attack
            pc = player.GetComponent<PlayerController>();
            if (pc.hp < pc.maxHp * 0.5f) {
                shouldAttack = true;
            }

            // If enemy is high hp, attack
            if (hp > maxHp * 0.5f) {
                shouldAttack = true;
            }

            if (shouldAttack) {
                SetState(EnemyState.AttackingPlayer);
                StartCoroutine(enemyAttack.Attack());
                return;
            }
        }
    }

    void DecideMovement() {
        Debug.Log("Deciding movement");
        Vector3 pos = gameObject.transform.position;
        GameObject player = GameManager.instance.player;
        PlayerController pc = player.GetComponent<PlayerController>();
        float distanceToCenter = pos.magnitude;
        float distanceToPlayer = (player.transform.position - pos).magnitude;

        if (hp > maxHp / 2f) {
            // If hp is high, rush attack
            SetState(EnemyState.MovingToPlayer);
            enemyMovement.FollowObject(player);
            Debug.Log("Moving to player (high hp)");
        } else if (GameManager.instance.weaponState == WeaponState.RangedWeapon) {
            // If player has ranged weapon, close distance
            SetState(EnemyState.MovingToPlayer);
            enemyMovement.FollowObject(player);
            Debug.Log("Moving to player (ranged weapon)");
        } else if (pc.hp < pc.maxHp * 0.5f) {
            // If player has hp is low, rush attack
            SetState(EnemyState.MovingToPlayer);
            enemyMovement.FollowObject(player);
            Debug.Log("Moving to player (player low hp)");
        } else if (distanceToPlayer < GameManager.instance.meleeWeapon.reach * 3f) {
            // If enemy is outside of player's reach, circle around player
            Vector2 directionToPlayer = player.transform.position - pos;
            directionToPlayer.Normalize();
            
            bool negateX = Random.value > 0.5f;
            Vector2 perpendicular;
            if (negateX) {
                perpendicular = new Vector2(directionToPlayer.y, -directionToPlayer.x);
            } else {
                perpendicular = new Vector2(-directionToPlayer.y, directionToPlayer.x);
            }

            SetState(EnemyState.MovingToPoint);
            enemyMovement.MoveToPoint(perpendicular * 3f);
            Debug.Log("Moving perpendicularly");
        } else {
            // Default option
            SetState(EnemyState.MovingToPlayer);
            enemyMovement.FollowObject(player);
            Debug.Log("Moving to player (default)");
        }
    }

    void Die()
    {
        GameManager.instance.SetState(GameState.BattleEnded);
        SoundManagerGameplay.instance.PlaySingle(deathSound);
        GameManager.instance.OpenDoors();
        gameObject.SetActive(false);
    }
}


public enum EnemyState {
    Spawning,
    Standby,
    MovingToPoint,
    MovingToPlayer,
    AttackingPlayer
}
