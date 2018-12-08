using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : EnemyMovementPhysics
{
    [Header("Drone Settings")] public float chaseSpeed = 4.5f;
    public float attackSpeed = 6f;
    public float chaseDistance;
    public float attackDistance;
    public float stuckCountdown;

    public float timeToDisable;
    private EnemyAttackPaolo attackScript;
    private bool isAttacking;
    private float _attackStartPosition;
    private float time;


    private void Start()
    {
        base.Start();
        
        attackScript = GetComponentInChildren<EnemyAttackPaolo>();
        time = stuckCountdown;
    }

    
    // Update is called once per frame
    void Update()
    {
        String ver = "";
        String hor = "";
        foreach (int i in controller.raysHorizontalArray)
        {
            ver += i;
        }
        foreach (int i in controller.raysVerticalArray)
        {
            hor += i;
        }
        Debug.Log(hor);
        Debug.Log(ver);
        if (!movementDisabled)
        {
            //TODO: Losing the target
            if (!foundTarget)
            {
                CheckIfFoundTarget();
                Patrol();
            }
            else if (!isAttacking)
            {
                Chase();
            }
            else
            {
//                Debug.Log("distance " + Mathf.Abs(transform.position.x - _attackStartPosition));
//                Debug.Log("attack " + attackDistance);
                if (!(controller.collisions.right || controller.collisions.left))
                {
                    if (Mathf.Abs(transform.position.x - _attackStartPosition) < attackDistance)
                    {
                        Attack();
                    }
                }
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
                        _attackStartPosition = transform.position.x;
                        attackScript.DeactivateAttackCollider();
                        time = stuckCountdown;
                    }
                }
            }

            // TODO: Different gravity
            ApplyGravity();
            controller.Move(velocity * Time.deltaTime);
            if (controller.collisions.above || controller.collisions.below)
            {
                velocity.y = 0;
            }

            //TODO: Overcoming obstacles

//        BlindStrategy();
        }
    }

    void Chase()
    {
        float distance = transform.position.x - target.position.x;
        if (Mathf.Abs(distance) > stopDistance)
        {
//			_animator.SetBool("isMoving", true);
//			_animator.SetInteger("isAttacking", 0);


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
        else
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
        velocity.x = (facingRight ? 1 : -1) * attackSpeed;
    }

    public void Dying()
    {
        attackScript.DeactivateAttackCollider();
        EnableMovement();
        Invoke("DisableMovement", timeToDisable);
    }
}