using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestButton : MonoBehaviour {

    [Header("Joel Reference")]
    public GameObject target;

    [Header("Canvas' Reference to show only HealthBar")]
    public Canvas canvas;
    public GameObject damage;
    public GameObject commandText;
    public GameObject healthBar;
    public GameObject minimap;
    public GameObject dialogueBox;
    public GameObject comboBar;

    //Slider reference
    private Slider _healthSlider;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        target.GetComponent<Animator>().SetBool("isSleeping", true);
        target.GetComponent<Animator>().SetBool("isMeditating", false);

        ShowHealthBar();
        _healthSlider = healthBar.GetComponent<Slider>();
        StartCoroutine("Heal");
    }

    private void ShowHealthBar()
    {
        canvas.enabled = true;
        damage.SetActive(false);
        commandText.SetActive(false);
        minimap.SetActive(false);
        dialogueBox.SetActive(false);
        comboBar.SetActive(false);
    }

    private void HideHealthBar()
    {
        comboBar.SetActive(true);
        dialogueBox.SetActive(true);
        minimap.SetActive(true);
        commandText.SetActive(true);
        damage.SetActive(true);
        canvas.enabled = false;
    }

    IEnumerator Heal()
    {   
        for (; target.GetComponentInChildren<JoelHealth>().currentHealth < target.GetComponentInChildren<JoelHealth>().joelHealth.health;)
        {
            int tmpValue = target.GetComponentInChildren<JoelHealth>().currentHealth + 10;

            if (tmpValue < target.GetComponentInChildren<JoelHealth>().joelHealth.health)
            {
                target.GetComponentInChildren<JoelHealth>().currentHealth = tmpValue;
                _healthSlider.value = tmpValue;
            }
            else
            {
                target.GetComponentInChildren<JoelHealth>().currentHealth = target.GetComponentInChildren<JoelHealth>().joelHealth.health;
                _healthSlider.value = target.GetComponentInChildren<JoelHealth>().joelHealth.health;
            }
            yield return new WaitForSecondsRealtime(1.0f);
        }

        HideHealthBar();
    }
}
