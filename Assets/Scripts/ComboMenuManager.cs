using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboMenuManager : MonoBehaviour {

    public static ComboMenuManager Instance { get; private set; }

    [Header("Scriptable Objects")]
    public Combo[] combos;

    [Header("Scrolling Menu Prefab")]
    public GameObject menuButton;

    private List<GameObject> _menuButtons;

    //Combo Selected in the scrolling menu
    private int _currentCombo;
    private bool _menuToMatrix;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        InitializeScrollingMenu();
	}
	
	// Update is called once per frame
	void Update () {
        PrintCurrentCombo();
	}

    private void InitializeScrollingMenu()
    {
        _menuButtons = new List<GameObject>();

        for (int i = 0; i < combos.Length; i++)
        {
            GameObject button = Instantiate(menuButton) as GameObject;
            button.SetActive(true);

            button.GetComponent<SingleComboButton>().SetIndex(i);
            button.GetComponent<SingleComboButton>().SetComboName(combos[i].comboName);
            button.GetComponent<SingleComboButton>().SetImage(combos[i].comboSprite);

            button.transform.SetParent(menuButton.transform.parent, false);

            _menuButtons.Add(button);
        }
    }

    private void PrintCurrentCombo()
    {
        Debug.Log("Currently Selected Combo is: " + combos[_currentCombo].comboName + " and its index is " + _currentCombo);
    }

    public void SetCurrentCombo(int buttonPressed)
    {
        _currentCombo = buttonPressed;
        _menuToMatrix = true;
    }
}
