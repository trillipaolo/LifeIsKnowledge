using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO detele!!!
using UnityEngine.SceneManagement;

public class InitializeDamageText : MonoBehaviour {

    private void Awake() {
        FloatingTextController.Initialize();
    }

    //TODO: delete!!!!!
    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        } else if (Input.GetKeyDown("escape")) {
            Application.Quit();
        }
    }
}
