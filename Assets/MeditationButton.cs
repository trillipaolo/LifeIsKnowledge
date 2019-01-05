using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeditationButton : MonoBehaviour {

    [Header("Combo Menu Reference")]
    public GameObject comboMenu;

    [Header("Joel reference")]
    public GameObject target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {   
        target.GetComponent<Animator>().SetBool("isMeditating", true);
        target.GetComponent<Animator>().SetBool("isSleeping", false);

        transform.parent.gameObject.GetComponent<Menu>().HideAllButtons();
        transform.parent.gameObject.GetComponent<Menu>()._buttonPressed = true;

        comboMenu.GetComponentInChildren<ComboMenuManager>()._calledByButton = true;
        comboMenu.SetActive(true);
    }

    public void CloseMenu()
    {
        comboMenu.GetComponentInChildren<ComboMenuManager>().SetCombosChosen();
        comboMenu.SetActive(false);
        comboMenu.GetComponentInChildren<ComboMenuManager>()._calledByButton = false;
        transform.parent.gameObject.GetComponent<Menu>()._buttonPressed = false;
        transform.parent.gameObject.GetComponent<Menu>().ShowAllButtons();
    }
}
