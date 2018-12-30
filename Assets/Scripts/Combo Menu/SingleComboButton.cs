using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SingleComboButton : MonoBehaviour {

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
        ComboMenuManager.Instance.SetCurrentCombo(_menuIndex);
    }

    public void SetImage(Sprite comboSprite)
    {
        Image imageComponent = GetComponent<Image>();
        imageComponent.preserveAspect = true;
        imageComponent.sprite = comboSprite;
        
    }

    public void SetKeyFrame(Sprite keyFrame)
    {
        SpriteRenderer keyFrameSprite = GetComponentInChildren<SpriteRenderer>();
        keyFrameSprite.sprite = keyFrame;
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
