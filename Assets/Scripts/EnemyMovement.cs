using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class EnemyMovement : MonoBehaviour
{
    public float movementSpeed = 2f;
    private Rigidbody2D rb2d;
    private Vector2 currentVelocity;
    private Animator animator;
    private Coroutine followObjectTask;
    private Coroutine moveToPointTask;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        currentVelocity = new Vector2(0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.velocity = currentVelocity;
        AdjustFace();
    }

    public void StartMovement(Vector2 direction)
    {
        currentVelocity = direction * movementSpeed;
    }

    public void StopMovement()
    {
        currentVelocity = new Vector2(0f, 0f);
        if (followObjectTask != null) {
            StopCoroutine(followObjectTask);
            followObjectTask = null;
        }
        if (moveToPointTask != null) {
            StopCoroutine(moveToPointTask);
            moveToPointTask = null;
        }
    }

    void AdjustFace()
    {
        if (currentVelocity.magnitude == 0) {
            return;
        }
        
        Vector2 direction = currentVelocity;
        FaceToDirection(direction);
    }

    public void FaceToDirection(Vector2 direction) {
        int face;
        int rotation = 0;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
            face = 2;
            if (direction.x > 0) {
                rotation = 180;
            }
        } else {
            if (direction.y > 0) {
                face = 1;
            } else {
                face = 3;
            }
        }

        animator.SetInteger("Face", face);
        transform.rotation = Quaternion.Euler(0, rotation, 0);
    }

    public void FollowObject(GameObject target)
    {
        followObjectTask = StartCoroutine(FollowObjectTask(target));
    }

    private IEnumerator FollowObjectTask(GameObject target)
    {
        while (true) {
            Vector2 currentPos = gameObject.transform.position;
            Vector2 targetPos = target.transform.position;
            Vector2 direction = targetPos - currentPos;
            direction.Normalize();
            StartMovement(direction);
            yield return null;
        }
    }

    public void MoveToPoint(Vector2 target)
    {
        followObjectTask = StartCoroutine(MoveToPointTask(target));
    }

    public IEnumerator MoveToPointTask(Vector2 target)
    {
        Vector2 currentPos = gameObject.transform.position;
        Vector2 direction = target - currentPos;
        float acceptableOffset = gameObject.GetComponent<Renderer>().bounds.size.y / 2;
        direction.Normalize();
        StartMovement(direction);
        
        float distance = (target - currentPos).magnitude;

        while (distance > acceptableOffset) {
            yield return null;

            // Also break if enemy starts moving away from target
            float newDistance = (target - (Vector2) gameObject.transform.position).magnitude;
            if (newDistance > distance) {
                break;
            }

            distance = newDistance;   
        }

        StopMovement();
    }
}
