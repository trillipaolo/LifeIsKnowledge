using UnityEngine;
using System;

public enum EnumCombo {
    UPANDSMASH = 1,
    HORIZONTALSPIN = 2,
    ROPETHROW = 3
}

public static class EnumerationCombo {

    public static int Size() {
        return Enum.GetValues(typeof(Combo)).Length;
    }
}
