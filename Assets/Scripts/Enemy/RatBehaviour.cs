using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
 * This class is responsible for handling player's input.
 * It requires Controller2D class which calculates movement depending on velocity vector.
 * 
 * Link to the original tutorial:
 * https://www.youtube.com/watch?v=MbWK8bCAU2w&list=PLFt_AvWsXl0f0hqURlhyIoAabKPgRsqjz&index=1
 */

[RequireComponent(typeof(RatController2D))]
public class RatBehaviour : MonoBehaviour {
    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    public float moveSpeed = 6;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector2 velocity;
    float velocityXSmoothing;

    RatController2D controller;
    private Animator _animator;
    Vector2 directionalInput;

    void Start() {
        controller = GetComponent<RatController2D>();
        _animator = GetComponent<Animator>();
        _animator.SetBool("Grounded",true);

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex,2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) + minJumpHeight);
        print("Gravity: " + gravity + " Jump velocity: " + maxJumpVelocity);
    }

    void Update() {
        CalculateVelocity();

        _animator.SetFloat("XSpeed",Mathf.Abs(velocity.x));

        controller.Move(velocity * Time.deltaTime,directionalInput);
        if (controller.collisions.above || controller.collisions.below) {
            velocity.y = 0;
        }

        if (controller.collisions.below) {
            _animator.SetBool("Grounded",true);
        } else {
            _animator.SetBool("Grounded",false);
        }
    }

    public void SetDirectionalInput(Vector2 input) {
        directionalInput = input;
    }

    public void OnJumpInputDown() {
        if (controller.collisions.below) {
            velocity.y = maxJumpVelocity;
        }
    }

    public void OnJumpInputUp() {
        if (velocity.y > minJumpVelocity) {
            velocity.y = minJumpVelocity;
        }
    }

    void CalculateVelocity() {
        // Smoothing the movement depending on whether the player is grounded or not.
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x,targetVelocityX,ref velocityXSmoothing,
            (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime; // Applying gravity to velocity
    }
}
