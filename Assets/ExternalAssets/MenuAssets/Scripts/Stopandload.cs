using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stopandload : MonoBehaviour {

    public GameObject canvas;
    public GameObject minimap;

    private bool _menu;
	// Use this for initialization
	void Start () {
        _menu = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("OpenComboMenu"))
        {
            if (_menu)
            {
                Time.timeScale = 0f;

                SceneManager.LoadScene("ComboMenu", LoadSceneMode.Additive);

                canvas.GetComponent<Canvas>().enabled = false;
                minimap.GetComponent<Camera>().enabled = false;

                _menu = !_menu;
            }
            else
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

                canvas.GetComponent<Canvas>().enabled = true;
                minimap.GetComponent<Camera>().enabled = true;

                _menu = !_menu;
            }
        }

        


	}

    
}
