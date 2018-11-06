using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float walkSpeed = 5.0f;
    public float jumpSpeed = 5.0f;
    public float gravity = 3.0f;
    public float yNudge = -2.5f;
    public float directionFacing = 1;

    private bool _grounded = false;
    private float _yVelocity = 0f;
    
	void Update () {

        UpdateXMovement();
        UpdateYMovement();
	}

    void UpdateXMovement() {
        Vector3 frameMovement = new Vector3(Input.GetAxis("Horizontal") * walkSpeed,0);

        transform.Translate(frameMovement * Time.deltaTime);
        if (frameMovement.x != 0) {
            directionFacing = Mathf.Sign(frameMovement.x);
            transform.localScale = new Vector3(directionFacing,1,1);
        }
    }

    void UpdateYMovement() {
        if(transform.position.y < yNudge) {
            _grounded = true;
            _yVelocity = 0;
            Vector3 nudge = transform.position;
            nudge.y = yNudge;
            transform.position = nudge;
        }

        if (!_grounded) {
            _yVelocity -= gravity * Time.deltaTime;
        } else {
            if (Input.GetKeyDown(KeyCode.Space)) {
                _yVelocity = jumpSpeed;
                _grounded = false;
            }
        }
        transform.position = new Vector3(transform.position.x,transform.position.y + _yVelocity,0);
    }
}
