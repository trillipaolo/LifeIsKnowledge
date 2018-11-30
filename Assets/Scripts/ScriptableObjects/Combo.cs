using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Combo")]
public class Combo : ScriptableObject {

    [Header("Combo Properties")]
    public string comboName;
    public ComboButton[] buttonSequence;
    public Sprite comboSprite;

    //Combo position in the grid
    [Header("Combo GridPosition: DOT NOT TOUCH")]
    public int rowSaved;
    public int coloumnSaved;
    public bool rotatedSaved;

    public bool CheckConsistency()
    {
        if ( buttonSequence[0].button == Buttons.CIRCLE &&
             buttonSequence[0].button == Buttons.CROSS &&
             buttonSequence[0].button == Buttons.TRIANGLE)
        {
            return false;
        }

        return true;
    }

    // +++++++++++++++++++

    [Header("Combo Keys")]
    public bool combo1Key;
    public bool combo2Key;
    public bool topKey;
    public bool middleKey;
    public bool downKey;
    [Header("Combo Values")]
    public EnumCombo enumCombo;
    public float cooldown;
    public float damage;
}
