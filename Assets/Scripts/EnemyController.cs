using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class EnemyController : MonoBehaviour
{
    public int hp {get; private set;} = 50;
    
    public float spawnDuration = 3f;
    public EnemyState state;
    private EnemyMovement enemyMovement;

    void Awake() {
        enemyMovement = gameObject.GetComponent<EnemyMovement>();
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
    }

    void InitiateSpawn() {
        float alphaIncrease = 1 /  (spawnDuration * 100f);
        StartCoroutine(Spawn(alphaIncrease));
        state = EnemyState.Standby;
    }

    IEnumerator Spawn(float alphaIncrease)
    {
        for (float alpha = 0; alpha <= 1f; alpha += alphaIncrease) {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, System.Math.Min(alpha, 255));
            yield return new WaitForSeconds(0.01f);
        }
        GameManager.instance.SetState(GameState.Battle);
        StartCoroutine(enemyMovement.MoveToPoint(GameManager.instance.player.transform.position));
        StartCoroutine(enemyMovement.FollowObject(GameManager.instance.player));
    }

    void DecreaseHp(int loss)
    {
        hp -= loss;
        if (hp < 0) {
            Die();
        }
    }

    void Die()
    {
        gameObject.SetActive(false);
    }
}


public enum EnemyState {
    Standby,
    MovingToPoint,
    MovingToPlayer,
    AttackingPlayer
}
