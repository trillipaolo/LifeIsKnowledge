using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMachineScientist : MonoBehaviour {

    public bool goTo;

    public Vector2 hidePosition;
    public Vector2 shownPositon;
    public float speed;
    public bool shown;

    private float _startTime;
    private float _distanceStartEnd;

    private Animator _animator;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _startTime = Time.timeSinceLevelLoad;
        _distanceStartEnd = Vector2.Distance(hidePosition,shownPositon);
    }

    private void Update() {
        if (shown) {
            float distCovered = (Time.timeSinceLevelLoad - _startTime) * speed;
            float fracJourney = distCovered / _distanceStartEnd;
            transform.localPosition = Vector2.Lerp(hidePosition,shownPositon,fracJourney);
        }

        if (goTo) {
            Go();
            goTo = false;
        }
    }

    public void Go() {
        _startTime = Time.timeSinceLevelLoad;
        shown = true;
    }

    public void Angry() {
        _animator.SetTrigger("Angry");
    }
}
