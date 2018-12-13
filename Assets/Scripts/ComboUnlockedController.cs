using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboUnlockedController : MonoBehaviour {

	public JoelEnemiesKilled joelEnemiesKilled;
    public bool resetAtStart = false;

    private void Awake() {
        if (resetAtStart) {
            joelEnemiesKilled.Reset();
            for (int i = 0; i < joelEnemiesKilled.enemies.Length; i++) {
                if (joelEnemiesKilled.enemies[i].comboUnlocked.comboName != "HorizontalSpin") {
                    joelEnemiesKilled.enemies[i].comboUnlocked.unlocked = false;
                }
            }
        }
    }
}
