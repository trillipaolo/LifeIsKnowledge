using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealJoel : EnemyAttackPaolo {

    public override void Awake() {
        joel = GameObject.FindGameObjectWithTag("Player");
        joelHealth = joel.GetComponentInChildren<JoelHealth>();
        attackCollider = GetComponent<BoxCollider2D>();
    }

    public override void Update() {
        // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
        if (_playerInRange) {// && _timer >= timeBetweenAttacks && enemyHealth.currentHealth > 0) {
            // ... attack.
            Heal();
        }

        // If the player has zero or less health...
        if (joelHealth.currentHealth <= 0) {
            // ... tell the animator the player is dead.
            //_animator.SetTrigger("PlayerDead");
        }
    }

    public void Heal() {

        // If the player has health to lose...
        if (joelHealth.currentHealth > 0) {
            // ... damage the player.
            joelHealth.TakeDamage(attackDamage);
        }
    }
}
