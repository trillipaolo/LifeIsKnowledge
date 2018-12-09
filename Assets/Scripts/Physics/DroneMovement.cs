using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : EnemyMovementPhysics
{
    [Header("Drone Settings")] public float verticalMoveSpeed = 1f;
    public float chaseSpeed = 4.5f;
    public float attackSpeed = 6f;
    public float chaseDistance;
    public float attackDistance;
    public float stuckCountdown;
    public float flyingHeight = 2f;
    public float droneSliding = 1f;

    public float timeToDisable;
    private EnemyAttackPaolo attackScript;
    private bool isAttacking;
    private float _attackStartPosition;
    private float time;
    private bool dead;


    private void Start()
    {
        base.Start();

        attackScript = GetComponentInChildren<EnemyAttackPaolo>();
        time = stuckCountdown;
    }


    void Update()
    {
        if (!movementDisabled)
        {
            if (!dead)
            {
                //TODO: Losing the target
                if (!foundTarget)
                {
                    CheckIfFoundTarget();
                    Flying();
                    Patrol();
                }
                else if (!isAttacking)
                {
                    Flying();
                    Chase();
                }
                else
                {
                    Debug.Log(Math.Abs(target.position.y - transform.position.y) + " " +
                              (Math.Abs(target.position.y - transform.position.y) <= visionRadiusY));
                    if (!(controller.collisions.right || controller.collisions.left))
                    {
                        if (Mathf.Abs(transform.position.x - _attackStartPosition) < attackDistance)
                        {
                            Attack();
                        }
                        // Attack distance increased
                        else
                        {
                            isAttacking = false;
                            _animator.SetBool("Attack", false);
                            attackScript.DeactivateAttackCollider();
                        }
                    }
                    // Drone is stuck
                    else
                    {
                        if (time > 0)
                        {
                            time -= Time.deltaTime;
                        }
                        else
                        {
                            isAttacking = false;
                            _animator.SetBool("Attack", false);
                            _attackStartPosition = transform.position.x; // ?
                            attackScript.DeactivateAttackCollider();
                            time = stuckCountdown;
                        }
                    }
                }

                controller.Move(velocity * Time.deltaTime);
                if (controller.collisions.above || controller.collisions.below)
                {
                    velocity.y = 0;
                }
            }
            else
            {
                ApplyGravity();
                if(velocity.x>=droneSliding)
                {
                    velocity.x -= droneSliding;
                }
                else
                {
                    velocity.x = 0;
                }
                controller.Move(velocity * Time.deltaTime);
                if (controller.collisions.above || controller.collisions.below)
                {
                    velocity.y = 0;
                }
            }
        }
    }

    void Flying()
    {
        Vector2 rayOrigin = new Vector2(transform.position.x, controller.collider.bounds.min.y);
        RaycastHit2D flyingHeightRaycastHit2D =
            Physics2D.Raycast(rayOrigin, Vector2.down, flyingHeight, controller.collisionMask);

        Debug.DrawRay(rayOrigin, Vector2.down * flyingHeight, Color.blue);
        if (flyingHeightRaycastHit2D)
        {
            velocity.y = verticalMoveSpeed;
        }
        else
        {
            velocity.y = -verticalMoveSpeed * 2;
            // ApplyGravity()
        }
    }

    void Chase()
    {
        float distance = transform.position.x - target.position.x;
        if (Mathf.Abs(distance) > stopDistance)
        {
            if (Mathf.Abs(target.position.y - transform.position.y) <= chaseDistance)
            {
                if (Mathf.Sign(distance) < 0)
                {
                    velocity.x = chaseSpeed;
                    facingRight = true;
                }
                else
                {
                    facingRight = false;
                    velocity.x = -chaseSpeed;
                }
            }
        }
        else if (Math.Abs(target.position.y - transform.position.y) <= visionRadiusY)
            // Going to attack
        {
            if (Mathf.Sign(distance) < 0)
            {
                velocity.x = chaseSpeed;
                facingRight = true;
            }
            else
            {
                facingRight = false;
                velocity.x = -chaseSpeed;
            }

            isAttacking = true;
            _animator.SetBool("Attack", true);
            _attackStartPosition = transform.position.x;
            attackScript.ActivateAttackCollider();
        }
    }

    void Attack()
    {
        velocity.y = 0;
        velocity.x = (facingRight ? 1 : -1) * attackSpeed;
    }


    public void Dying()
    {
        EnableMovement();
        dead = true;
//        velocity.x = 2;
        attackScript.DeactivateAttackCollider();
//        Invoke("DisableMovement", timeToDisable);
    }
}