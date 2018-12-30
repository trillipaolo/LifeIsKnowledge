using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailDropBehaviour : DropBehaviour
{
    private TrailRenderer trail;
    public float velocityTimeTrail = 5f;
    private float smoothedTrailTime; 
    // Use this for initialization
    new void Start () {
        base.Start();
        trail = gameObject.GetComponent<TrailRenderer>();
        smoothedTrailTime = trail.time;
    }
	
	// Update is called once per frame
	new void FixedUpdate () {
        base.FixedUpdate();

        if (isclose)
            smoothedTrailTime = Mathf.Lerp(trail.time, 0f, velocityTimeTrail * Time.deltaTime);
        trail.time = smoothedTrailTime;
    }
}
