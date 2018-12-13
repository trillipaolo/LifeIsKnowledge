using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretAreaScript : MonoBehaviour {

    private ParticleSystem[] _particles;
    private Transform _joel;

    private void Awake() {
        _particles = GetComponentsInChildren<ParticleSystem>();
        _joel = GameObject.FindWithTag("Player").transform;
        DisableParticles();
    }

    private void DisableParticles() {
        for(int i = 0; i < _particles.Length; i++) {
            _particles[i].enableEmission = false;
        }
    }

    private void EnableParticles() {
        for (int i = 0; i < _particles.Length; i++) {
            _particles[i].time = 0;
            _particles[i].enableEmission = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.transform == _joel) {
            EnableParticles();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.transform == _joel) {
            DisableParticles();
        }
    }
}
