using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBehaviour : MonoBehaviour {

    
    [Header("Following Player speed and delay settings:")]
    public float timeLeft = 5f;
    public float smoothSpeed=5f;
    public float shrinkSpeed = 5f;
    public float speedAccel = 3f;
    public float dstAccel = 2f;
    public bool autoDestruct = true;
    [Header("Jiggle-Y settings:")]
    public float jiggleY=0.5f;
    public float jiggleFreq = 0.5f;
    


    [HideInInspector]
    public bool isclose = false;
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public Vector3 _startPosition;
    [HideInInspector]
    public Vector3 smoothedPosition;
    [HideInInspector]
    public Vector3 growScale;
    [HideInInspector]
    public bool followPlayer = false;

    public virtual void  Start() {
        _startPosition = transform.position;
        player = GameObject.FindWithTag("Player");
        target = player.transform;
    }

    public virtual void  FixedUpdate() {



        if (timeLeft < 0) {
            
            if ((transform.position - target.position).magnitude > dstAccel ) {

                smoothedPosition = Vector3.Lerp(transform.position, target.position, smoothSpeed * Time.deltaTime);
            }
            else {
                isclose = true;
                smoothedPosition = Vector3.Lerp(transform.position, target.position, speedAccel * smoothSpeed * Time.deltaTime);

                if (transform.localScale.sqrMagnitude <= 0.001)
                {
                    if(autoDestruct)
                        Destroy(gameObject);
                }

                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, shrinkSpeed * Time.deltaTime);
            }
            
            transform.position = smoothedPosition;

        }
        else {
                timeLeft -= Time.deltaTime;
            transform.position = _startPosition + new Vector3( 0.0f, jiggleY * (1+ Mathf.Sin(Time.time * jiggleFreq)), 0.0f);
        }

    }
}
