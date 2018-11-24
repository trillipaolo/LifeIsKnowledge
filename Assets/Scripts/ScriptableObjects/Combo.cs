using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Combo", menuName = "Combo")]
public class Combo : ScriptableObject {

    public string comboName;
    public ComboButton[] buttonSequence;
    public Sprite comboSprite;

    public bool CheckConsistency()
    {
        if ( buttonSequence[0].button == Button.CIRCLE &&
             buttonSequence[0].button == Button.CROSS &&
             buttonSequence[0].button == Button.TRIANGLE)
        {
            return false;
        }

        return true;
    }
}
