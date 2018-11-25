using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Animator _joel;

    public float speed = 0f;

    public float walkSpeed = 15.0f;
    public float jumpSpeed = 0.5f;
    public float gravity = 3.0f;
    public float yNudge = -2.7f;
    public float directionFacing = 1;

    private bool _grounded = false;
    private float _yVelocity = 0f;

    private void Awake() {
        _joel = GetComponent<Animator>();
    }


    void Update () {

        UpdateXMovement();
        UpdateYMovement();
	}

    void UpdateXMovement() {
        Vector3 frameMovement = new Vector3(Input.GetAxis("Horizontal") * walkSpeed,0);

        float _lastX = transform.position.x;
        transform.Translate(frameMovement * Time.deltaTime);
        speed = Mathf.Abs(transform.position.x - _lastX)/(walkSpeed * Time.deltaTime);
        if(speed < 0) {
            speed = 0;
        } else {
            if(Mathf.Abs(speed - 1) < 0.0001) {
                speed = 1;
            }
        }
        _joel.SetFloat("XSpeed",speed);

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
