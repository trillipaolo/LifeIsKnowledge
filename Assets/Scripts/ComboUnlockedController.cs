using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboUnlockedController : MonoBehaviour {

	public JoelEnemiesKilled joelEnemiesKilled;
    public JoelCombos joelCombos;
    public BasicAttack basicAttacks;
    public bool resetAtStart = false;

    private void Awake() {
        if (resetAtStart) {
            joelEnemiesKilled.Reset();
            for (int i = 0; i < joelEnemiesKilled.enemies.Length; i++) {
                joelEnemiesKilled.enemies[i].comboUnlocked.unlocked = false;
                joelEnemiesKilled.enemies[i].comboUnlocked.InitializeDamage();
            }
            basicAttacks.InitializeDamage();

            joelCombos.combos = new Combo[0];
        }
    }
}
