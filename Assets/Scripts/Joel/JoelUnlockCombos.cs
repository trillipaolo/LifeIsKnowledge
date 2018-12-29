using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoelUnlockCombos : MonoBehaviour {

    public GameObject[] combos;
	public JoelEnemiesKilled joelEnemiesKilled;
    private bool _changed = false;

    void Update() {
        if (_changed) {
            UpdateUnlockedCombos();
        }
    }

    private void UpdateUnlockedCombos() {
        EnemyNode[] enemiesKilled = joelEnemiesKilled.enemies;
        for (int i = 0; i < enemiesKilled.Length; i++) {
            if(enemiesKilled[i].timesKilled >= enemiesKilled[i].timesUnlockCombo) {
                Combo combo = enemiesKilled[i].comboUnlocked;
                if (!combo.unlocked) {
                    combo.unlocked = true;

                    int tmp=0;
                    for(int j=0; j < combos.Length; j++)
                    {
                        //Debug.Log(combos[j].name + "==" + combo.name + "Drop" + "result" + combos[j].name.Equals(combo.name + "Drop"));
                        if (combos[j].name.Equals(combo.name + "Drop"))
                            tmp = j;
                    }
                    
                    Instantiate(combos[tmp], transform.position, Quaternion.identity);
                    
                    FloatingTextController.CreateUnlockComboText(combo.comboName + " combo unlocked!!",transform.position + Vector3.up);
                }
            }
        }
    }

    public void KilledEnemy(EnumEnemies enemy) {
        for(int i = 0; i < joelEnemiesKilled.enemies.Length; i++) {
            if(joelEnemiesKilled.enemies[i].enemy == enemy) {
                joelEnemiesKilled.enemies[i].timesKilled++;

                _changed = true;
                return;
            }
        }
    }
}
