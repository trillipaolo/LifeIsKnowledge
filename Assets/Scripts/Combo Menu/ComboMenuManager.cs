using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ComboMenuManager : MonoBehaviour {

    public static ComboMenuManager Instance { get; private set; }

    public Transform mainCamera;

    [Header("Scriptable Objects: All combos available to the Player")]
    public Combo[] combos;

    [Header("Scrolling Menu Prefab")]
    public GameObject menuButton;

    [Header("Grid Menu Properties")]
    public GameObject menuCell;
    public int rows;
    public int coloumns;
    [Tooltip("Offset between each cells in the grid - X Axis")]
    public float xOffset;
    [Tooltip("Offset between each cells in the grid - Y Axis")]
    public float yOffset;
    [Tooltip("Offset w.r.t the MainCamera - X Axis")]
    public float xGlobalOffset;
    [Tooltip("Offset w.r.t the MainCamera - Y Axis")]
    public float yGlobalOffset;

    [Header("Activate/Deactivate ScrollBar in the ScrollMenu")]
    public bool hideScrollBar;
    public GameObject scrollBar;
    public GameObject slidingArea;
    public GameObject handle;


    [Header("Reset Grid Status")]
    public bool reset;

    [Header("Grid Sprites for Combos")]
    public Sprite emptyCellWhite;
    public Sprite emptyCell;
    public Sprite initialCell;
    public Sprite initialCellRotated;
    public Sprite finalCell;
    public Sprite finalCellRotated;
    public Sprite intermediateCell;
    public Sprite intermediateCellRotated;

    [Header("Combos Available to Joel in-game")]
    public JoelCombos joelCombos;

    [Header("References: Safe Area interaction")]
    public GameObject meditationButton;
    public GameObject blurBackground;
    public bool _calledByButton = false;
    //To decouple the B button between normal menu and meditation menu
    private bool _backToScroll;

    [Header("Back Button Sprites")]
    public Sprite backButtonStart;
    public Sprite backButtonB;

    [Header("Canvas Button References")]
    public GameObject backButton;
    public GameObject changeMenuButton;

    //List of Buttons in the scrolling menu: each button refers to a combo
    [Header("ScrollMenu Button List")]
    public List<GameObject> _menuButtons;
    private int _menuButtonsIndex;
    private bool _updateSelectedButton;

    //Combo Selected in the scrolling menu
    private int _currentCombo;

    //List of cells in the grid menu: each cells can contain one button of a combo
    public ComboGridCell[,] _menuGridCells;

    //Current position and state in the grid
    private int _row;
    private int _coloumn;
    private bool _rotated;
    private bool _scrollToGrid;

    //JoyPad Booleans
    private bool _dpadDown = false;
    private bool _dpadUp = false;
    private bool _dpadLeft = false;
    private bool _dpadRight = false;
    private bool _rotationX = false;
    private bool _insertionA = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {   
        //Manage differences between calling the Menu in the game and in the Safe Areas
        if (_calledByButton)
        {
            backButton.GetComponent<SpriteRenderer>().sprite = backButtonB;
            blurBackground.SetActive(false);
            changeMenuButton.SetActive(false);
            menuCell.GetComponent<SpriteRenderer>().sprite = emptyCellWhite;
        }
        else
        {
            backButton.GetComponent<SpriteRenderer>().sprite = backButtonStart;
            blurBackground.SetActive(true);
            changeMenuButton.SetActive(true);
            menuCell.GetComponent<SpriteRenderer>().sprite = emptyCell;
        }

        //The Menu starts with the Scrolling Menu active
        _scrollToGrid = false;

        //No Combo selected at the beginning
        _currentCombo = -1;

        //Combo is not rotated at the beginning (which means horizontal)
        _rotated = false;

        //Starting point of Grid Position is element 0,0
        _row = 0;
        _coloumn = 0;

        //Initialize menuButtons index and flag
        _menuButtonsIndex = 0;
        _updateSelectedButton = false;

        //Initialize Menus
        InitializeScrollingMenu();
        InitializeGridMenu();

        //Reset combo not unlocked (prevent inconsistent state for Scriptable Objects)
        SoftResetGrid();

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

    void OnDisable()
    {   
        //Destroy every object of the menu

        foreach (GameObject g in _menuButtons)
        {
            Destroy(g);
        }

        _menuButtons = new List<GameObject>();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < coloumns; j++)
            {
                Destroy(_menuGridCells[i, j].gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update () {
        _backToScroll = false;

        if (_calledByButton)
        {
            if (_scrollToGrid)
            {
                //What to do when we're in the grid menu


                //Get back to Scroll menu pressing Backspace
                if (Input.GetButtonDown("BackToScroll"))
                {
                    StopHighlight();
                    _scrollToGrid = false;
                    _rotated = false;
                    _currentCombo = -1;
                    _row = 0;
                    _coloumn = 0;

                    EnableScrollMenu();
                    _backToScroll = true;

                    return;
                }

                Combo currentCombo = combos[_currentCombo];

                //Movement in the grid
                GridMovement();

                //Rotation in the grid
                GridRotation();

                //Insertion in the grid
                if (Input.GetButtonDown("GridInsertion") || Input.GetAxis("GridInsertion") > 0)
                {
                    GridInsertion();
                }
            }
            else
            {
                //What to do when we're in the scroll menu

                //Manual select of menuButtons (Combos in the Scroll menu)
                Debug.Log("You can select one combo now");
                ScrollSelection();
            }
        }
	}

    private void LateUpdate()
    {
        if (!_backToScroll && _calledByButton && Input.GetButtonDown("BackToScroll"))
        {
            meditationButton.GetComponent<MeditationButton>().CloseMenu();
        }
    }

    private void SoftResetGrid()
    {
        for (int i = 0; i < combos.Length; i++)
        {   
            if (combos[i].unlocked == false)
            {
                combos[i].coloumnSaved = -1;
                combos[i].rowSaved = -1;
                combos[i].rotatedSaved = false;
            }
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
            if (combos[i].unlocked)
            {
                GameObject button = Instantiate(menuButton) as GameObject;
                button.SetActive(true);

                button.GetComponent<SingleComboButton>().SetIndex(i);
                button.GetComponent<SingleComboButton>().SetComboName(combos[i].comboName);
                button.GetComponent<SingleComboButton>().SetImage(combos[i].comboSprite);
                button.GetComponent<SingleComboButton>().SetKeyFrame(combos[i].cooldownImage);
                button.GetComponent<SingleComboButton>().SetGlowEffect(combos[i].highlightedComboSprite);

                button.transform.SetParent(menuButton.transform.parent, false);

                _menuButtons.Add(button);
            }
        }


        //Selecting the first button of the List if there is at least one
        if (_menuButtons.Count > 0 && _calledByButton)
        {
            UpdateButtonSelected(-1);
        }

        //Hide the scrollbar if hideScrollBar = true
        if (hideScrollBar)
        {
            handle.SetActive(false);
            slidingArea.SetActive(false);

            Image scrollBarImage = scrollBar.GetComponent<Image>();
            scrollBarImage.enabled = false;
        }

    }

    private void InitializeGridMenu()
    {
        _menuGridCells = new ComboGridCell[rows, coloumns];

        Vector3 cellsGlobalOffset = new Vector3(xGlobalOffset, yGlobalOffset, 0);

        Vector3 cellOffset = mainCamera.position - cellsGlobalOffset;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < coloumns; j++)
            {
                GameObject newGridCell = Instantiate(menuCell, cellOffset + Vector3.forward*10, new Quaternion(0, 0, 0, 0));
                newGridCell.SetActive(true);
                cellOffset += new Vector3(-xOffset, 0, 0);

                newGridCell.transform.SetParent(menuCell.transform.parent, false);

                _menuGridCells[i, j] = newGridCell.GetComponent<ComboGridCell>();
            }

            cellOffset += new Vector3(xOffset * coloumns, yOffset, 0);
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

            Debug.Log("Time scale value_ " + Time.timeScale);
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

    private void ScrollSelection()
    {   
        ScrollMenuUp();

        ScrollMenuDown();
        
        PressMenuButton();
    }

    private void ScrollMenuUp()
    {
        bool _downInputButton = Input.GetButtonDown("GridUp");
        bool _downInputAxis = Input.GetAxis("GridUp") > 0;

        if ((_downInputButton || _downInputAxis) && !_dpadUp && (_menuButtonsIndex > 0))
        {   
            _menuButtonsIndex -= 1;
            _updateSelectedButton = true;
            Debug.Log("New _menuButtonsIndex value: " + _menuButtonsIndex);
        }

        _dpadUp = (_downInputButton || _downInputAxis);

        if (_updateSelectedButton)
        {
            UpdateButtonSelected(_menuButtonsIndex + 1);

            _updateSelectedButton = false;
        }
    }

    private void ScrollMenuDown()
    {
        bool _downInputButton = Input.GetButtonDown("GridDown");
        bool _downInputAxis = Input.GetAxis("GridDown") > 0;

        if ((_downInputButton || _downInputAxis) && !_dpadDown && (_menuButtonsIndex < _menuButtons.Count - 1))
        {
            _menuButtonsIndex += 1;
            _updateSelectedButton = true;
            Debug.Log("New _menuButtonsIndex value: " + _menuButtonsIndex);
        }

        _dpadDown = (_downInputButton || _downInputAxis);

        if (_updateSelectedButton)
        {
            UpdateButtonSelected(_menuButtonsIndex - 1);

            _updateSelectedButton = false;
        }
    }

    //Call this method with parameter -1 in order to highlight only the current button
    //without "de"-highlighting the previous one
    private void UpdateButtonSelected(int previousButtonIndex)
    {
        SingleComboButton currentComboButton = _menuButtons[_menuButtonsIndex].GetComponent<SingleComboButton>();
        currentComboButton.StartHighlight();

        if (previousButtonIndex != -1)
        {
            SingleComboButton previousComboButton = _menuButtons[previousButtonIndex].GetComponent<SingleComboButton>();
            previousComboButton.StopHighlight();
        }
    }

    private void PressMenuButton()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Button currentButton = _menuButtons[_menuButtonsIndex].GetComponent<Button>();
            currentButton.onClick.Invoke();
        }
    }

    private void GridDown()
    {
        bool _downInputButton = Input.GetButtonDown("GridDown");
        bool _downInputAxis = Input.GetAxis("GridDown") > 0;

        if ((_downInputButton || _downInputAxis) && !_dpadDown)
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

        _dpadDown = (_downInputButton || _downInputAxis);
    }

    private void GridUp()
    {
        bool _downInputButton = Input.GetButtonDown("GridUp");
        bool _downInputAxis = Input.GetAxis("GridUp") > 0;

        if ((_downInputButton || _downInputAxis) && !_dpadUp)
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
        

        _dpadUp = (_downInputButton || _downInputAxis);
    }

    private void GridLeft()
    {
        bool _downInputButton = Input.GetButtonDown("GridLeft");
        bool _downInputAxis = Input.GetAxis("GridLeft") > 0;

        if ((_downInputButton || _downInputAxis) && !_dpadLeft)
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

        _dpadLeft = (_downInputButton || _downInputAxis);
    }

    private void GridRight()
    {
        bool _downInputButton = Input.GetButtonDown("GridRight");
        bool _downInputAxis = Input.GetAxis("GridRight") > 0;

        if ((_downInputButton || _downInputAxis) && !_dpadRight)
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

        _dpadRight = _downInputButton || _downInputAxis;
    }

    private void GridMovement()
    {

        GridDown();

        GridUp();

        GridLeft();

        GridRight();
    }

    private void GridRotation()
    {
        bool _rotationInput = Input.GetButtonDown("GridRotation");

        if (_rotationInput && !_rotationX)
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

        _rotationX = _rotationInput;
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
        _row = 0;
        _coloumn = 0;
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
                    _menuGridCells[startingRow + i, startingColoumn].GetComponent<SpriteRenderer>().sprite = emptyCellWhite;

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
                    _menuGridCells[startingRow, startingColoumn + i].GetComponent<SpriteRenderer>().sprite = emptyCellWhite;

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
        if (!hideScrollBar)
        {
            scrollBar.GetComponent<Scrollbar>().interactable = false;
        }

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

        if (!hideScrollBar)
        {
            scrollBar.GetComponent<Scrollbar>().interactable = true;
        }

        GameObject scrollRect = GameObject.FindGameObjectWithTag("ScrollList");
        scrollRect.GetComponent<Image>().GetComponent<ScrollRect>().enabled = true;

        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].GetComponent<SingleComboButton>().EnableButton();
        }
    }

    public void SetCombosChosen()
    {
        int j = 0;

        for(int i = 0; i < combos.Length; i++) {
            if (combos[i].rowSaved != -1)
            {
                j++;
            }
        }

        joelCombos.SetNumberCombos(j);

        j = 0;
        for (int i = 0; i < combos.Length; i++)
        {
            if (combos[i].rowSaved != -1)
            {
                joelCombos.combos[j] = combos[i];
                j++;
            }
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
