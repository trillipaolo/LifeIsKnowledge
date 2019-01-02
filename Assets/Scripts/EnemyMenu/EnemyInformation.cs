using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInformation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void EnableCombo ()
    {
        transform.Find("ComboName").gameObject.SetActive(true);
        transform.Find("ComboAnimation").gameObject.SetActive(true);
    }
}
