using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    private BoxCollider2D _frontTop;
    private BoxCollider2D _frontMiddle;
    private BoxCollider2D _frontDown;
    private BoxCollider2D _backTop;
    private BoxCollider2D _backMiddle;
    private BoxCollider2D _backDown;
    private BoxCollider2D[] _attackColliders;

    //TODO import from Weapon Sciptable object
    public Transform tr;
    public float height = 2f;
    public float lenght = 4f;

    private void Awake() {
        _attackColliders = GetComponents<BoxCollider2D>();

        _frontTop = _attackColliders[0];
        _frontMiddle = _attackColliders[1];
        _frontDown = _attackColliders[2];
        _backTop = _attackColliders[3];
        _backMiddle = _attackColliders[4];
        _backDown = _attackColliders[5];
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            _frontTop.enabled = false;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            _frontMiddle.enabled = false;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            _frontDown.enabled = false;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            _backTop.enabled = false;
        if (Input.GetKeyDown(KeyCode.Alpha5))
            _backMiddle.enabled = false;
        if (Input.GetKeyDown(KeyCode.Alpha6))
            _backDown.enabled = false;
    }

    public void ActiveFrontTopCollider(ColliderPosition colliderPosition) {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(Vector2.down,Vector2.down, 0);
        for(int i = 0; i < colliders.Length; i++) {
            //colliders[i].GetComponent<Enemy>().TakeDamage(10);
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(tr.position, new Vector3(lenght, height, 0));
    }
}
