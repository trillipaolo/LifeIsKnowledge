using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistMovement : EnemyMovementPhysics
{
    private Animator _hitboxAnimator;
    private float _lastAttack = -300;
    public float timeBetweenAttacks = 3f;
    public float spawnDistance = 10f;
    public float avoidDistance = 7f;
    public bool canMove = true;
    private float _lastXPos;
    public GameObject drone;
    private GameObject currentDrone;
    public int dronesAmount = 3;
    private bool spawned;
    public static int jumpAttempts = 3;
    private int jumpAttemptCounter;

    private void Awake()
    {
        _hitboxAnimator = transform.Find("AttackCollider").GetComponent<Animator>();
        jumpAttemptCounter = jumpAttempts;
    }

    public override void Update()
    {
        //TODO: fix zero velocity stuck
        if (!isDead)
        {
            if (!spawned && dronesAmount != 0)
            {
                if (!CheckForSpawn())
                {
                    Patrol();
                }
                else if (dronesAmount > 0)
                {
                    SpawnDrone();
                }
            }
            else if (dronesAmount > 0)
            {
                AvoidPlayer();
                if (currentDrone.GetComponent<DroneMovement>().dead)
                {
                    dronesAmount -= 1;
                    spawned = false;
                }
            }
            else if (!foundTarget)
            {
                CheckIfFoundTarget();
                Patrol();
            }
            else
            {
                ScientistFollowAndAttack();
            }

            if (!movementDisabled)
            {
                //if (canMove) {
                //  Move();
                //}
                if (!canMove)
                {
                    velocity.x = 0;
                }

                Move();
            }
        }
        else
        {
            _animator.SetTrigger("Dead");
            _hitboxAnimator.SetTrigger("Dead");
            ApplyGravity();
            velocity.x = 0;
            controller.Move(velocity * Time.deltaTime);
            if (controller.collisions.above || controller.collisions.below)
            {
                velocity.y = 0;
            }

            currentDrone.GetComponent<DroneBehaviour>().Die();
        }

        float xSpeed = Mathf.Abs(_lastXPos - transform.position.x) / (Time.deltaTime * moveSpeed);
        _animator.SetFloat("XSpeed", xSpeed);
        _lastXPos = transform.position.x;
    }

    private void Move()
    {
        ApplyGravity();
        controller.Move(velocity * Time.deltaTime);
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        if (controller.collisions.left || controller.collisions.right)
        {
            JumpOrChangeDirection();
        }
    }

    private void ScientistFollowAndAttack()
    {
        float distance = transform.position.x - target.position.x;

        // if too distant go near joel
        if (Mathf.Abs(distance) > stopDistance)
        {
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
            if (Time.time - _lastAttack > timeBetweenAttacks)
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

                _animator.SetTrigger("Attack");
                _hitboxAnimator.SetTrigger("Attack");
            }
        }
    }

    private void AttackStart()
    {
        _lastAttack = Time.time;
    }

    public bool CheckForSpawn()
    {
        return (Mathf.Abs(target.position.x - transform.position.x) <= spawnDistance)
               && (Mathf.Abs(target.position.y - transform.position.y) <= visionRadiusY);
    }

    public void SpawnDrone()
    {
        _animator.SetBool("Spawn", true);
        spawned = true;
        currentDrone = Instantiate(drone, transform.position, Quaternion.identity);
        currentDrone.GetComponent<EnemyMovementPhysics>().foundTarget = true;
        // TODO: ???
//        Invoke("", 2f);
        _animator.SetBool("Spawn", false);
    }

    public void AvoidPlayer()
    {
        if (Mathf.Abs(target.position.x - transform.position.x) <= avoidDistance
            && Mathf.Abs(target.position.y - transform.position.y) <= visionRadiusY)
        {
            if (target.position.x - transform.position.x > 0)
            {
                velocity.x = -moveSpeed;
            }
            else
            {
                velocity.x = moveSpeed;
            }
        }
        else
        {
            velocity.x = 0f;
        }
    }

    public void JumpOrChangeDirection()
    {
        Vector2 rayOrigin;
        Vector2 direction;
        if (facingRight)
        {
            rayOrigin = controller.raycastOrigins.topRight;
            direction = Vector2.right;
        }
        else
        {
            rayOrigin = controller.raycastOrigins.topLeft;
            direction = Vector2.left;
        }

        RaycastHit2D inFrontRaycastHit2D =
            Physics2D.Raycast(rayOrigin, direction, 0.1f, controller.collisionMask);

        Debug.DrawRay(rayOrigin, direction * 0.1f, Color.blue);
        if (inFrontRaycastHit2D)
        {
            ChangeDirection();
            velocity.x = (facingRight ? 1 : -1) * moveSpeed;
        }
        else
        {
            Jump();
        }
    }
}