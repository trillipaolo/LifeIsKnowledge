using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDust : MonoBehaviour {

    private void Awake() {
        Invoke("Destroy",1f);
    }

    private void Destroy() {
        Destroy(gameObject);
    }

}
