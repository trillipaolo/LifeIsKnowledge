using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stopandload : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.O))
        {
            Time.timeScale = 0f;

            SceneManager.LoadScene("ComboMenu", LoadSceneMode.Additive);

            GetComponentInChildren<Canvas>().enabled = false;
            GetComponentInChildren<Camera>().enabled = false;

        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject[] toEliminate = GameObject.FindGameObjectsWithTag("GridCell");
            if (toEliminate != null)
            {
                foreach (GameObject gameObject in toEliminate)
                {
                    Destroy(gameObject);
                }
            }

            Time.timeScale = 1f;

            SceneManager.UnloadSceneAsync("ComboMenu");

            GetComponentInChildren<Canvas>().enabled = true;
            GetComponentInChildren<Camera>().enabled = true;
        }

        


	}

    
}
