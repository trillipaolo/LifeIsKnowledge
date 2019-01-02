using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SingleComboButton : MonoBehaviour {

    [Header("Combo Name Text Options")]
    public float fontSize;
    public float characterSpacing;
    public TextAlignmentOptions textAlignmentOptions;


    private int _menuIndex;
    private SpriteRenderer _comboSpriteHighlighted;
    private SpriteRenderer _glowEffect;

    public void SetIndex(int index)
    {
        _menuIndex = index;
    }

    public void SetComboName(string name)
    {
        TextMeshProUGUI buttonTMP = GetComponentInChildren<TextMeshProUGUI>();
        buttonTMP.fontSize = fontSize;
        buttonTMP.characterSpacing = characterSpacing;
        buttonTMP.alignment = textAlignmentOptions;
        buttonTMP.text = name;
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

    public void SetGlowEffect(Sprite glowEffect)
    {
        _glowEffect = transform.Find("SingleComboButtonGlow").GetComponent<SpriteRenderer>();
        _glowEffect.sprite = glowEffect;

        Color glowEffectColor = _glowEffect.material.color;
        glowEffectColor.a = 0;
        _glowEffect.material.color = glowEffectColor;
    }

    public void DisableButton()
    {
        GetComponent<Button>().interactable = false;
    }

    public void EnableButton()
    {
        GetComponent<Button>().interactable = true;
    }

    public void StartHighlight()
    {
        _comboSpriteHighlighted = transform.Find("ComboSpriteHighlighted").GetComponent<SpriteRenderer>();
        StartCoroutine("Fading");
    }

    public void StopHighlight()
    {
        StopCoroutine("Fading");
        ResetColor();
    }

    private void ResetColor()
    {
        Color c = _comboSpriteHighlighted.material.color;
        c.a = 1;
        _comboSpriteHighlighted.material.color = c;

        Color glowEffectColor = _glowEffect.material.color;
        glowEffectColor.a = 0;
        _glowEffect.material.color = glowEffectColor;
    }

    IEnumerator Fading()
    {
        do
        {
            for (float f = 0.05f; f <= 1; f += 0.05f)
            {
                Color c = _comboSpriteHighlighted.material.color;
                c.a = f;
                _comboSpriteHighlighted.material.color = c;
                _glowEffect.material.color = c;
                yield return new WaitForSecondsRealtime(0.05f);
            }

            for (float f = 1f; f >= -0.05f; f -= 0.05f)
            {
                Color c = _comboSpriteHighlighted.material.color;
                c.a = f;
                _comboSpriteHighlighted.material.color = c;
                _glowEffect.material.color = c;
                yield return new WaitForSecondsRealtime(0.05f);
            }
        } while (true);
    }

    

}
