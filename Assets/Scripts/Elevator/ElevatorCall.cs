using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorCall : MonoBehaviour {
    
    [SerializeField]
    private bool _hasCalled;
    [SerializeField]
    private bool _inCollider;

    private void Update() {
        if (_inCollider) {
            if (Input.GetButton("Teleport")) {
                _hasCalled = true;
            } else {
                _hasCalled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            _inCollider = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            _inCollider = false;
        }
    }

    public bool HasCalled() {
        return _hasCalled;
    }

    public void ResetCalled() {
        _hasCalled = false;
        _inCollider = false;
    }
}
