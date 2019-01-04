﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    [Header("Menu Buttons: Rest, Meditate, Continue")]
    public GameObject[] menuButtons;
    private int _menuButtonsIndex;

    //JoyPad Booleans
    private bool _dpadUp = false;
    private bool _dpadDown = false;

    private void OnEnable()
    {   
        //Set menu properties
        _menuButtonsIndex = 0;

        //Select First Button
        menuButtons[_menuButtonsIndex].GetComponent<Button>().Select();
    }

    private void Update()
    {
        ButtonSelection();

        ButtonPressed();
    }

    private void ButtonPressed()
    {
        if (Input.GetButtonDown("GridInsertion"))
        {
            menuButtons[_menuButtonsIndex].GetComponent<Button>().onClick.Invoke();
        }
    }

    private void ButtonSelection()
    {
        ButtonUp();

        ButtonDown();
    }

    private void ButtonUp()
    {
        bool _upInputButton = Input.GetButtonDown("GridUp");
        bool _upInputAxis = Input.GetAxis("GridUp") > 0;

        if ((_upInputButton || _upInputAxis) && !_dpadUp && (_menuButtonsIndex != 0))
        {
            _menuButtonsIndex -= 1;
            menuButtons[_menuButtonsIndex].GetComponent<Button>().Select();
        }

        _dpadUp = (_upInputButton || _upInputAxis);
    }

    private void ButtonDown()
    {
        bool _downInputButton = Input.GetButtonDown("GridDown");
        bool _downInputAxis = Input.GetAxis("GridDown") > 0;

        if ((_downInputButton || _downInputAxis) && !_dpadDown && (_menuButtonsIndex < menuButtons.Length - 1))
        {
            _menuButtonsIndex += 1;
            menuButtons[_menuButtonsIndex].GetComponent<Button>().Select();
        }

        _dpadDown = (_downInputButton || _downInputAxis);
    }

    public void PressContinueButton()
    {
        transform.parent.gameObject.GetComponent<SafeArea>().CloseSafeAreaMenu();
    }
}