using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BasicAttack")]
public class BasicAttack : ScriptableObject {

    public float[] damage = new float[EnumerationBasicAttack.Size()];

    public void InitializeDamage() {
        damage[(int)EnumBasicAttack.TOPTOP] = 100;
        damage[(int)EnumBasicAttack.TOPMIDDLE] = 55;
        damage[(int)EnumBasicAttack.TOPDOWN] = 40;
        damage[(int)EnumBasicAttack.MIDDLETOP] = 55;
        damage[(int)EnumBasicAttack.MIDDLEMIDDLE] = 100;
        damage[(int)EnumBasicAttack.MIDDLEDOWN] = 55;
        damage[(int)EnumBasicAttack.DOWNTOP] = 40;
        damage[(int)EnumBasicAttack.DOWNMIDDLE] = 55;
        damage[(int)EnumBasicAttack.DOWNDOWN] = 100;
    }
}
