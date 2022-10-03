using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float reach = 2f;
    public int damage = 30;
    public float stunDuration = 0.5f;
    public float attackAnimationWait = 0.2f;
    public float attackDelay = 0.1f;
    public AudioClip attackSound;
    private AudioSource audioSource;
    private EnemyController enemyController;
    private EnemyMovement enemyMovement;
    private Animator animator;
    private Vector2 attackDirection;

    void Awake()
    {
        enemyController = gameObject.GetComponent<EnemyController>();
        enemyMovement = gameObject.GetComponent<EnemyMovement>();
        animator = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public IEnumerator Attack()
    {
        enemyMovement.StopMovement();
        StartAnimation();
        animator.speed = 0;
        yield return new WaitForSeconds(attackAnimationWait);
        animator.speed = 1;
        FinishAttack();
        yield return new WaitForSeconds(attackDelay);
        enemyController.SetState(EnemyState.Standby);
    }

    void StartAnimation()
    {
        animator.SetTrigger("Attack");
        attackDirection = GameManager.instance.player.transform.position - gameObject.transform.position;
        attackDirection = Utilities.VectorToSingularDirection(attackDirection);

        enemyMovement.FaceToDirection(attackDirection);
    }

    void FinishAttack()
    {
        audioSource.clip = attackSound;
        audioSource.Play();

        Vector2 playerDirection = GameManager.instance.player.transform.position - gameObject.transform.position;
        float playerDistance = playerDirection.magnitude;
        playerDirection = Utilities.VectorToSingularDirection(playerDirection);

        if (playerDirection == attackDirection && playerDistance < reach) {
            GameManager.instance.player.SendMessage("DecreaseHp", damage);
        }
    }
}
