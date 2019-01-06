using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitGame : MonoBehaviour {

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        } else if (Input.GetKeyDown("escape")) {
            Application.Quit();
        }
    }
}
