using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class EnemyMovementPhysics : MonoBehaviour
{
    [HideInInspector] public Animator _animator;
    [Header("Enemy Movement Settings")]
    public float moveSpeed = 3f;
    public float stopDistance = 1f;
    public float jumpHeight = 1.5f;
    public float timeToJumpApex = .4f;
    [HideInInspector] public Vector2 velocity;
    float gravity;

    private float _jumpVelocity;

    [Header("Patrol Settings")]
    public bool facingRight = true;
    public float patrolRadius = 5f;
    public float visionRadiusX = 2f;
    public float visionRadiusY = 0.5f;
    public bool foundTarget = false;
    private float _anchor;

    public Controller2D controller;

    public Transform target;
    [HideInInspector]
    public bool movementDisabled;

    public bool isDead = false;

    // Use this for initialization
    public virtual void Start()
    {
        _anchor = transform.position.x;
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);

        controller = GetComponent<Controller2D>();
        _animator = GetComponentInChildren<Animator>();

        target = GameObject.FindWithTag("Player").transform;
        _jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        movementDisabled = false;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!movementDisabled)
        {
            if (!foundTarget)
            {
                CheckIfFoundTarget();
                Patrol();
            }
            else
            {
                FollowAndAttack();
            }

            ApplyGravity();
            controller.Move(velocity * Time.deltaTime);
            if (controller.collisions.above || controller.collisions.below)
            {
                velocity.y = 0;
            }

            if (velocity.x > 0)
            {
                _animator.SetBool("isMoving", true);
            }

            if (controller.collisions.left || controller.collisions.right)
            {
                Jump();
                _animator.SetBool("isMoving", false);
            }

//        BlindStrategy();
        }
    }

    public void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
    }

    public void CheckIfFoundTarget()
    {
        foundTarget = (Mathf.Abs(target.position.x - transform.position.x) <= visionRadiusX)
                      && (Mathf.Abs(target.position.y - transform.position.y) <= visionRadiusY);
    }

    public void Patrol()
    {
        float patrolDistance = transform.position.x - _anchor;
//        Debug.Log(patrolDistance);
        if (Mathf.Abs(patrolDistance) < patrolRadius)
        {
            velocity.x = (facingRight ? 1 : -1) * moveSpeed;
        }
        else
        {
            ChangeDirection();
            velocity.x = (facingRight ? 1 : -1) * moveSpeed;
        }
    }

    public void FollowAndAttack()
    {
        float distance = transform.position.x - target.position.x;
        if (Mathf.Abs(distance) > stopDistance)
        {
            _animator.SetBool("isMoving", true);
            _animator.SetInteger("isAttacking", 0);
            if (Mathf.Abs(target.position.y - transform.position.y) <= visionRadiusY)
            {
                if (Mathf.Sign(distance) < 0)
                {
                    velocity.x = moveSpeed;
                    facingRight = true;
                }
                else
                {
                    facingRight = false;
                    velocity.x = -moveSpeed;
                }
            }
        }
        else if (Mathf.Abs(target.position.y - transform.position.y) <= visionRadiusY)
        {
            velocity.x = 0;
//            _animator.SetBool("isMoving", false);
//            _animator.SetInteger("isAttacking", Random.Range(1, 3));
        }
    }

    public void BlindStrategy()
    {
        if (controller.collisions.right || controller.collisions.left)
        {
            ChangeDirection();
        }
    }

    public void Jump()
    {
        if (controller.collisions.below)
        {
            velocity.y = _jumpVelocity;
        }
    }

    public void ChangeDirection()
    {
        facingRight = !facingRight;
    }

    public void DisableMovement()
    {
        movementDisabled = true;
    }

    public void EnableMovement()
    {
        movementDisabled = false;
    }
}