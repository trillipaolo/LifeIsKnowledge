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
        if (_teleport) {
            if (Input.GetKeyDown(KeyCode.W)) {
                StartCoroutine("Teleporting");
            }
        }
    }

    IEnumerator Teleporting() {
        yield return new WaitForSeconds(0.01f);

        SceneManager.LoadScene(scene);
    }
}
