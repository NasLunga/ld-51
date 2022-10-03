using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class Weapon : MonoBehaviour
{
    public int damage;
    public float reload;
    public float animationDuration;
    public bool canAttack {get; private set;} = true;
    public Animator animator;
    public AudioSource audioSource;

    public abstract void Attack();

    void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Update() {
        UpdateAnimatorDirection();
    }

    public void UpdateAnimatorDirection()
    {
        int face = GameManager.instance.player.GetComponent<Animator>().GetInteger("Face");
        animator.SetInteger("Face", face);
    }

    protected void BeforeAttack()
    {
        animator.SetTrigger("Attack");
    }

    protected void AfterAttack()
    {
        GameManager.instance.player.SendMessage("Stun", animationDuration);
        StartCoroutine(Reload());
        audioSource.Play();
    }

    public IEnumerator Reload()
    {
        canAttack = false;
        yield return new WaitForSeconds(reload);
        canAttack = true;
    }
}
