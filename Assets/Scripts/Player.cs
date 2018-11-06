using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private bool _isGrounded = false;

    public float maxVelocity = 8f;
    public float jumpForce = 5f;
    public float raycastGroundDistance = 0.06f;

    public GameObject leftFoot;
    public GameObject rightFoot;
    public LayerMask terrain;

    private Rigidbody2D _rb;
    private Animator _animator;
    private Transform _trLeftFoot;
    private Transform _trRightFoot;

    public float velocity;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _trLeftFoot = leftFoot.GetComponent<Transform>();
        _trRightFoot = rightFoot.transform;
    }

	void Update () {
        CheckGrounded();

        if (Input.GetKey(KeyCode.A)){
            _rb.AddForce(Vector2.left * maxVelocity,ForceMode2D.Force);
        } else if (Input.GetKey(KeyCode.D)){
            _rb.AddForce(Vector2.right * maxVelocity,ForceMode2D.Force);
        }

        if (Input.GetKeyDown(KeyCode.Space)){
            if (_isGrounded) {
                _rb.AddForce(Vector2.up * jumpForce,ForceMode2D.Impulse);
            }
        }

        velocity = Mathf.Abs(_rb.velocity.x) / maxVelocity;
        if(velocity > 1) {
            velocity = 1;
        }
        _animator.SetFloat("Velocity",velocity);

        if (_rb.velocity.x != 0) {
            transform.localScale = new Vector3(Mathf.Sign(_rb.velocity.x),1,1);
        }
    }

    void CheckGrounded() {
        if (Physics2D.Raycast(_trLeftFoot.position,Vector2.down,raycastGroundDistance,terrain)
            || Physics2D.Raycast(_trRightFoot.position,Vector2.down,raycastGroundDistance,terrain)) {
            _isGrounded = true;
        } else {
            _isGrounded = false;
        }
    }
}
