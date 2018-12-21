using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportScene : MonoBehaviour {

    //New Scene to load
    public string scene;

    //Player GameObject
    public GameObject target;

    //Teleport Kind
    public TeleportType teleportType;

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
        bool _teleportInput = Input.GetAxis("Teleport") > 0;

        if (_teleportInput && !_lastTeleport && _teleport) {
            SceneManager.LoadScene(scene);
        }

        _lastTeleport = _teleportInput;
    }
}
