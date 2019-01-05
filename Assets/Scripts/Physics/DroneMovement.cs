using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

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
    public float attackVisionRadiusY = 1.5f;

    public float timeRangeMin = 0.5f;
    public float timeRangeMax = 2f;
    private EnemyAttackPaolo attackScript;
    private bool isAttacking;
    private float _attackStartPosition;
    private float time;
    private float randomAttackTime;
    [HideInInspector] public bool dead;

    public string droneFallsSound="DroneFalls";
    bool fallSoundPlayed=false;

    private void Start()
    {
        base.Start();

        attackScript = GetComponentInChildren<EnemyAttackPaolo>();
        time = stuckCountdown;
        base.audioManager = AudioManager.instance;
        randomAttackTime = UnityEngine.Random.Range(timeRangeMin, timeRangeMax);
    }


    void Update()
    {
        if (!movementDisabled)
        {
            if (!dead)
            {
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
                    if (!(controller.collisions.right || controller.collisions.left))
                    {
                        if (Mathf.Abs(transform.position.x - _attackStartPosition) < attackDistance)
                        {
                            // Random waiting before attacking
                            if (randomAttackTime > 0)
                            {
                                randomAttackTime -= Time.deltaTime;
                                
                                if (Mathf.Sign(transform.position.x - target.position.x) < 0)
                                {
                                    facingRight = true;
                                }
                                else
                                {
                                    facingRight = false;
                                }
                            }
                            else
                            {
                                _animator.SetBool("Attack", true);
                                Attack();
                            }
                        }
                        // Attack distance increased
                        else
                        {
                            velocity.x = 0;
                            isAttacking = false;
                            _animator.SetBool("Attack", false);
                            attackScript.DeactivateAttackCollider();
                            randomAttackTime = UnityEngine.Random.Range(timeRangeMin, timeRangeMax);
                        }
                    }
                    // Drone is stuck
                    else
                    {
                        velocity.x = 0;
                        if (time > 0)
                        {
                            _animator.SetBool("Stuck", true);
                            time -= Time.deltaTime;
                            attackScript.DeactivateAttackCollider();
                        }
                        else
                        {
                            _animator.SetBool("Stuck", false);
                            isAttacking = false;
                            _animator.SetBool("Attack", false);
                            _attackStartPosition = transform.position.x; // ?
                            attackScript.DeactivateAttackCollider();
                            time = stuckCountdown;
                            randomAttackTime = UnityEngine.Random.Range(timeRangeMin, timeRangeMax);
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
                if (velocity.x >= droneSliding)
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
                    if(fallSoundPlayed == false) {
                        base.audioManager.Play(droneFallsSound);
                        fallSoundPlayed = true;
                    }
                    
                    velocity.y = 0;//add metal falling
                }
            }
        }
        else
        {
            
            attackScript.DeactivateAttackCollider();
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
            // Here are possible ideas:
//            velocity.y = 0;
//            velocity.y = -verticalMoveSpeed * 2;
            velocity.y = -verticalMoveSpeed;
            // ApplyGravity()
        }
    }

    void Chase()
    {
        float distance = transform.position.x - target.position.x;
        if (Mathf.Abs(distance) > stopDistance)
        {
            if (Mathf.Abs(target.position.x - transform.position.x) <= chaseDistance)
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
            else
            {
                foundTarget = false;
                _animator.SetBool("FoundTarget", false);
                _anchor = transform.position.x;
            }
        }
        else if ((transform.position.y - target.position.y) >= 0 &&
                (transform.position.y - target.position.y) <= attackVisionRadiusY)
            // Going to attack
        {
            isAttacking = true;
            velocity.y = 0;
            velocity.x = 0;
//            _animator.SetBool("Attack", true);
            _attackStartPosition = transform.position.x;
        }
    }

    void Attack()
    {
        velocity.x = (facingRight ? 1 : -1) * attackSpeed;
        attackScript.ActivateAttackCollider();
    }


    public void Dying()
    {
        EnableMovement();
        dead = true;
        attackScript.DeactivateAttackCollider();
    }
}