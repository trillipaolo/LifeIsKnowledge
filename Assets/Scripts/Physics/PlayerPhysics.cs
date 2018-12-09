using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/***
 * This class is responsible for handling player's input.
 * It requires Controller2D class which calculates movement depending on velocity vector.
 * 
 * Link to the original tutorial:
 * https://www.youtube.com/watch?v=MbWK8bCAU2w&list=PLFt_AvWsXl0f0hqURlhyIoAabKPgRsqjz&index=1
 */

[RequireComponent(typeof(Controller2D))]
public class PlayerPhysics : MonoBehaviour
{
    [Header("Movement settings")] public float moveSpeed = 10;
    float accelerationTimeGrounded = .1f;
    [Header("Jump setting")] public float maxJumpHeight = 4f;
    public float minJumpHeight = 1f;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector2 velocity;
    float velocityXSmoothing;

    Controller2D controller;
    private Animator _animator;
    Vector2 directionalInput;
    private bool _rolling;
    [Header("Roll settings")] public int maxFrames = 15;
    public float maxDistance = 5.0f;
    public float rollingSpeed = 25f;
    public float rollColdownTime = 3f;
    int currentFrame;
    float currentDistance;

    public GameObject coolDown;
    public Transform canvas;
    private float _nextFireTime = 0;

    private bool _attackMovement = false;
    private float _distance = 0;
    private int _attackMovementFrames = 0;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        currentFrame = 0;
        currentDistance = 0f;
        _rolling = false;
        _animator = GetComponent<Animator>();
        _animator.SetBool("Grounded", true);

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) + minJumpHeight);
        print("Gravity: " + gravity + " Jump velocity: " + maxJumpVelocity);
    }

    void Update()
    {
        
        velocity.y += gravity * Time.deltaTime;
        if (!_attackMovement)
        {
            CalculateVelocity();

            _animator.SetFloat("XSpeed", Mathf.Abs(velocity.x));

            
        }
        else
        {
            if (_attackMovementFrames > 0)
            {
                velocity.x = ((controller.facingRight) ? 1 : -1) * (_distance / _attackMovementFrames);
                _attackMovementFrames -= 1;
            }
            
            currentDistance = 0f;
            _rolling = false;
            _animator.SetBool("Roll", false);
        }
        
        controller.Move(velocity * Time.deltaTime);
        //TODO: deal with it
        if (_attackMovement)
        {
            velocity.x = 0;
        }
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        if (controller.collisions.below)
        {
            _animator.SetBool("Grounded", true);
        }
        else
        {
            _animator.SetBool("Grounded", false);
        }

    }

    public void SetDirectionalInput(Vector2 input)
    {
        //
        directionalInput = input;
    }

    public void OnJumpInputDown()
    {
        if (controller.collisions.below)
        {
            velocity.y = maxJumpVelocity;
        }
    }

    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }


    public void Roll()
    {
        //if (controller.collisions.below) ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ ROLL WHERE YOU WANT! 
        {
            if (Time.time > _nextFireTime)
            {
                _rolling = true;
                _animator.SetBool("Roll", true);
                _animator.SetTrigger("RollTrigger");
                _nextFireTime = Time.time + rollColdownTime;
                GameObject cd = Instantiate(coolDown, new Vector3(0, -170, 0), Quaternion.identity) as GameObject;
                cd.transform.SetParent(canvas, false);
            }
        }
    }

    void CalculateVelocity()
    {
        if (!_rolling)
        {
            // Smoothing the movement depending on whether the player is grounded or not.
            float targetVelocityX = directionalInput.x * moveSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
                (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        }
        else
        {
            if (currentFrame < maxFrames)
            {
                currentFrame++;
            }
            else
            {
                currentFrame = 0;
            }

            if (currentDistance < maxDistance)
            {
                float targetVelocityX = ((controller.facingRight) ? 1 : -1) * rollingSpeed;
                currentDistance += Mathf.Abs(targetVelocityX * Time.deltaTime);
                velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
                    (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            }
            else
            {
                currentDistance = 0f;
                _rolling = false;
                _animator.SetBool("Roll", false);
            }
        }

//        velocity.y += gravity * Time.deltaTime; // Applying gravity to velocity
    }


    // If the player needs to be flipped during attacks he will be flipped (called into animator of joel)
    private void FlipBetweenAttacks()
    {
        // if (input right but facing left) OR (input left but facing right) flip the player
        if ((directionalInput.x > 0 && !controller.facingRight) ||
            directionalInput.x < 0 && controller.facingRight)
        {
            Debug.Log("Hello!" + directionalInput.x + " " + controller.facingRight);
            controller.Flip();
        }
    }

//    private void SwitchAttackMovement()
//    {
//        attackMovement = !attackMovement;
//    }

    private void PerformAttackMovement(AnimationEvent animationEvent)
    {
        _distance = animationEvent.floatParameter;
        _attackMovementFrames = animationEvent.intParameter;
    }

    private void DisableMovement()
    {
        _attackMovement = true;
    }

    private void EnableMovement()
    {
        _attackMovement = false;
    }
}