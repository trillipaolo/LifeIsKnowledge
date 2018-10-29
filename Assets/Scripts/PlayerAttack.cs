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
}
