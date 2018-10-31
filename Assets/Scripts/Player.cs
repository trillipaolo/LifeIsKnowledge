using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private bool _isGrounded = false;

    public float velocity = 15f;
    public float jumpForce = 5f;
    public float raycastGroundDistance = 0.06f;

    public GameObject leftFoot;
    public GameObject rightFoot;
    public LayerMask terrain;

    private Rigidbody2D _rb;
    private Transform _trLeftFoot;
    private Transform _trRightFoot;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _trLeftFoot = leftFoot.GetComponent<Transform>();
        _trRightFoot = rightFoot.transform;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        CheckGrounded();

        if (Input.GetKey(KeyCode.A)){
            _rb.AddForce(Vector2.left * velocity,ForceMode2D.Force);
        } else if (Input.GetKey(KeyCode.D)){
            _rb.AddForce(Vector2.right * velocity,ForceMode2D.Force);
        }

        if (Input.GetKeyDown(KeyCode.Space)){
            if (_isGrounded) {
                _rb.AddForce(Vector2.up * jumpForce,ForceMode2D.Impulse);
            }
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
