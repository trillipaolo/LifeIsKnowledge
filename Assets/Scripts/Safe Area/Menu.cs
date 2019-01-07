using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour {

    [Header("Menu Buttons: Meditate, Journal, Continue")]
    public GameObject[] menuButtons;
    private int _menuButtonsIndex;
    private Button _currentButton;

    //Menu Booleans
    public bool _buttonPressed = false;

    //JoyPad Booleans
    private bool _dpadUp = false;
    private bool _dpadDown = false;

    private void OnEnable()
    {   
        //Set menu properties
        _menuButtonsIndex = 0;

        //Select First Button
        _currentButton = menuButtons[_menuButtonsIndex].GetComponent<Button>();
        StartHighlight();
    }

    private void Update()
    {

        //Avoid input detection if another menu is opened
        if (!_buttonPressed)
        {
            Debug.Log("You should be able to select button in the Menu");

            ButtonSelection();

            ButtonPressed();
        }
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
            Debug.Log("ButtonUp Pressed");
            StopHighlight();
            _menuButtonsIndex -= 1;
            _currentButton = menuButtons[_menuButtonsIndex].GetComponent<Button>();
            StartHighlight();
        }

        _dpadUp = (_upInputButton || _upInputAxis);
    }

    private void ButtonDown()
    {
        bool _downInputButton = Input.GetButtonDown("GridDown");
        bool _downInputAxis = Input.GetAxis("GridDown") > 0;

        if ((_downInputButton || _downInputAxis) && !_dpadDown && (_menuButtonsIndex < menuButtons.Length - 1))
        {
            Debug.Log("ButtonUp Pressed");
            StopHighlight();
            _menuButtonsIndex += 1;
            _currentButton = menuButtons[_menuButtonsIndex].GetComponent<Button>();
            StartHighlight();
        }

        _dpadDown = (_downInputButton || _downInputAxis);
    }

    public void PressContinueButton()
    {
        transform.parent.gameObject.GetComponent<SafeArea>().CloseSafeAreaMenu();
    }

    public void HideAllButtons()
    {
        foreach (GameObject button in menuButtons)
        {
            button.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
        }
    }

    public void ShowAllButtons()
    {
        foreach (GameObject button in menuButtons)
        {
            Debug.Log("Trying to re-show all buttons");
            button.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
        }
    }

    public void StartHighlight()
    {
        StartCoroutine("Fading");
    }

    public void StopHighlight()
    {
        StopAllCoroutines();
        ResetColor();
    }

    public void ResetColor()
    {
        ColorBlock cb = _currentButton.colors;
        Color c = cb.normalColor;
        c.a = 0;
        cb.normalColor = c;
        _currentButton.colors = cb;
    }

    IEnumerator Fading()
    {
        do
        {
            for (float f = 0.01f; f <= 0.2f; f += 0.01f)
            {
                ColorBlock cb = _currentButton.colors;
                Color c = cb.normalColor;
                c.a = f;
                cb.normalColor = c;
                _currentButton.colors = cb;
                yield return new WaitForSecondsRealtime(0.05f);
            }

            for (float f = 0.2f; f >= -0.01f; f -= 0.01f)
            {
                ColorBlock cb = _currentButton.colors;
                Color c = cb.normalColor;
                c.a = f;
                cb.normalColor = c;
                _currentButton.colors = cb;
                yield return new WaitForSecondsRealtime(0.05f);
            }
        } while (true);
    }
}
