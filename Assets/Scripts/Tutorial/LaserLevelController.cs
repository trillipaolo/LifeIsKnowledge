using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLevelController : MonoBehaviour {

    private LaserMachineLaser _laser;

    private void Awake() {
        _laser = GetComponent<LaserMachineLaser>();
    }
    
    private void Start () {
        _laser.On();
	}
	
	// Update is called once per frame
	void Update () {
        if (_laser.endStop) {
            if (_laser.right) {
                _laser.Left();
            } else {
                _laser.Right();
            }
        }
    }
}
