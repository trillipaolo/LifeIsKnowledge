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

    public string spawnsDroneSound= "ScSpawnsDrone";

    private void Awake()
    {
        _hitboxAnimator = transform.Find("AttackCollider").GetComponent<Animator>();

        base.audioManager = AudioManager.instance;
    }

    public override void Update()
    {
        //TODO: fix attack with the last drone
        //TODO: fix zero velocity stuck
        if (!isDead)
        {
            if (!spawned)
            {
                if (!CheckForSpawn())
                {
                    Patrol();
                }
                else
                {
                    SpawnDrone();
                }
            }
            else if (dronesAmount > 1)
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
            DisableMovement();
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
            Jump();
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
        base.audioManager.Play(spawnsDroneSound);
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
        if(Mathf.Abs(target.position.x - transform.position.x) <= avoidDistance
            && Mathf.Abs(target.position.y - transform.position.y) <= visionRadiusY)
        {
            if(target.position.x - transform.position.x > 0)
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
}