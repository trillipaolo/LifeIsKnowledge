using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
 * This class is responsible for the Raycast system.
 * 
 * Link of the original tutorial:
 * https://www.youtube.com/watch?v=MbWK8bCAU2w&list=PLFt_AvWsXl0f0hqURlhyIoAabKPgRsqjz&index=1
 */
[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour
{
    public LayerMask collisionMask; // Setting layers for collision checking
    const float distanceBetweenRays = .25f;
    public const float skinWidth = .015f; //

    // Number of rays
    [HideInInspector] public int horizontalRayCount;
    [HideInInspector] public int verticalRayCount;

    // Spacing between rays
    [HideInInspector] public float horizontalRaySpacing;
    [HideInInspector] public float verticalRaySpacing;
    [HideInInspector] public BoxCollider2D collider;
    public RaycastOrigins raycastOrigins;

    public void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    public virtual void Start()
    {
        CalculateRaySpacing();
    }

    public void UpdateRaycastOrigins()
    {
        // Shrink the bounds by skin width from each side
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        // Assign corners
        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    public void CalculateRaySpacing()
    {
        // Shrink the bounds by skin width from each side
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        horizontalRayCount = Mathf.RoundToInt(boundsHeight / distanceBetweenRays);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / distanceBetweenRays);

        // Setting spacing between rays
        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}