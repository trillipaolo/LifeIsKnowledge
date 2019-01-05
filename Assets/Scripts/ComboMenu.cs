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

    private bool _comboMenu = false;
    private bool _enemyMenu = false;

    //Boolean to prevent ComboMenu from working while in Safe Area
    public bool _menu = false;

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

    private void Update()
    {
        if (!_menu)
        {
            //Open the menu: ComboMenu is opened as first by default
            if (Input.GetButtonDown("OpenMenu"))
            {
                if (!_comboMenu && !_enemyMenu)
                {
                    DisableJoel();
                    ActivateComboMenu();
                }
                else
                {
                    if (_comboMenu)
                    {   
                        DeactivateComboMenu();
                        EnableJoel();
                    }
                    if (_enemyMenu)
                    {
                        DeactivateEnemyMenu();
                        EnableJoel();
                    }
                }
            }

            //While in ComboMenu switch to the EnemyMenu
            if (((Input.GetAxis("SwitchToEnemyMenu") == 1) || (Input.GetButtonDown("SwitchToEnemyMenu"))) && _comboMenu && !_enemyMenu)
            {
                DeactivateComboMenu();

                ActivateEnemyMenu();
            }

            //While in EnemyMenu switch to the ComboMenu
            if (((Input.GetAxis("SwitchToComboMenu") == 1) || (Input.GetButtonDown("SwitchToComboMenu"))) && !_comboMenu && _enemyMenu)
            {
                DeactivateEnemyMenu();

                ActivateComboMenu();
            }
        }
    }

    private void ActivateComboMenu()
    {
        Time.timeScale = 0;
        comboMenu.SetActive(true);
        canvas.enabled = false;
        minimap.enabled = true;
        _comboMenu = true;
    }

    private void DeactivateComboMenu()
    {
        Time.timeScale = 1;

        ComboMenuManager menuManager = comboMenuManager.GetComponent<ComboMenuManager>();
        menuManager.SetCombosChosen();

        comboMenu.SetActive(false);
        canvas.enabled = true;
        minimap.enabled = true;
        _comboMenu = false;
    }

    private void ActivateEnemyMenu()
    {
        Time.timeScale = 0;
        enemyMenu.SetActive(true);
        canvas.enabled = false;
        minimap.enabled = false;
        _enemyMenu = true;
    }

    private void DeactivateEnemyMenu()
    {
        Time.timeScale = 1;

        enemyMenu.SetActive(false);
        canvas.enabled = true;
        minimap.enabled = true;
        _enemyMenu = false;
    }

    private void DisableJoel()
    {
        GetComponent<PlayerInput>().DisableInput();
        GetComponent<PlayerPhysics>().DisableMovement();
    }

    private void EnableJoel()
    {
        GetComponent<PlayerInput>().EnableInput();
        GetComponent<PlayerPhysics>().EnableMovement();
    }
}

    
