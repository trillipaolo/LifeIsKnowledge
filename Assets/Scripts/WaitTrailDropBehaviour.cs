﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitTrailDropBehaviour : DropBehaviour {

    
    [Header("Set PickUp distance:")]
    public float pickUpDst = 3f;
    private bool waitingUntilClose = true;

    
    private TrailRenderer trail;
    [Header("Set Trail Fade Velocity:")]
    public float velocityTimeTrail = 5f;
    private float smoothedTrailTime;

    [Header("PowerUp reference:")]
    public GameObject powerUp;
    private float diff=0f;
    private bool isPwrUp = false;

    private JoelHealth jH;
    // Use this for initialization
    new void Start () {
        base.Start();
        trail = gameObject.GetComponent<TrailRenderer>();
        smoothedTrailTime = trail.time;
        jH = player.GetComponentInChildren<JoelHealth>();
    }

    // Update is called once per frame
    public override void FixedUpdate () {

        diff = (transform.position - target.position).magnitude;
        if (diff < pickUpDst)
            followPlayer = true;

        if (timeLeft < 0)
        {

            if (diff > dstAccel)
            {

                smoothedPosition = Vector3.Lerp(transform.position, target.position, smoothSpeed * Time.deltaTime);
            }
            else
            {
                isclose = true;
                smoothedPosition = Vector3.Lerp(transform.position, target.position, speedAccel * smoothSpeed * Time.deltaTime);

                if (transform.localScale.sqrMagnitude <= 0.001)
                {
                    
                    if (autoDestruct)
                        Destroy(gameObject);
                }

                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, shrinkSpeed * Time.deltaTime);
            }

            transform.position = smoothedPosition;

        }
        else
        {
            if (!waitingUntilClose || followPlayer)
                timeLeft -= Time.deltaTime;


            transform.position = _startPosition + new Vector3(0.0f, jiggleY * (1 + Mathf.Sin(Time.time * jiggleFreq)), 0.0f);
        }

        if (isclose)
        {
            smoothedTrailTime = Mathf.Lerp(trail.time, 0f, velocityTimeTrail * Time.deltaTime);

            if (isPwrUp == false)
            {
                Debug.Log("AUMENTA STATISTICHE");
                powerUp.GetComponent<PowerUp>().IncreaseAttack();
                powerUp.GetComponent<PowerUp>().IncreaseHealth();
                jH.UpdateJoelHealth();
                isPwrUp = true;
            }
        }
        trail.time = smoothedTrailTime;
        
    }
}