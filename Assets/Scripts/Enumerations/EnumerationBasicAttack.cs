using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum EnumBasicAttack {
    TOPTOP = 0,
    TOPMIDDLE = 1,
    TOPDOWN = 2,
    MIDDLETOP = 3,
    MIDDLEMIDDLE = 4,
    MIDDLEDOWN = 5,
    DOWNTOP = 6,
    DOWNMIDDLE = 7,
    DOWNDOWN = 8
}

public static class EnumerationBasicAttack {

    public static int Size() {
        return Enum.GetValues(typeof(EnumBasicAttack)).Length;
    }
}
