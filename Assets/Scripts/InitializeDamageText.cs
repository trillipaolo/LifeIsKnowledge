using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeDamageText : MonoBehaviour {

    private void Awake() {
        FloatingTextController.Initialize();
    }
}
