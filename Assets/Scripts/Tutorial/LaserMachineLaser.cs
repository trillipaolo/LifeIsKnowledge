using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMachineLaser : MonoBehaviour {

    public bool openDoor;
    public bool closeDoor;

    public Vector2 leftPosition;
    public Vector2 rightPosition;
    public float moveSpeed;
    public bool right;
    public bool endStop;
    public int damage;

    public bool on;
    [HideInInspector]
    public bool hit = false;
    private bool _inCollider = false;

    private float _startTime;
    private Vector2 _startPosition;
    private float _distanceStartEnd;

    private Animator _animator;
    private BoxCollider2D _collider;
    private JoelHealth _joel;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();

        _startTime = Time.timeSinceLevelLoad;
        _startPosition = right ? rightPosition : leftPosition;
        _distanceStartEnd = Vector2.Distance(rightPosition,leftPosition);
        _collider.enabled = on;
    }

    private void Start() {
        _joel = GameObject.FindGameObjectsWithTag("Player")[0].transform.root.GetComponentInChildren<JoelHealth>();
    }

    private void Update() {
        Move();
        Active();
        Damage();

        if (openDoor) {
            Left();
            openDoor = false;
        }
        if (closeDoor) {
            Right();
            closeDoor = false;
        }
    }

    private void Move() {
        float distCovered = (Time.timeSinceLevelLoad - _startTime) * moveSpeed;
        float fracJourney = distCovered / _distanceStartEnd;
        transform.localPosition = Vector2.Lerp(_startPosition,(right ? rightPosition : leftPosition),fracJourney);

        endStop = transform.localPosition.x == (right ? rightPosition.x : leftPosition.x) && transform.localPosition.y == (right ? rightPosition.y : leftPosition.y);
    }

    private void Active() {
        if(on && !_collider.enabled) {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("LaserOn")) {
                _collider.enabled = true;
            }
        }
    }

    private void Damage() {
        if (_inCollider) {
            _joel.TakeDamage(damage);
            hit = true;
        } else {
            hit = false;
        }
    }

    public void Left() {
        _startTime = Time.timeSinceLevelLoad;
        _startPosition = transform.localPosition;
        _distanceStartEnd = Vector2.Distance(_startPosition,leftPosition);
        right = false;
    }

    public void Right() {
        _startTime = Time.timeSinceLevelLoad;
        _startPosition = transform.localPosition;
        _distanceStartEnd = Vector2.Distance(_startPosition,rightPosition);
        right = true;
    }

    public void Stop() {
        _startPosition = transform.localPosition;
        _distanceStartEnd = Mathf.Infinity;
    }

    public void SetSpeed(float speed) {
        moveSpeed = speed;
        _startTime = Time.timeSinceLevelLoad;
        _startPosition = transform.localPosition;
        _distanceStartEnd = Vector2.Distance(_startPosition,(right ? rightPosition : leftPosition));
    }

    public void On() {
        on = true;
        _animator.SetBool("On",true);
    }

    public void Off() {
        on = false;
        _animator.SetBool("On",false);
        _collider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            _inCollider = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            _inCollider = false;
        }
    }
}
