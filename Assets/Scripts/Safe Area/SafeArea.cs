using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour {

    [Header("Joel Reference")]
    public GameObject target;

    [Header("HUD Reference for disabling")]
    public Canvas canvas;
    public Camera minimap;

    [Header("RespawnPoint reference")]
    public JoelRespawn joelRespawn;

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
        Debug.Log("Colliding: " + _isColliding);
		if (_isColliding)
        {
            EnableInteraction();
        }
	}

    private void EnableInteraction()
    {
        bool _upInputButton = Input.GetButtonDown("Teleport");
        bool _upInputAxis = Input.GetAxis("Teleport") > 0.75f;

        if ((_upInputButton || _upInputAxis) && !_stickUp)
        {
            SaveJoelRespawnPoint();
            OpenSafeAreaMenu();
        }

        _stickUp = (_upInputButton || _upInputAxis);
    }
    
    private void OpenSafeAreaMenu()
    {
        target.GetComponent<ComboMenu>()._menu = true;
        canvas.enabled = false;
        minimap.enabled = false;
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
        minimap.enabled = true;
        canvas.enabled = true;
        target.GetComponent<ComboMenu>()._menu = false;
    }

    private void SaveJoelRespawnPoint()
    {
        joelRespawn.SetRespawnPoint(transform);
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
