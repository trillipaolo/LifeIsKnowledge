using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistMovement : EnemyMovementPhysics {

    private Animator _hitboxAnimator;
    private float _lastAttack = -300;
    public float timeBetweenAttacks = 3f;
    public bool canMove = true;
    private float _lastXPos;

    private void Awake() {
        _hitboxAnimator = transform.Find("AttackCollider").GetComponent<Animator>();
    }

    public override void Update() {
        if (!isDead) {
            if (!foundTarget) {
                CheckIfFoundTarget();
                Patrol();
            } else {
                ScientistFollowAndAttack();
            }

            if (!movementDisabled) {
                //if (canMove) {
                //  Move();
                //}
                if (!canMove) {
                    velocity.x = 0;
                }
                Move();
            }
        } else {
            _animator.SetTrigger("Dead");
            _hitboxAnimator.SetTrigger("Dead");
            DisableMovement();
        }

        float xSpeed = Mathf.Abs(_lastXPos - transform.position.x) / (Time.deltaTime * moveSpeed);
        _animator.SetFloat("XSpeed",xSpeed);
        _lastXPos = transform.position.x;
    }

    private void Move() {
        ApplyGravity();
        controller.Move(velocity * Time.deltaTime);
        if (controller.collisions.above || controller.collisions.below) {
            velocity.y = 0;
        }

        if (controller.collisions.left || controller.collisions.right) {
            Jump();
        }
    }

    private void ScientistFollowAndAttack() {
        float distance = transform.position.x - target.position.x;

        // if too distant go near joel
        if (Mathf.Abs(distance) > stopDistance) {
            if (Mathf.Abs(target.position.y - transform.position.y) <= visionRadiusY) {
                if (Mathf.Sign(distance) < 0) {
                    velocity.x = moveSpeed;
                    facingRight = true;
                } else {
                    facingRight = false;
                    velocity.x = -moveSpeed;
                }
            }
        } else if (Mathf.Abs(target.position.y - transform.position.y) <= visionRadiusY) {
            if (Time.time - _lastAttack > timeBetweenAttacks) {
                if (Mathf.Sign(distance) < 0) {
                    velocity.x = moveSpeed;
                    facingRight = true;
                } else {
                    facingRight = false;
                    velocity.x = -moveSpeed;
                }
                _animator.SetTrigger("Attack");
                _hitboxAnimator.SetTrigger("Attack");
            }
        }
    }

    private void AttackStart() {
        _lastAttack = Time.time;
    }
}
