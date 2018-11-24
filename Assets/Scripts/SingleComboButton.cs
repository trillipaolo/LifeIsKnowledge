using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleComboButton : MonoBehaviour {

    public ComboMenuManager comboMenuManager;

    private string comboName;
    private int menuIndex;

    public void SetIndex(int index)
    {
        menuIndex = index;
    }

    public void SetComboName(string name)
    {
        comboName = name;
    }

    public void OnClick ()
    {
        comboMenuManager.SetCurrentCombo(menuIndex);
    }

    public void SetImage(Sprite comboSprite)
    {
        Image imageComponent = GetComponent<Image>();
        imageComponent.preserveAspect = true;
        imageComponent.sprite = comboSprite;
    }
}
