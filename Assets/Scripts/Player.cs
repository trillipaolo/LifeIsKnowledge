using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private bool isGrounded = false;

    public float velocity = 15f;
    public float jumpForce = 5f;
    public float raycastGroundDistance = 0.06f;

    public GameObject LeftFoot;
    public GameObject RightFoot;
    public LayerMask Terrain;

    private Rigidbody2D rb;
    private Transform trLeftFoot;
    private Transform trRightFoot;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        trLeftFoot = LeftFoot.GetComponent<Transform>();
        trRightFoot = RightFoot.transform;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        CheckGrounded();

        if (Input.GetKey(KeyCode.A)){
            rb.AddForce(Vector2.left * velocity,ForceMode2D.Force);
        } else if (Input.GetKey(KeyCode.D)){
            rb.AddForce(Vector2.right * velocity,ForceMode2D.Force);
        }

        if (Input.GetKeyDown(KeyCode.Space)){
            if (isGrounded) {
                rb.AddForce(Vector2.up * jumpForce,ForceMode2D.Impulse);
            }
        }
	}

    void CheckGrounded() {
        if (Physics2D.Raycast(trLeftFoot.position,Vector2.down,raycastGroundDistance,Terrain)
            || Physics2D.Raycast(trRightFoot.position,Vector2.down,raycastGroundDistance,Terrain)) {
            isGrounded = true;
        } else {
            isGrounded = false;
        }
    }
}
