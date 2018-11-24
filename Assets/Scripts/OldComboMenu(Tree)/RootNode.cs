using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootNode : MonoBehaviour {

    private Vector3 _position;
    private bool _set;

    private void Awake()
    {
        _position = GetComponent<Transform>().position;
        _set = true;
    }

    public bool GetSet()
    {
        return _set;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
