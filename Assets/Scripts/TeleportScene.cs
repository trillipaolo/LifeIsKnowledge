using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TeleportScene : MonoBehaviour {

    [Header("Next Level Reference")]
    //New Scene to load
    public string scene;
    //Reference to the Respawn point
    public JoelRespawn joelRespawn;
    public Coordinates nextLevelRespawn;

    [Header("Loading Screen Reference")]
    //Loading screen reference
    public GameObject loadingScreen;
    //Canvas reference
    public Canvas canvas;

    [Header("Joel Reference")]
    //Player GameObject
    public GameObject target;

    //True if the player is in front of the stairs (Colliders are actually colliding)
    //False otherwise
    private bool _teleport;
    private bool _lastTeleport = false;

    private void Awake() {
        _teleport = false;
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == target) {
            _teleport = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject == target) {
            _teleport = false;
        }
    }

    private void Update() {
        bool _teleportInput = Input.GetAxis("Teleport") > 0.75f;

        if (_teleportInput && !_lastTeleport && _teleport) {
            //SceneManager.LoadScene(scene);
            StartCoroutine("LoadAsynchronously");
        }

        _lastTeleport = _teleportInput;
    }

    IEnumerator LoadAsynchronously ()
    {
        canvas.enabled = false;
        loadingScreen.SetActive(true);
        joelRespawn.SetRespawnPoint(nextLevelRespawn);

        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            loadingScreen.GetComponentInChildren<Slider>().value = progress;

            yield return null;
        }
        
    }
}
