using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMachineDoor : MonoBehaviour {

    public bool openDoor;
    public bool closeDoor;

    public Vector2 openPosition;
    public Vector2 closePosition;
    public float openSpeed;
    public float closeSpeed;
    public bool open;

    private float _startTime;
    private Vector2 _startPosition;
    private float _distanceStartEnd;

    private void Awake() {
        _startTime = Time.timeSinceLevelLoad;
        _startPosition = open ? openPosition : closePosition;
        _distanceStartEnd = Vector2.Distance(openPosition,closePosition);
    }

    private void Update () {
        float distCovered = (Time.timeSinceLevelLoad - _startTime) * (open ? openSpeed : closeSpeed);
        float fracJourney = distCovered / _distanceStartEnd;
        transform.localPosition = Vector2.Lerp(_startPosition,(open ? openPosition : closePosition),fracJourney);

        if (openDoor) {
            Open();
            openDoor = false;
        }
        if (closeDoor) {
            Close();
            closeDoor = false;
        }
    }

    public void Open() {
        _startTime = Time.timeSinceLevelLoad;
        _startPosition = transform.localPosition;
        _distanceStartEnd = Vector2.Distance(_startPosition,openPosition);
        open = true;
    }

    public void Close() {
        _startTime = Time.timeSinceLevelLoad;
        _startPosition = transform.localPosition;
        _distanceStartEnd = Vector2.Distance(_startPosition,closePosition);
        open = false;
    }
}
