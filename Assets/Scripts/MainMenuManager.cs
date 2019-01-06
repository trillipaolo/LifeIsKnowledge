using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    AudioManager audioManager;
    public string mainMenuThemeSound = "MainMenuTheme";

    private void Awake()
    {
        audioManager = AudioManager.instance;
        audioManager.Play(mainMenuThemeSound);
    }

    public void PlayGame() {
        audioManager.Stop(mainMenuThemeSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() {
        audioManager.Stop(mainMenuThemeSound);
        Application.Quit();
    }
}
