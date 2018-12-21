using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretAreaScript : MonoBehaviour {

    private ParticleSystem[] _particles;
    private Transform _joel;
    private AudioManager audioManager;

    private void Awake() {
        _particles = GetComponentsInChildren<ParticleSystem>();
        _joel = GameObject.FindWithTag("Player").transform;
        DisableParticles();
    }

    private void Start() {
        audioManager = AudioManager.instance;
    }

    private void DisableParticles() {
        for(int i = 0; i < _particles.Length; i++) {
            _particles[i].enableEmission = false;
        }
    }

    private void EnableParticles() {
        for (int i = 0; i < _particles.Length; i++) {
            _particles[i].time = i % 3;
            _particles[i].enableEmission = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.transform == _joel) {
            EnableParticles();
            audioManager.Play("Confetti");
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.transform == _joel) {
            DisableParticles();
        }
    }
}
