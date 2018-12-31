using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Quick fix for the animation clipping between planes in the menu
        transform.position = transform.position - new Vector3(0, 0, transform.position.z);
	}
}
