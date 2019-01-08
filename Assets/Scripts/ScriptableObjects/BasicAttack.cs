using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BasicAttack")]
public class BasicAttack : ScriptableObject {

    public float[] damage = new float[EnumerationBasicAttack.Size()];

    public void InitializeDamage() {
        damage[(int)EnumBasicAttack.TOPTOP] = 50;
        damage[(int)EnumBasicAttack.TOPMIDDLE] = 30;
        damage[(int)EnumBasicAttack.TOPDOWN] = 20;
        damage[(int)EnumBasicAttack.MIDDLETOP] = 30;
        damage[(int)EnumBasicAttack.MIDDLEMIDDLE] = 50;
        damage[(int)EnumBasicAttack.MIDDLEDOWN] = 30;
        damage[(int)EnumBasicAttack.DOWNTOP] = 20;
        damage[(int)EnumBasicAttack.DOWNMIDDLE] = 30;
        damage[(int)EnumBasicAttack.DOWNDOWN] = 50;
        damage[(int)EnumBasicAttack.JUMP] = 20;
    }
}
