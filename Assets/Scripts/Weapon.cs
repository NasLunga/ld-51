using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public int damage;
    public float reload;
    public float animationDuration;
    public bool canAttack {get; private set;} = true;
    public Animator animator;

    public abstract void Attack();

    void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    protected void BeforeAttack()
    {
        animator.SetTrigger("Attack");
    }

    protected void AfterAttack()
    {
        GameManager.instance.player.SendMessage("Stun", animationDuration);
        StartCoroutine(Reload());
    }

    public IEnumerator Reload()
    {
        canAttack = false;
        yield return new WaitForSeconds(reload);
        canAttack = true;
    }
}
