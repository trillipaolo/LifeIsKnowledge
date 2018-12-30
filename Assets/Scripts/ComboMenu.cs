using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboMenu : MonoBehaviour
{

    public GameObject comboMenu;
    public GameObject comboMenuManager;

    public GameObject enemyMenu;
    public GameObject enemyMenuManager;

    public Canvas canvas;
    public Camera minimap;

    [Header("Reset number of enemy killed at restart")]
    public bool resetEnemyKilled;

    private bool _comboMenu = true;
    private bool _enemyMenu = true;

    public void Start()
    {
       if (resetEnemyKilled)
        {
            EnemyMenuManager enemyMenu = enemyMenuManager.GetComponent<EnemyMenuManager>();
            
            for(int i = 0; i < enemyMenu.enemyList.enemies.Length; i++)
            {
                enemyMenu.enemyList.enemies[i].timesKilled = 0;
            }
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("OpenComboMenu"))
        {
            if (_comboMenu)
            {
                Time.timeScale = 0;
                comboMenu.SetActive(true);
                canvas.enabled = false;
                minimap.enabled = false;
                _comboMenu = !_comboMenu;
            }
            else
            {
                Time.timeScale = 1;

                ComboMenuManager menuManager = comboMenuManager.GetComponent<ComboMenuManager>();
                menuManager.SetCombosChosen();

                comboMenu.SetActive(false);
                canvas.enabled = true;
                minimap.enabled = true;
                _comboMenu = !_comboMenu;
            }
        }

        if (Input.GetButtonDown("OpenEnemyMenu"))
        {
            if (_enemyMenu)
            {
                Time.timeScale = 0;
                enemyMenu.SetActive(true);
                canvas.enabled = false;
                minimap.enabled = false;
                _enemyMenu = !_enemyMenu;
            }
            else
            {
                Time.timeScale = 1;

                enemyMenu.SetActive(false);
                canvas.enabled = true;
                minimap.enabled = true;
                _enemyMenu = !_enemyMenu;
            }
        }
    }
}

    
