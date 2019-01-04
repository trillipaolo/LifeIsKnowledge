using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour {

    [Header("Joel Reference")]
    public GameObject target;

    //Safe Areas Components
    private Collider2D _collider;

    //Safe Areas Properties
    private bool _isColliding;

    //Input Booleans
    private bool _stickUp = false;

    public void SetIsColliding(bool isColliding)
    {
        _isColliding = isColliding;
    }

	// Use this for initialization
	void Start () {
        //Initialize Collider Properties
        _isColliding = false;
        _collider = transform.Find("ColliderArea").GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (_isColliding)
        {
            EnableInteraction();
        }

        if(Input.GetKeyDown(KeyCode.H))
        {
            DisableMeditating();
        }
	}

    private void EnableInteraction()
    {
        bool _upInputButton = Input.GetButtonDown("Teleport");
        bool _upInputAxis = Input.GetAxis("Teleport") > 0;

        if ((_upInputButton || _upInputAxis) && !_stickUp)
        {
            OpenSafeAreaMenu();
        }

        _stickUp = (_upInputButton || _upInputAxis);
    }
    
    private void OpenSafeAreaMenu()
    {
        DisableJoel();
        target.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
        Time.timeScale = 0f;
        transform.Find("Menu").gameObject.SetActive(true);
    }

    public void CloseSafeAreaMenu()
    {
        transform.Find("Menu").gameObject.SetActive(false);
        Time.timeScale = 1f;
        target.GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;
        EnableJoel();
    }

    private void EnableMeditating()
    {
        DisableJoel();
        target.GetComponent<Animator>().SetBool("isMeditating", true);
    }

    private void DisableMeditating()
    {
        _isColliding = false;

        EnableJoel();
        target.GetComponent<Animator>().SetBool("isMeditating", false);
    }

    private void DisableJoel()
    {
        target.GetComponent<PlayerInput>().DisableInput();
        target.GetComponent<PlayerPhysics>().DisableMovement();
    }

    private void EnableJoel()
    {
        target.GetComponent<PlayerInput>().EnableInput();
        target.GetComponent<PlayerPhysics>().EnableMovement();
    }
    
}
