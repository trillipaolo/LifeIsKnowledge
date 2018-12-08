using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePosition : MonoBehaviour {

    public Vector2 pos;

    [Header("Dialogue Properties")]
    public GameObject target;
    public Camera cameraTarget;

    //Variable to adjust dialogue position
    private Vector2 _targetPosition;
    private RectTransform _rt;
    private RectTransform _canvasRT;
    private Vector2 _targetScreenPosition;

	// Use this for initialization
	void Start () {
        _targetPosition = target.transform.position;

        _rt = GetComponent<RectTransform>();
        _canvasRT = GetComponentInParent<Canvas>().GetComponent<RectTransform>();



        _rt.anchorMax = _targetScreenPosition;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
