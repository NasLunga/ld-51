using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int hp {get; private set;} = 50;
    public float spawnDuration = 3f;

    // Start is called before the first frame update
    void Start()
    {
        float alphaIncrease = 1 /  (spawnDuration * 100f);
        StartCoroutine(Spawn(alphaIncrease));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Spawn(float alphaIncrease)
    {
        for (float alpha = 0; alpha <= 255; alpha += alphaIncrease) {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, System.Math.Min(alpha, 255));
            yield return new WaitForSeconds(0.01f);
        }
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
    
}
