﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboMenuManager : MonoBehaviour {

    public static ComboMenuManager Instance { get; private set; }

    [Header("Scriptable Objects")]
    public Combo[] combos;

    [Header("Scrolling Menu Prefab")]
    public GameObject menuButton;

    [Header("Grid Menu Properties")]
    public GameObject menuCell;
    public int rows;
    public int coloumns;
    public float xOffset;
    public float yOffset;

    [Header("Reset Grid Status")]
    public bool reset;

    [Header("Grid Sprites for Combos")]
    public Sprite emptyCell;
    public Sprite initialCell;
    public Sprite initialCellRotated;
    public Sprite finalCell;
    public Sprite finalCellRotated;
    public Sprite intermediateCell;
    public Sprite intermediateCellRotated;

    //List of Buttons in the scrolling menu: each button refers to a combo
    private List<GameObject> _menuButtons;

    //Combo Selected in the scrolling menu
    private int _currentCombo;

    //List of cells in the grid menu: each cells can contain one button of a combo
    private ComboGridCell[,] _menuGridCells;

    //Current position and state in the grid
    private int _row;
    private int _coloumn;
    private bool _rotated;
    private bool _scrollToGrid;

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
    void Start()
    {
        //The Menu starts with the Scrolling Menu active
        _scrollToGrid = false;

        //No Combo selected at the beginning
        _currentCombo = -1;

        //Combo is not rotated at the beginning (which means horizontal)
        _rotated = false;

        //Starting point of Grid Position is element 0,0
        _row = 0;
        _coloumn = 0;

        //Initialize Menus
        InitializeScrollingMenu();
        InitializeGridMenu();

        //Set if the Grid must be re-set each time the Scene is played
        if (reset)
        {
            ResetGrid();
        }
        else
        {
            ReconstructGrid();
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (_scrollToGrid)
        {
            //What to do when we're in the grid menu
            

            //Get back to Scroll menu pressing Backspace
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                StopHighlight();
                _scrollToGrid = false;
                _rotated = false;
                _currentCombo = -1;
                _row = 0;
                _coloumn = 0;
                return;
            }

            Combo currentCombo = combos[_currentCombo];

            //Movement in the grid
            GridMovement();

            //Rotation in the grid
            GridRotation();

            //Insertion in the grid
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GridInsertion();
                
            }
        }
        else
        {
            //What to do when we're in the scroll menu
        }
	}

    private void ResetGrid()
    {
        for (int i = 0; i < combos.Length; i++)
        {
            combos[i].coloumnSaved = -1;
            combos[i].rowSaved = -1;
            combos[i].rotatedSaved = false;
        }
    }

    private void ReconstructGrid()
    {
        Debug.Log("Trying to Reconstruct the grid");
        for(int i = 0; i < combos.Length; i++)
        {
            
            if ((combos[i].coloumnSaved != -1) && (combos[i].rowSaved != -1))
            {
                _currentCombo = i;
                _rotated = combos[i].rotatedSaved;
                _row = combos[i].rowSaved;
                _coloumn = combos[i].coloumnSaved;
                GridInsertion();
            }
        }

        //No Combo selected at the beginning
        _currentCombo = -1;

        //Combo is not rotated at the beginning (which means horizontal)
        _rotated = false;

        //Starting point of Grid Position is element 0,0
        _row = 0;
        _coloumn = 0;
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

    private void InitializeGridMenu()
    {
        _menuGridCells = new ComboGridCell[rows, coloumns];

        Vector3 cellOffset = menuCell.GetComponent<Transform>().position;

        for(int i = 0; i < rows; i++)
        {
            for(int j = 0; j < coloumns; j++)
            {
                GameObject newGridCell = Instantiate(menuCell, cellOffset, new Quaternion(0, 0, 0, 0));
                newGridCell.SetActive(true);
                cellOffset += new Vector3(xOffset, 0, 0);

                _menuGridCells[i, j] = newGridCell.GetComponent<ComboGridCell>();
            }

            cellOffset += new Vector3(-xOffset * coloumns, -yOffset, 0);   
        }
    }

    private void Highlight()
    {
        Combo comboToHighlight = combos[_currentCombo];

        try
        {
            if (_rotated)
            {
                for (int i = 0; i < comboToHighlight.buttonSequence.Length; i++)
                {
                    _menuGridCells[_row + i, _coloumn].StartFading();
                }
            }
            else
            {
                for(int i = 0; i < comboToHighlight.buttonSequence.Length; i++)
                {
                    _menuGridCells[_row, _coloumn + i].StartFading();
                }
            }
        } 
        catch
        {
            Debug.Log("Trying to reach a Cell outside the grid: highlight is misscalled, combo is misplaced");
        }
    }

    private void StopHighlight()
    {
        Combo comboToHighlight = combos[_currentCombo];

        try
        {
            if (_rotated)
            {
                for (int i = 0; i < comboToHighlight.buttonSequence.Length; i++)
                {
                    _menuGridCells[_row + i, _coloumn].StopFading();
                }
            }
            else
            {
                for (int i = 0; i < comboToHighlight.buttonSequence.Length; i++)
                {
                    _menuGridCells[_row, _coloumn + i].StopFading();
                }
            }
        }
        catch
        {
            Debug.Log("Trying to reach a Cell outside the grid: highlight is misscalled, combo is misplaced");
        }
    }

    private void GridMovement()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {   
            //Stop previous location fading effect
            StopHighlight();

            _row += 1;
            
            if (!CheckMovementConsistency())
            {
                _row -= 1;
                Debug.Log("Player is trying to go outside the Grid");
            }

            //Start next location fading effect
            Highlight();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            //Stop previous location fading effect
            StopHighlight();

            _row -= 1;

            if (!CheckMovementConsistency())
            {
                _row += 1;
                Debug.Log("Player is trying to go outside the Grid");
            }

            //Start next location fading effect
            Highlight();
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            //Stop previous location fading effect
            StopHighlight();

            _coloumn -= 1;

            if (!CheckMovementConsistency())
            {
                _coloumn += 1;
                Debug.Log("Player is trying to go outside the Grid");
            }

            //Start next location fading effect
            Highlight();
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            //Stop previous location fading effect
            StopHighlight();

            _coloumn += 1;

            if (!CheckMovementConsistency())
            {
                _coloumn -= 1;
                Debug.Log("Player is trying to go outside the Grid");
            }

            //Start next location fading effect
            Highlight();
        }
    }

    private void GridRotation()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Stop previous location fading effect
            StopHighlight();

            _rotated = !_rotated;

            int excessCells = CheckRotationConsistency();

            if (excessCells != 0)
            {
                AdjustRotationPosition(excessCells);

                if (!CheckMovementConsistency())
                {
                    AdjustRotationPosition(-excessCells);
                    _rotated = !_rotated;
                    Debug.Log("Player is trying to rotate a combo which is too long");
                }
            }

            //Start next location fading effect
            Highlight();
        }
    }

    private void GridInsertion()
    {   
        //Check if the combo can be inserted in this position
        if (!CheckInsertionConsistency())
        {
            Debug.Log("Cannot insert combo here: at least one grid cell is already occupied");
            return;
        }

        //Stop the fading effect of all GridCells
        StopHighlight();

        Combo comboToPlace = combos[_currentCombo];

        try
        {
            if (_rotated)
            {
                //Save the initial position of the combo: this helps in deletion of the combo and in saving grid status
                combos[_currentCombo].rowSaved = _row;
                combos[_currentCombo].coloumnSaved = _coloumn;
                combos[_currentCombo].rotatedSaved = _rotated;

                for (int i = 0; i < comboToPlace.buttonSequence.Length; i++)
                {
                    //Set the current GridCell as occupied
                    _menuGridCells[_row + i, _coloumn].SetOccupied(true);

                    //Set the GridCell border (sprite)
                    if (i == 0)
                    {
                        _menuGridCells[_row + i, _coloumn].GetComponent<SpriteRenderer>().sprite = initialCellRotated;
                    }
                    else if (i < comboToPlace.buttonSequence.Length - 1)
                    {
                        _menuGridCells[_row + i, _coloumn].GetComponent<SpriteRenderer>().sprite = intermediateCellRotated;
                    }
                    else
                    {
                        _menuGridCells[_row + i, _coloumn].GetComponent<SpriteRenderer>().sprite = finalCellRotated;
                    }

                    //Set the GridCell content (sprite)
                    SpriteRenderer gridContent = _menuGridCells[_row + i, _coloumn].transform.GetChild(0).transform.GetComponent<SpriteRenderer>();
                    gridContent.sprite = comboToPlace.buttonSequence[i].sprite;
                }
            }
            else
            {
                //Save the initial position of the combo: this helps in deletion of the combo and in saving matrix status
                combos[_currentCombo].rowSaved = _row;
                combos[_currentCombo].coloumnSaved = _coloumn;
                combos[_currentCombo].rotatedSaved = _rotated;

                for (int i = 0; i < comboToPlace.buttonSequence.Length; i++)
                {
                    //Set the current GridCell as occupied
                    _menuGridCells[_row, _coloumn + i].SetOccupied(true);

                    //Set the GridCell border (sprite)
                    if (i == 0)
                    {
                        _menuGridCells[_row, _coloumn + i].GetComponent<SpriteRenderer>().sprite = initialCell;
                    }
                    else if (i < comboToPlace.buttonSequence.Length - 1)
                    {
                        _menuGridCells[_row, _coloumn + i].GetComponent<SpriteRenderer>().sprite = intermediateCell;
                    }
                    else
                    {
                        _menuGridCells[_row, _coloumn + i].GetComponent<SpriteRenderer>().sprite = finalCell;
                    }

                    //Set the GridCell content (sprite)
                    SpriteRenderer gridContent = _menuGridCells[_row, _coloumn + i].transform.GetChild(0).transform.GetComponent<SpriteRenderer>();
                    gridContent.sprite = comboToPlace.buttonSequence[i].sprite;
                }
            }
        }
        catch
        {
            Debug.Log("Trying to missplace a combo. Something wrong is happened with the actual position. All check should be already done at this point");
        }

        _scrollToGrid = false;
        EnableScrollMenu();
    }

    private void GridDeletion()
    {
        Combo comboToDelete = combos[_currentCombo];
        int startingRow = comboToDelete.rowSaved;
        int startingColoumn = comboToDelete.coloumnSaved;

        try
        {
            if (comboToDelete.rotatedSaved)
            {

                for (int i = 0; i < comboToDelete.buttonSequence.Length; i++)
                {
                    //Set the current GridCell as free
                    _menuGridCells[startingRow + i, startingColoumn].SetOccupied(false);

                    //Reset Gridcell sprite
                    _menuGridCells[startingRow + i, startingColoumn].GetComponent<SpriteRenderer>().sprite = emptyCell;

                    //Reset Gridcell content
                    SpriteRenderer gridContent = _menuGridCells[startingRow + i, startingColoumn].transform.GetChild(0).transform.GetComponent<SpriteRenderer>();
                    gridContent.sprite = null;
                }
            }
            else
            {
                for (int i = 0; i < comboToDelete.buttonSequence.Length; i++)
                {
                    //Set the current GridCell as free
                    _menuGridCells[startingRow, startingColoumn + i].SetOccupied(false);

                    //Reset Gridcell sprite
                    _menuGridCells[startingRow, startingColoumn + i].GetComponent<SpriteRenderer>().sprite = emptyCell;

                    //Reset GridCell content
                    SpriteRenderer gridContent = _menuGridCells[startingRow, startingColoumn + i].transform.GetChild(0).transform.GetComponent<SpriteRenderer>();
                    gridContent.sprite = null;
                }
            }
        }
        catch
        {
            Debug.Log("Something went wrong while deleting combo number " + _currentCombo);
        }

        //Reset the Scriptable Object
        combos[_currentCombo].rowSaved = -1;
        combos[_currentCombo].coloumnSaved = -1;
        combos[_currentCombo].rotatedSaved = false;
        _scrollToGrid = false;
    }

    private bool CheckInsertionConsistency()
    {   
        Combo comboToPlace = combos[_currentCombo];

        try
        {   
            if (_rotated)
            {
                for (int i = 0; i < comboToPlace.buttonSequence.Length; i++)
                {
                    //Set the current GridCell as occupied
                    if (_menuGridCells[_row + i, _coloumn].GetOccupied())
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < comboToPlace.buttonSequence.Length; i++)
                {
                    if (_menuGridCells[_row, _coloumn + i].GetOccupied())
                    {
                        return false;
                    }
                }
            }
        }
        catch
        {
            Debug.Log("Trying to missplace a combo. Something wrong is happened with the actual position. All check should be already done at this point");
        }

        return true;
    }

    private bool CheckMovementConsistency()
    {
        Combo comboToHighlight = combos[_currentCombo];

        try
        {
            if (_rotated)
            {
                for (int i = 0; i < comboToHighlight.buttonSequence.Length; i++)
                {
                    ComboGridCell cell = _menuGridCells[_row + i, _coloumn];
                }
            }
            else
            {
                for (int i = 0; i < comboToHighlight.buttonSequence.Length; i++)
                {
                    ComboGridCell cell = _menuGridCells[_row, _coloumn + i];
                }
            }
        }
        catch
        {
            return false;
        }

        return true;
    }

    private int CheckRotationConsistency()
    {
        Combo comboToHighlight = combos[_currentCombo];

        int countExcessCells = 0;

        if (_rotated)
        {
            for (int i = 0; i < comboToHighlight.buttonSequence.Length; i++)
            {
                try
                {
                    ComboGridCell cell = _menuGridCells[_row + i, _coloumn];
                }
                catch
                {
                    countExcessCells += 1;
                }
            }
        }
        else
        {
            for (int i = 0; i < comboToHighlight.buttonSequence.Length; i++)
            {
                try
                {
                    ComboGridCell cell = _menuGridCells[_row, _coloumn + i];
                }
                catch
                {
                    countExcessCells += 1;
                }
            }
        }

        return countExcessCells;
    }

    private void AdjustRotationPosition(int excessCells)
    {
        if (!_rotated)
        {
            _coloumn -= excessCells;
        }
        else
        {
            _row -= excessCells;
        }
    }

    private void DisableScrollMenu()
    {
        Debug.Log("Trying to disable ScrollMenu");

        GameObject scrollBar = GameObject.FindGameObjectWithTag("ScrollBar");
        scrollBar.GetComponent<Scrollbar>().interactable = false;

        GameObject scrollRect = GameObject.FindGameObjectWithTag("ScrollList");
        scrollRect.GetComponent<Image>().GetComponent<ScrollRect>().enabled = false;

        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].GetComponent<SingleComboButton>().DisableButton();
        }
    }

    private void EnableScrollMenu()
    {
        Debug.Log("Trying to enable ScrollMenu");

        GameObject scrollBar = GameObject.FindGameObjectWithTag("ScrollBar");
        scrollBar.GetComponent<Scrollbar>().interactable = true;

        GameObject scrollRect = GameObject.FindGameObjectWithTag("ScrollList");
        scrollRect.GetComponent<Image>().GetComponent<ScrollRect>().enabled = true;

        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].GetComponent<SingleComboButton>().EnableButton();
        }
    }

    public void SetCurrentCombo(int buttonPressed)
    {   
        _currentCombo = buttonPressed;

        Debug.Log("RowSaved should be -1 but it is: " + combos[_currentCombo].rowSaved);

        if((combos[_currentCombo].rowSaved == -1) && (combos[_currentCombo].coloumnSaved == -1))
        {
            _scrollToGrid = true;
            Highlight();
            DisableScrollMenu();
        }
        else
        {
            Debug.Log("Trying to delete");
            GridDeletion();
        }
    }
}