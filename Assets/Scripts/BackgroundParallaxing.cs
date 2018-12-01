using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallaxing : MonoBehaviour {

    public Transform camera;
    private Vector3 _lastCameraPosition;

    public Transform[] backgrounds;
    private float[] _parallaxScales;
    public float smoothing = 1f;

    private void Awake() {
        _lastCameraPosition = camera.position;
        _parallaxScales = new float[backgrounds.Length];
        for(int i = 0; i < _parallaxScales.Length; i++) {
            _parallaxScales[i] = backgrounds[i].position.z * -1;
        }
    }
	
	void Update () {
		for(int i = 0; i < backgrounds.Length; i++) {
            float parallax = (_lastCameraPosition.x - camera.position.x) * _parallaxScales[i];
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX,backgrounds[i].position.y,backgrounds[i].position.z);
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position,backgroundTargetPos,smoothing * Time.deltaTime);
        }
        _lastCameraPosition = camera.position;
	}
}
