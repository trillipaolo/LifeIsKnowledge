using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMenuManager : MonoBehaviour {

    [Header("Scriptable Objects: All enemies in the game")]
    public JoelEnemiesKilled enemyList;

    [Header("Scrollbar Menu Properties")]
    public GameObject menuSlider;

    [Header("Enemy Information List")]
    public List<GameObject> enemyInformationList;

    [Header("Activate/Deactivate ScrollBar in the ScrollMenu")]
    public bool hideScrollBar;
    public GameObject scrollBar;
    public GameObject slidingArea;
    public GameObject handle;

    [Header("References: Safe Area interaction")]
    public GameObject blurBackground;
    public GameObject journalButton;
    public bool _calledByButton = false;

    [Header("Back Button Sprites")]
    public Sprite backButtonStart;
    public Sprite backButtonB;

    [Header("Canvas Back Button Reference")]
    public GameObject backButton;

    private List<GameObject> _menuSliders;
    private int _menuSlidersIndex;
    private bool _updateSlider;

    //JoyPad Booleans
    private bool _dpadDown = false;
    private bool _dpadUp = false;

    private void OnEnable()
    {   
        //Initialize List of the Sliders
        _menuSliders = new List<GameObject>();
        _menuSlidersIndex = 0;
        _updateSlider = false;
        

        InitializeScrollbarMenu();
        if (_menuSliders.Count != 0)
        {
            SelectSlider(-1);
        }

        //Hide the scrollbar if hideScrollBar = true
        if (hideScrollBar)
        {
            handle.SetActive(false);
            slidingArea.SetActive(false);

            Image scrollBarImage = scrollBar.GetComponent<Image>();
            scrollBarImage.enabled = false;
        }

        if(_calledByButton)
        {
            backButton.GetComponent<SpriteRenderer>().sprite = backButtonB;
            blurBackground.SetActive(false);
        }
        else
        {
            backButton.GetComponent<SpriteRenderer>().sprite = backButtonStart;
            blurBackground.SetActive(true);
        }
    }

    private void OnDisable()
    {   
        //Destroy the List of Enemy in the menuSlider when the Menu is closed
        foreach (GameObject g in _menuSliders)
        {
            Destroy(g);
        }

        _menuSliders = new List<GameObject>();

        ResetEnemyInformation();
    }

    // Update is called once per frame
    void Update () {
        SliderSelection();

        if (_calledByButton && Input.GetButtonDown("BackToScroll"))
        {
            journalButton.GetComponent<JournalButton>().CloseMenu();
        }
	}

    private void InitializeScrollbarMenu()
    {
        for (int i = 0; i < enemyList.enemies.Length; i++)
        {   
            //Instantiate the Bar only if the enemy was killed at least once
            if (enemyList.enemies[i].timesKilled != 0)
            {
                GameObject slider = Instantiate(menuSlider) as GameObject;
                slider.SetActive(true);

                SingleEnemySlider singleEnemySlider = slider.GetComponent<SingleEnemySlider>();
                singleEnemySlider.SetEnemyName(enemyList.enemies[i].name);
                singleEnemySlider.SetBarFilling(enemyList.enemies[i].timesUnlockCombo, enemyList.enemies[i].timesKilled);

                Debug.Log(singleEnemySlider.GetEnemyName());

                slider.transform.SetParent(menuSlider.transform.parent, false);
                _menuSliders.Add(slider);
            }
        }
    }

    private void SliderSelection()
    {
        SliderMenuUp();

        SliderMenuDown();
    }

    private void SliderMenuUp()
    {
        bool _downInputButton = Input.GetButtonDown("GridUp");
        bool _downInputAxis = Input.GetAxis("GridUp") > 0;

        if ( (_downInputButton || _downInputAxis) && !_dpadUp && (_menuSlidersIndex != 0))
        {
            _menuSlidersIndex -= 1;
            SelectSlider(_menuSlidersIndex + 1);
        }

        _dpadUp = (_downInputButton || _downInputAxis);
    }

    private void SliderMenuDown()
    {
        bool _downInputButton = Input.GetButtonDown("GridDown");
        bool _downInputAxis = Input.GetAxis("GridDown") > 0;

        if ((_downInputButton || _downInputAxis) && !_dpadDown && (_menuSlidersIndex < _menuSliders.Count - 1))
        {
            _menuSlidersIndex += 1;
            SelectSlider(_menuSlidersIndex - 1);
        }

        _dpadDown = (_downInputButton || _downInputAxis);
    }

    //Call this method with parameter -1 in order to highlight only the current button
    //without "de"-highlighting the previous one
    private void SelectSlider(int lastSliderIndex)
    {
        if (lastSliderIndex != -1)
        {   
            foreach (GameObject enemyInformationObject in enemyInformationList)
            {
                EnemyInformation enemyInformation = enemyInformationObject.GetComponent<EnemyInformation>();
                SingleEnemySlider enemySlider = _menuSliders[lastSliderIndex].GetComponent<SingleEnemySlider>();

                if (enemyInformation.name == enemySlider.GetEnemyName())
                {
                    enemyInformationObject.SetActive(false);
                    enemySlider.StopFading();
                }
            }
        }

        foreach (GameObject enemyInformationObject in enemyInformationList)
        {
            EnemyInformation enemyInformation = enemyInformationObject.GetComponent<EnemyInformation>();
            SingleEnemySlider enemySlider = _menuSliders[_menuSlidersIndex].GetComponent<SingleEnemySlider>();

            if (enemyInformation.name == enemySlider.GetEnemyName())
            {
                enemyInformationObject.SetActive(true);
                enemySlider.StartFading();

                if (enemyList.enemies[_menuSlidersIndex].timesKilled >= enemyList.enemies[_menuSlidersIndex].timesUnlockCombo)
                {
                    enemyInformation.EnableCombo();
                    
                }
            }
        }
    }

    private void ResetEnemyInformation()
    {
        foreach(GameObject enemyInformation in enemyInformationList)
        {
            enemyInformation.SetActive(false);
        }
    }
}
