using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum EnumEnemies {
    DRONE,
    SCIENTIST,
    GUARD
}

public class EnumerationEnemies {
    public static int Size() {
        return Enum.GetValues(typeof(EnumEnemies)).Length;
    }

    public static EnumEnemies[] GetEnumArray() {
        Array temp = Enum.GetValues(typeof(EnumEnemies));

        EnumEnemies[] output = new EnumEnemies[temp.Length];
        for(int i = 0; i < temp.Length; i++) {
            output[i] = (EnumEnemies)temp.GetValue(i);
        }

        return output;
    }
}
