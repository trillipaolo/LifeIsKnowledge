using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SingleEnemySlider : MonoBehaviour {

    [Header("Enemy Name Text Options")]
    public float fontSize;
    public int characterSpacing;
    public TextAlignmentOptions textAlignmentOptions;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetEnemyName(string name)
    {
        TextMeshProUGUI sliderTMP = GetComponentInChildren<TextMeshProUGUI>();
        sliderTMP.fontSize = fontSize;
        sliderTMP.characterSpacing = characterSpacing;
        sliderTMP.alignment = textAlignmentOptions;
        sliderTMP.text = name + "'s understanding:";
    }

    public void SetBarFilling(int maxValue, int value)
    {
        Slider slider = GetComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = maxValue;
        slider.value = value;
    }
}
