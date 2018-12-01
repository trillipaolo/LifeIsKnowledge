using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDust : MonoBehaviour {

    public GameObject dustSpread;
    public Transform transform;
    private Animator _animator;

    private bool _lastGrounded;

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    private void Update() {
        bool _currentGrounded = _animator.GetBool("Grounded");
        if(_lastGrounded != _currentGrounded) {
            SpawnDustSpread();
        }
        _lastGrounded = _currentGrounded;
    }

    public void SpawnDustSpread() {
        Instantiate(dustSpread,transform.position,Quaternion.identity);
    }
}
