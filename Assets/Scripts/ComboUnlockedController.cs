using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboUnlockedController : MonoBehaviour {

	public JoelEnemiesKilled joelEnemiesKilled;
    public JoelCombos joelCombos;
    public Combo horizontalSpin;
    public bool resetAtStart = false;

    private void Awake() {
        if (resetAtStart) {
            joelEnemiesKilled.Reset();
            for (int i = 0; i < joelEnemiesKilled.enemies.Length; i++) {
                if (joelEnemiesKilled.enemies[i].comboUnlocked.comboName != "Horizontal Spin") {
                    joelEnemiesKilled.enemies[i].comboUnlocked.unlocked = false;
                }
            }

            joelCombos.combos = new Combo[0];

            horizontalSpin.coloumnSaved = -1;
            horizontalSpin.rowSaved = -1;
            horizontalSpin.rotatedSaved = false;
        }
    }
}
