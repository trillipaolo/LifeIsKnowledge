using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : EnemyMovementPhysics
{
	[Header("Drone Settings")]
	public float chaseSpeed = 4.5f;
	public float attackSpeed = 6f;
	public float chaseDistance;
	public float attackDistance;
	
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
				Chase();
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

			//TODO: Overcoming obstacles

//        BlindStrategy();
		}
	}

	void Chase()
	{
		float distance = transform.position.x - target.position.x;
		if (Mathf.Abs(distance) > attackDistance)
		{
//			_animator.SetBool("isMoving", true);
//			_animator.SetInteger("isAttacking", 0);
			if (Mathf.Abs(target.position.y - transform.position.y) <= visionRadiusY)
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
		else if (Mathf.Abs(target.position.y - transform.position.y) <= visionRadiusY)
		{
			velocity.x = 0;
			_animator.SetBool("isMoving", false);
			_animator.SetInteger("isAttacking", Random.Range(1, 3));
		}
	}
}
