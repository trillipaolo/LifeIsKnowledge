using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootNode : MonoBehaviour {

    private Vector3 _position;

    private void Awake()
    {
        _position = GetComponent<Transform>().position;
    }

    public Vector3 GetPosition()
    {
        return _position;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
