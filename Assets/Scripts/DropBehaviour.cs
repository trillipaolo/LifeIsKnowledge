using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBehaviour : MonoBehaviour {

    private Transform target;
    public float timeLeft = 5f;
    public float smoothSpeed=5f;
    public float shrinkSpeed = 5f;
    public float speedAccel = 3f;
    public float dst = 2f;
    public float jiggleY=0.5f;
    public float jiggleFreq = 0.5f;
    
    
    private Vector3 _startPosition;
    Vector3 smoothedPosition;
    Vector3 growScale;

    private void Start() {
        _startPosition = transform.position;
        target = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate() {




        
        if (timeLeft < 0) {
            
            if ((transform.position - target.position).magnitude > dst ) {

                smoothedPosition = Vector3.Lerp(transform.position, target.position, smoothSpeed * Time.deltaTime);
            }
            else {
                
                smoothedPosition = Vector3.Lerp(transform.position, target.position, speedAccel * smoothSpeed * Time.deltaTime);
                /*
                if (growScale.x > 0 && growScale.y > 0 && growScale.z > 0) { 
                    growScale -= new Vector3(1, 1, 1) * 0.0001f;
                }
                else
                {
                    Destroy(gameObject);
                }*/

                if (transform.localScale.sqrMagnitude <= 0.001)
                {
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
