using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipInteractButton : MonoBehaviour {

    private Transform _joel;

    private void Awake() {
        _joel = transform.root;
    }

    private void Update() {
        if(_joel.localScale.x < 0) {
            transform.localScale = new Vector3(-1,1,1);
        } else {
            transform.localScale = new Vector3(1,1,1);
        }
    }
}
