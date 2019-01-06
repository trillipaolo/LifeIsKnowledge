using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMovement : EnemyMovementPhysics
{
    public float chillTimer = 2f;
	private Animator _armorAnimator;
	
	private Animator _hitboxAnimator;
	private float _lastAttack = -300;
	public float timeBetweenAttacks = 3f;

    private float _countdown;

    // Use this for initialization
    void Start()
    {
        base.Start();
	    _armorAnimator = transform.Find("Armor").GetComponent<Animator>();
	    _hitboxAnimator = transform.Find("AttackCollider").GetComponent<Animator>();
        _countdown = chillTimer;
    }

    // Update is called once per frame
	void Update () {

		if (!isDead)
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

				if (Mathf.Abs(velocity.x) > 0)
				{
					_animator.SetBool("isMoving", true);
					_armorAnimator.SetBool("isMoving", true);
				}

				if (controller.collisions.left || controller.collisions.right)
				{
					Jump();
					_animator.SetBool("isMoving", false);
					_armorAnimator.SetBool("isMoving", false);
				}

//        BlindStrategy();
			}
		}else
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
        else if (!foundTarget)
        {
	        if (Mathf.Abs(patrolDistance) > patrolRadius && (_countdown > 0))
	        {
		        velocity.x = 0;
		        _animator.SetBool("isMoving", false);
		        _armorAnimator.SetBool("isMoving", false);
		        _countdown -= Time.deltaTime;
	        }
	        else
	        {
		        _countdown = chillTimer;
		        ChangeDirection();
		        _animator.SetBool("isMoving", true);
		        _armorAnimator.SetBool("isMoving", true);
		        velocity.x = (facingRight ? 1 : -1) * moveSpeed;
	        }
        }
    }

	public override void FollowAndAttack()
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
			if (Time.timeSinceLevelLoad - _lastAttack > timeBetweenAttacks)
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

				Attack();
			}
		}
	}

	public void Attack()
	{
		
		_animator.SetBool("isMoving", false);
		_armorAnimator.SetBool("isMoving", false);
		_lastAttack = Time.timeSinceLevelLoad;
		_animator.SetBool("Attack",true);
		_armorAnimator.SetBool("Attack",true);
		_hitboxAnimator.SetBool("Attack",true);
	}

	private void FinishAttack()
	{
		
		_animator.SetBool("isMoving", true);
		_armorAnimator.SetBool("isMoving", true);
		_animator.SetBool("Attack",false);
		_armorAnimator.SetBool("Attack",false);
		_hitboxAnimator.SetBool("Attack",true);
	}
	
	private void AttackStart()
	{
	}
}