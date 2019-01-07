using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipInteractButton : MonoBehaviour {

    private void Update() {
        if(transform.lossyScale.x < 1) {
            transform.localScale -= 2 * Vector3.right * transform.localScale.x;
        }
    }
}
