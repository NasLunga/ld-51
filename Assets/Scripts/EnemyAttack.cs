using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float reach = 2f;
    public int damage = 30;
    public float attackAnimationDuration = 0.2f;
    public float attackDelay = 0.1f;
    private EnemyController enemyController;
    private EnemyMovement enemyMovement;
    private Vector2 attackDirection;

    void Awake()
    {
        enemyController = gameObject.GetComponent<EnemyController>();
        enemyMovement = gameObject.GetComponent<EnemyMovement>();
    }

    public IEnumerator Attack()
    {
        enemyMovement.StopMovement();
        StartAnimation();
        yield return new WaitForSeconds(attackAnimationDuration);
        FinishAttack();
        yield return new WaitForSeconds(attackDelay);
        enemyController.SetState(EnemyState.Standby);
    }

    void StartAnimation()
    {
        Vector2 hitDirection = gameObject.transform.position - GameManager.instance.player.transform.position;
        hitDirection.Normalize();

        int x = 0;
        int y = 0;
        if (Mathf.Abs(hitDirection.x) > Mathf.Abs(hitDirection.y)) {
            if (hitDirection.x > 0) {
                x = 1;
            } else {
                x = -1;
            }
        } else {
            if (hitDirection.y > 0) {
                y = 1;
            } else {
                y = -1;
            }
        }
        hitDirection = new Vector2(x, y);

        enemyMovement.FaceToDirection(hitDirection);
        attackDirection = hitDirection;
    }

    void FinishAttack()
    {
        
    }
}
