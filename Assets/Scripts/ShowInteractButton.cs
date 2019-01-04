using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInteractButton : MonoBehaviour {

    private GameObject _button;

	private void Start () {
        _button = GameObject.FindGameObjectsWithTag("Player")[0].transform.root.Find("InteractButton").gameObject;
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
    }

    public void HideButton() {
        _button.SetActive(false);
    }
}
