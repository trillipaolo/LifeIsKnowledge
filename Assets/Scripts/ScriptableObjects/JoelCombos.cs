using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/JoelCombo")]
public class JoelCombos : ScriptableObject {

    public Combo[] combos;

    public void SetNumberCombos(int number)
    {
        combos = new Combo[number];
    }

}
