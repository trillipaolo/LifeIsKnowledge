using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour {

    private Vector3 _position;
    private bool _chosen;
    public GameObject placeHolderAnimation;

    public bool GetChosen()
    {
        return _chosen;
    }

    public void SetChosen(bool chosen)
    {
        _chosen = chosen;
    }

    private void Awake()
    {
        _position = GetComponent<Transform>().position;
        _chosen = false;
    }

    public void AdjustPlaceHolderPosition(float xOffset, float yOffset) 
    {
        Vector3 placeHolderOffset = new Vector3(xOffset, yOffset, 0);
        Vector3 placeHolderPosition = _position + placeHolderOffset;
        Quaternion placeHolderRotation = new Quaternion(0, 0, 0, 0);
        placeHolderAnimation = Instantiate(placeHolderAnimation, placeHolderPosition, placeHolderRotation);
    }

    public void DestroyPlaceHolder()
    {
        Destroy(placeHolderAnimation);
    }

    public void SwitchPlaceHolderRight()
    {
        AnimationComboMenuPlaceHolder placeHolder = placeHolderAnimation.GetComponent<AnimationComboMenuPlaceHolder>();
        placeHolder.SwitchPlaceHolderRight();
    }

    public void SwitchPlaceHolderLeft()
    {
        AnimationComboMenuPlaceHolder placeHolder = placeHolderAnimation.GetComponent<AnimationComboMenuPlaceHolder>();
        placeHolder.SwitchPlaceHolderLeft();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
