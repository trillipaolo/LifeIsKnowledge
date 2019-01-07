using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneManager : MonoBehaviour {

    private AudioManager _audioManager;

    private void Start() {
        _audioManager = AudioManager.instance;

        _audioManager.Pause("Theme");
        _audioManager.Play("CutScene");
    }

    private void Update() {

        // Quit game and reload scene
        if (Input.GetKeyDown(KeyCode.T)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        } else if (Input.GetKeyDown("escape")) {
            Application.Quit();
        }
    }
}
