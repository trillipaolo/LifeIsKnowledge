using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleComboButton : MonoBehaviour {

    public ComboMenuManager comboMenuManager;

    private string _comboName;
    private int _menuIndex;

    public void SetIndex(int index)
    {
        _menuIndex = index;
    }

    public void SetComboName(string name)
    {
        Text buttonText = GetComponentInChildren<Text>();
        buttonText.text = name;
    }

    
    public void OnClick ()
    {
        Debug.Log("Button number " + _menuIndex + " has been clicked");
        comboMenuManager.SetCurrentCombo(_menuIndex);
    }

    public void SetImage(Sprite comboSprite)
    {
        Image imageComponent = GetComponent<Image>();
        imageComponent.preserveAspect = true;
        imageComponent.sprite = comboSprite;
    }

    public void DisableButton()
    {
        GetComponent<Button>().interactable = false;
    }

    public void EnableButton()
    {
        GetComponent<Button>().interactable = true;
    }
}
