using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float WalkSpeed = 5.0f;
    public float JumpSpeed = 5.0f;
    public float Gravity = 3.0f;
    public float YNudge = -2.5f;

    private bool _grounded = false;
    private float _yVelocity = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        UpdateXMovement();
        UpdateYMovement();
	}

    void UpdateXMovement() {
        Vector3 frameMovement = new Vector3(Input.GetAxis("Horizontal") * WalkSpeed,0);

        transform.Translate(frameMovement * Time.deltaTime);
        if (frameMovement.x != 0) {
            transform.localScale = new Vector3(Mathf.Sign(frameMovement.x),1,1);
        }
    }

    void UpdateYMovement() {
        if(transform.position.y < YNudge) {
            _grounded = true;
            _yVelocity = 0;
            Vector3 nudge = transform.position;
            nudge.y = YNudge;
            transform.position = nudge;
        }

        if (!_grounded) {
            _yVelocity -= Gravity * Time.deltaTime;
        } else {
            if (Input.GetKeyDown(KeyCode.Space)) {
                _yVelocity = JumpSpeed;
                _grounded = false;
            }
        }
        transform.position = new Vector3(transform.position.x,transform.position.y + _yVelocity,0);
    }
}
