using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAttack))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    public float damageBlinkDuration = 0.1f;
    public float damageAnimationDuration = 1f;
    public int maxHp = 100;
    public int hp {get; private set;}
    public AudioClip takeDamageSound;
    public AudioClip deathSound;
    public AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        hp = maxHp;
        audioSource = gameObject.GetComponent<AudioSource>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void DecreaseHp(int loss)
    {
        hp -= loss;
        if (hp < 0) {
            Die();
        }
        SoundManagerGameplay.instance.PlaySingle(takeDamageSound);
        StartCoroutine(RedBlinkAnimation());
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

    void Die()
    {
        Debug.Log("Death");
        audioSource.clip = deathSound;
        audioSource.Play();
    }
}
