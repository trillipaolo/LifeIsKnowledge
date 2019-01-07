using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour {

    [Header("Joel Reference")]
    public GameObject target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        target.GetComponent<Animator>().SetBool("isMeditating", false);
        target.GetComponent<Animator>().SetBool("isSleeping", false);
        transform.parent.gameObject.GetComponent<Menu>().ResetColor();
        transform.parent.gameObject.transform.parent.gameObject.GetComponent<SafeArea>().CloseSafeAreaMenu();
    }
}
