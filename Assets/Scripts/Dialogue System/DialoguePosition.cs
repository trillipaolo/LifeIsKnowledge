using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePosition : MonoBehaviour {
    
    [Header("Dialogue Properties")]
    public GameObject target;
    public Camera cameraTarget;

    [Header("Text Offset")]
    public float xOffset;
    public float yOffset;

    //Variable to adjust dialogue position
    private RectTransform _rt;
    private Vector2 _targetScreenPosition;

	// Use this for initialization
	void Start () {
        _rt = GetComponent<RectTransform>();

        //Transform the absolute position of the Target Gameobject in to the Local position in the screen
        //_targetScreenPosition = 0,0 is the the bottom left of the camera
        //_targetScreenPosition = 1,1 is the the top right of the camera
        //This line of code set _targetScreenPosition to an intermediate value which represents the position of the target
        //  on the screen
        _targetScreenPosition = cameraTarget.WorldToViewportPoint(target.transform.position);

        //Fix the position of the text to the target's one plus an offset
        _rt.anchorMax = _targetScreenPosition + new Vector2(xOffset, yOffset);
        _rt.anchorMin = _targetScreenPosition + new Vector2(xOffset, yOffset);
	}
	
	// Update is called once per frame
	void Update () {
        //Update the position of the text at each frame
        _targetScreenPosition = cameraTarget.WorldToViewportPoint(target.transform.position);

        _rt.anchorMax = _targetScreenPosition + new Vector2(xOffset, yOffset);
        _rt.anchorMin = _targetScreenPosition + new Vector2(xOffset, yOffset);
    }
}
