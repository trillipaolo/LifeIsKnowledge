using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackPaolo : MonoBehaviour {

    //public float timeBetweenAttacks = 0.5f;     // The time in seconds between each attack.
    public int attackDamage = 17;               // The amount of health taken away per attack.


    private Animator _animator;                              // Reference to the animator component.
    private GameObject _joel;                          // Reference to the player GameObject.
    private JoelHealth _joelHealth;                  // Reference to the player's health.
    private Collider2D _attackCollider;
    //private EnemyHealth _enemyHealth;                    // Reference to this enemy's health.
    private bool _playerInRange;                         // Whether player is within the trigger collider and can be attacked.
    private float _timer;                                // Timer for counting up to the next attack.

    void Awake() {
        // Setting up the references.
        _joel = GameObject.FindGameObjectWithTag("Player");
        _joelHealth = _joel.GetComponentInChildren<JoelHealth>();
        _attackCollider = GetComponent<BoxCollider2D>();
        //enemyHealth = GetComponent<EnemyHealth>();
        //_animator = GetComponent<Animator>();
        DeactivateAttackCollider();
    }

    void Update() {
        // Add the time since Update was last called to the timer.
        _timer += Time.deltaTime;

        // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
        if (_playerInRange) {// && _timer >= timeBetweenAttacks && enemyHealth.currentHealth > 0) {
            // ... attack.
            Attack();
        }

        // If the player has zero or less health...
        if (_joelHealth.currentHealth <= 0) {
            // ... tell the animator the player is dead.
            //_animator.SetTrigger("PlayerDead");
        }
    }

    void Attack() {
        // Reset the timer.
        _timer = 0f;

        // If the player has health to lose...
        if (_joelHealth.currentHealth > 0) {
            // ... damage the player.
            _joelHealth.TakeDamage(attackDamage);
            _playerInRange = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        // If the entering collider is the player...
        if (other.gameObject == _joel) {
            // ... the player is in range.
            _playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        // If the exiting collider is the player...
        if (other.gameObject == _joel) {
            // ... the player is no longer in range.
            _playerInRange = false;
        }
    }

    public void ActivateAttackCollider() {
        _attackCollider.enabled = true;
    }

    public void DeactivateAttackCollider() {
        _attackCollider.enabled = false;
    }
}
