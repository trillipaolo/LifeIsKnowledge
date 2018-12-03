using UnityEngine;
using System;

public enum EnumCombo {
    UPANDSMASH = 1
}

public static class EnumerationCombo {

    public static int Size() {
        return Enum.GetValues(typeof(Combo)).Length;
    }
}
