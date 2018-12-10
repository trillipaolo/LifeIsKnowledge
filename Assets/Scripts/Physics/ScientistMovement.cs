using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistMovement : EnemyMovementPhysics {

    private Animator _hitboxAnimator;
    private float _lastAttack = -300;
    public float timeBetweenAttacks = 3f;

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
                Move();
            }
        } else {
            _animator.SetTrigger("Dead");
            _hitboxAnimator.SetTrigger("Dead");
            DisableMovement();
        }
    }

    private void Move() {
        ApplyGravity();
        controller.Move(velocity * Time.deltaTime);
        if (controller.collisions.above || controller.collisions.below) {
            velocity.y = 0;
        }

        if (velocity.x > 0) {
            _animator.SetBool("isMoving",true);
        }

        if (controller.collisions.left || controller.collisions.right) {
            Jump();
            _animator.SetBool("isMoving",false);
        }
    }

    private void ScientistFollowAndAttack() {
        float distance = transform.position.x - target.position.x;

        // if too distant go near joel
        if (Mathf.Abs(distance) > stopDistance) {
            _animator.SetBool("isMoving",true);
            _animator.SetInteger("isAttacking",0);
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
                _animator.SetTrigger("Attack");
                _hitboxAnimator.SetTrigger("Attack");
            }
        }
    }

    private void AttackStart() {
        _lastAttack = Time.time;
    }
}
