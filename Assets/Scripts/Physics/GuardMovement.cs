using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMovement : EnemyMovementPhysics
{
    public float chillTimer = 2f;

    private float _countdown;

    // Use this for initialization
    void Start()
    {
        base.Start();
        _countdown = chillTimer;
    }

    // Update is called once per frame
	void Update () {
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

    public override void Patrol()
    {
        float patrolDistance = transform.position.x - _anchor;
//        Debug.Log(patrolDistance);
        if (Mathf.Abs(patrolDistance) < patrolRadius)
        {
            velocity.x = (facingRight ? 1 : -1) * moveSpeed;
//			Debug.Log(Mathf.Abs(patrolDistance) + " " + patrolRadius);
        }
        else if (Mathf.Abs(patrolDistance) > patrolRadius && (_countdown > 0))
        {
            velocity.x = 0;
            _animator.SetBool("isMoving", false);
            _countdown -= Time.deltaTime;
        }
        else
        {
            _countdown = chillTimer;
            ChangeDirection();
            _animator.SetBool("isMoving", true);
            velocity.x = (facingRight ? 1 : -1) * moveSpeed;
        }
    }
}