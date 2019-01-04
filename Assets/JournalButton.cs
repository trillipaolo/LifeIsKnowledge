using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalButton : MonoBehaviour {

    [Header("Enemy Menu Reference")]
    public GameObject enemyMenu;

    [Header("Joel reference")]
    public GameObject target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        enemyMenu.GetComponentInChildren<EnemyMenuManager>()._calledByButton = true;
        enemyMenu.SetActive(true);
    }

    public void CloseMenu()
    {
        enemyMenu.SetActive(false);
        enemyMenu.GetComponentInChildren<EnemyMenuManager>()._calledByButton = false;
    }
}
