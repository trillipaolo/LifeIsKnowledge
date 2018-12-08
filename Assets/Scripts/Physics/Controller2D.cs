using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/***
 * This class is responsible for moving the player,
 * constraining movement by calculating vertical and horizontal collisions.
 * Collision detection is implemented with the Raycast system.
 * 
 * Link of the original tutorial:
 * https://www.youtube.com/watch?v=MbWK8bCAU2w&list=PLFt_AvWsXl0f0hqURlhyIoAabKPgRsqjz&index=1
 */

public class Controller2D : RaycastController
{
    public CollisionInfo collisions;
    [HideInInspector] public Vector2 playerInput;
    public bool facingRight = true; // For determining which way the player is currently facing.
    public bool movementDisable = false;
    public Vector2 distanceAttack;

    public int[] raysHorizontalArray;
    public int[] raysVerticalArray;
//    public Camera camera;

    public override void Start()
    {
        base.Start();
        raysHorizontalArray = new int [horizontalRayCount];
        raysVerticalArray = new int [verticalRayCount];
    }

    public void Move(Vector2 moveAmount)
    {
//        if (!movementDisable)
        {
            UpdateRaycastOrigins();
            collisions.Reset();

            if (moveAmount.x != 0)
            {
                if (moveAmount.x > 0 && !facingRight)
                {
                    Flip();
                }
                else if (moveAmount.x < 0 && facingRight)
                {
                    Flip();
                }

                HorizontalCollisions(ref moveAmount);
            }

            if (moveAmount.y != 0)
            {
                VerticalCollisions(ref moveAmount);
            }

            transform.Translate(moveAmount);
        }
    }

    public void AttackMove(float distance)
    {
//        camera.GetComponent<CameraFollow>().smoothX = true;
        distanceAttack = new Vector2(distance * (facingRight ? 1: -1) * Time.deltaTime, 0 );
        
        UpdateRaycastOrigins();
        collisions.Reset();

        if (distance != 0)
        {
            if (distanceAttack.x > 0 && !facingRight)
            {
                Flip();
            }
            else if (distanceAttack.x < 0 && facingRight)
            {
                Flip();
            }

            HorizontalCollisions(ref distanceAttack);
        }

        if (distanceAttack.y != 0)
        {
            VerticalCollisions(ref distanceAttack);
        }

        transform.Translate(distanceAttack,0);
    }

    void HorizontalCollisions(ref Vector2 moveAmount)
    {
        float directionX = Mathf.Sign(moveAmount.x); // Moving left: -1, moving right: 1
        float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;
        for (int i = 0; i < horizontalRayCount; i++)
        {
            // If we've moving left, we want to start Rays from the bottom left corner,
            // if right - from bottom right corner.
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if (hit)
            {
                Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.green);
                moveAmount.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
                
                raysHorizontalArray[i] = 1;
            }
        }
    }

    void VerticalCollisions(ref Vector2 moveAmount)
    {
        float directionY = Mathf.Sign(moveAmount.y); // Moving down: -1, moving up: 1
        float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;
        for (int i = 0; i < verticalRayCount; i++)
        {
            // If we've moving down, we want to start Rays from the bottom left corner,
            // if up - from top left corner.
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            if (hit)
            {
                Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.green);
                moveAmount.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;

                raysVerticalArray[i] = 1;
            }
        }
    }

    // This structure keeps the track of the collisions
    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }

    public void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;
Debug.Log("fLIPPING");
        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}