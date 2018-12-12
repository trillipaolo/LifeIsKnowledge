using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EnemyNode")]
public class EnemyNode : ScriptableObject{

	public EnumEnemies enemy;
    public int timesKilled;
    public int timesUnlockCombo;
    public Combo comboUnlocked;

}
