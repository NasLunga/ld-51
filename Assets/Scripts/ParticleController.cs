using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ParticleController : MonoBehaviour
{
    public float particleSpeed = 1;
    public int damage;
    public string sourceTag;
    public string targetTag;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag(sourceTag) || coll.gameObject.CompareTag("Particle")) {
            return;
        }

        if (coll.gameObject.CompareTag(targetTag)) {
            coll.gameObject.SendMessage("DecreaseHp", damage);
        }
        GameObject.Destroy(gameObject);
    }
}
