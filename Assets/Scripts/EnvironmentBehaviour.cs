using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentBehaviour : MonoBehaviour {

    private Animator _animator;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _animator.Play("MushroomIdle",0,Random.Range(0.0f,1.0f));
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            _animator.SetTrigger("Move");
        }
    }
}
