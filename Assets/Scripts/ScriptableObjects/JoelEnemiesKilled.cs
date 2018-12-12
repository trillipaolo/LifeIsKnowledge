using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/JoelEnemiesKilled")]
public class JoelEnemiesKilled : ScriptableObject {

    public EnemyNode[] enemies;

    public void Reset() {
        if(enemies != null) {
            for(int i = 0; i < enemies.Length; i++) {
                enemies[i].timesKilled = 0;
            }
        }
    }

    public void Initialize() {
        enemies = new EnemyNode[EnumerationEnemies.Size()];
        EnumEnemies[] enemiesEnum = EnumerationEnemies.GetEnumArray();

        for(int i = 0; i < enemies.Length; i++) {
            enemies[i].enemy = enemiesEnum[i];
            enemies[i].timesKilled = 0;
        }
    }

}
