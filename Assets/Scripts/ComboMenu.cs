using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboMenu : MonoBehaviour {

    public GameObject comboMenu;
    public GameObject background;
    public Canvas canvas;
    public Camera minimap;

    private bool _menu = true;

    void Update () {
        if (Input.GetButtonDown("OpenComboMenu"))
        {
            if (_menu)
            {
                Time.timeScale = 0;
                comboMenu.SetActive(true);
                background.SetActive(true);
                canvas.enabled = false;
                minimap.enabled = false;
                _menu = !_menu;
            }
            else
            {
                Time.timeScale = 1;
                comboMenu.SetActive(false);
                background.SetActive(false);
                canvas.enabled = true;
                minimap.enabled = true;
                _menu = !_menu;
            }
        }
    }
}
