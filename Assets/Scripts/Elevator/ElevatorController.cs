using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ElevatorController : MonoBehaviour {

    public float speed;

    public Vector2 upPosition;
    public Vector2 downPosition;
    public float waitingTime;

    private Vector2 _startPosition;
    private Vector2 _endPositon;
    private float _startTime;
    private float _distanceStartEnd;

    [SerializeField]
    private bool _isUp;
    [SerializeField]
    private bool _isDown;
    [SerializeField]
    private bool _isMoving;
    [SerializeField]
    private bool _withJoel;

    private Transform _chassis;
    [SerializeField]
    private ElevatorCall _callUp;
    [SerializeField]
    private ElevatorCall _callDown;
    [SerializeField]
    private GameObject _fakeJoel;
    [SerializeField]
    private GameObject _joel;
    [SerializeField]
    private CameraFollow _mainCamera;

    AudioManager audioManager;
    public string elevatorMovingSound = "ElevatorMoving";
    public string elevatorArrivedSound = "ElevatorArrived";

    private void Awake() {
        _chassis = transform.Find("ElevatorChassis");
        _callUp = transform.Find("ColliderUp").GetComponent<ElevatorCall>();
        _callDown = transform.Find("ColliderDown").GetComponent<ElevatorCall>();
        _fakeJoel = _chassis.Find("FakeJoel").gameObject;
        audioManager = AudioManager.instance;
    }

    private void Start() {
        _joel = GameObject.FindGameObjectsWithTag("Player")[0].transform.root.gameObject;
        _mainCamera = GameObject.Find("Main Camera").GetComponent<CameraFollow>();

        if((Vector2)_chassis.localPosition == upPosition) {
            Up();
        } else if((Vector2)_chassis.localPosition == downPosition) {
            Down();
        } else {
            if(Vector2.Distance(_chassis.localPosition, upPosition) < Vector2.Distance(_chassis.localPosition,downPosition)) {
                MovingUp();
            } else {
                MovingDown();
            }
        }
    }

    private void Update () {
        if (_isMoving) {
            Move();
            if ((Vector2)_chassis.localPosition == upPosition) {
                Up();
            } else if ((Vector2)_chassis.localPosition == downPosition) {
                Down();
            }
        } else if (_isUp) {
            if (_callUp.HasCalled()) {
                MovingJoelDown();
            } else if (_callDown.HasCalled()) {
                MovingDown();
            } else if (_withJoel) {
                ReleaseJoelControl();
            }
        } else if (_isDown) {
            if (_callUp.HasCalled()) {
                MovingUp();
            } else if (_callDown.HasCalled()) {
                MovingJoelUp();
            } else if (_withJoel) {
                ReleaseJoelControl();
            }
        }
	}

    private void Move() {
        float distCovered = (Time.timeSinceLevelLoad - _startTime) * speed;
        float fracJourney = distCovered / _distanceStartEnd;
        _chassis.localPosition = Vector2.Lerp(_startPosition,_endPositon,fracJourney);
    }

    private void MovingJoelUp() {
        Stop();
        TakeJoelControl();

        Invoke("MovingUp", waitingTime);
    }

    private void MovingJoelDown() {
        Stop();
        TakeJoelControl();

        Invoke("MovingDown", waitingTime);
    }

    private void TakeJoelControl() {
        _joel.SetActive(false);
        _fakeJoel.SetActive(true);
        _mainCamera.target = _fakeJoel.GetComponent<Controller2D>();
        _withJoel = true;

        PlayerPhysics _temp = _joel.GetComponent<PlayerPhysics>();
        _temp.DisableMovement();
        _temp.velocity = Vector2.zero;
        
        // To avoid strange behaviour with colliders
        _callDown.ResetCalled();
        _callUp.ResetCalled();
        try{
            _callDown.GetComponent<ShowInteractButton>().HideButton();
        } catch(Exception e) { }
        try {
            _callUp.GetComponent<ShowInteractButton>().HideButton();
        } catch (Exception e) { }
    }

    private void ReleaseJoelControl() {
        _joel.transform.position = _fakeJoel.transform.position;
        _joel.SetActive(true);
        _fakeJoel.SetActive(false);
        _mainCamera.target = _joel.GetComponent<Controller2D>();
        _withJoel = false;

        PlayerPhysics _temp = _joel.GetComponent<PlayerPhysics>();
        _temp.EnableMovement();
        _temp.velocity = Vector2.zero;
    }

    private void MovingUp() {
        Moving();
        _startTime = Time.timeSinceLevelLoad;
        _startPosition = _chassis.localPosition;
        _endPositon = upPosition;
        _distanceStartEnd = Vector2.Distance(_startPosition,_endPositon);
    }

    private void MovingDown() {
        Moving();
        _startTime = Time.timeSinceLevelLoad;
        _startPosition = _chassis.localPosition;
        _endPositon = downPosition;
        _distanceStartEnd = Vector2.Distance(_startPosition,_endPositon);
    }

    private void Up() {
        _isUp = true;
        _isDown = false;
        _isMoving = false;
        Debug.Log("SONO UP");
        audioManager.Stop(elevatorMovingSound);
        audioManager.Play(elevatorArrivedSound);
    }

    private void Down() {
        _isUp = false;
        _isDown = true;
        _isMoving = false;
        Debug.Log("SONO DOWN");

        audioManager.Stop(elevatorMovingSound);
        audioManager.Play(elevatorArrivedSound);
    }

    private void Moving() {
        _isUp = false;
        _isDown = false;
        _isMoving = true;
        audioManager.Play(elevatorMovingSound);
    }

    private void Stop() {
        _isUp = false;
        _isDown = false;
        _isMoving = false;
        Debug.Log("SONO FERMO");
    }
}
