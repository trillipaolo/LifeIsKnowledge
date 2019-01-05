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

    private string enemyName;
    private Image _glowEffect;

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

        enemyName = name;
    }

    public string GetEnemyName()
    {
        return enemyName;
    }

    public void SetBarFilling(int maxValue, int value)
    {
        Slider slider = GetComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = maxValue;
        slider.value = value;
    }

    public void StartFading()
    {
        Debug.Log("StartFading");

        _glowEffect = transform.Find("Background_Glow").GetComponent<Image>();

        StartCoroutine("Fading");
    }

    public void StopFading()
    {
        StopCoroutine("Fading");
        ResetColor();
    }

    private void ResetColor()
    {
        Color glowEffectColor = _glowEffect.color;
        glowEffectColor.a = 0;
        _glowEffect.color = glowEffectColor;
    }

    IEnumerator Fading()
    {
        do
        {
            for (float f = 0.05f; f <= 1; f += 0.05f)
            {
                Debug.Log("Setting a to " + f);

                Color c = _glowEffect.color;
                c.a = f;
                _glowEffect.color = c;
                yield return new WaitForSecondsRealtime(0.05f);
            }

            for (float f = 1f; f >= -0.05f; f -= 0.05f)
            {
                Color c = _glowEffect.color;
                c.a = f;
                _glowEffect.color = c;
                yield return new WaitForSecondsRealtime(0.05f);
            }
        } while (true);
    }
}
