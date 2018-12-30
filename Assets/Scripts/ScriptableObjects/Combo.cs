using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObjects/Combo")]
public class Combo : ScriptableObject {

    

    [Header("Combo Properties")]
    public string comboName;
    public ComboButton[] buttonSequence;
    public Sprite comboSprite;
    public Sprite highlightedComboSprite;
    public bool unlocked;

    //Combo position in the grid
    [Header("Combo GridPosition: DOT NOT TOUCH")]
    public int rowSaved;
    public int coloumnSaved;
    public bool rotatedSaved;

    public bool CheckConsistency()
    {
        if ( buttonSequence[0].button == Buttons.B &&
             buttonSequence[0].button == Buttons.A &&
             buttonSequence[0].button == Buttons.Y)
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
    public float[] damage;

    [Header("Combo Cooldown Image")]
    public Sprite cooldownImage;
    public Sprite cooldownKey;

    public void InitializeDamage() {
        switch (enumCombo) {
            case EnumCombo.UPANDSMASH:
                damage = new float[2];
                damage[0] = 10;
                damage[1] = 200;
                break;
            case EnumCombo.HORIZONTALSPIN:
                damage = new float[1];
                damage[0] = 40;
                break;
            case EnumCombo.ROPETHROW:
                damage = new float[2];
                damage[0] = 40;
                damage[1] = 80;
                break;
        }
    }
}
