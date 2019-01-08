using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowInteractButton : MonoBehaviour {

    private GameObject _button;
    private TextMeshProUGUI _tmp;

    public string text;

	private void Start () {
        _button = GameObject.FindGameObjectsWithTag("Player")[0].transform.root.Find("InteractButton").gameObject;
        _tmp = _button.transform.Find("InteractCanvas").GetComponentInChildren<TextMeshProUGUI>();
	}

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            ShowButton();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            HideButton();
        }
    }

    public void ShowButton() {
        _button.SetActive(true);
        _tmp.text = text;
    }

    public void HideButton() {
        _button.SetActive(false);
    }
}
