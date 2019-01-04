using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        transform.parent.gameObject.transform.parent.gameObject.GetComponent<SafeArea>().CloseSafeAreaMenu();
    }
}
