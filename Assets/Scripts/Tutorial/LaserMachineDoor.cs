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

    AudioManager audioManager;
    public string doorLoopSound = "DoorLoop";
    public string doorEndSound = "DoorEndRun";
    private float prevFracJourney=0;
    private float edgeBarrier = 0.9f;
    private bool isInit = false;

    private void Awake() {
        _startTime = Time.timeSinceLevelLoad;
        _startPosition = open ? openPosition : closePosition;
        _distanceStartEnd = Vector2.Distance(openPosition,closePosition);
        audioManager = AudioManager.instance;
    }

    private void Update () {
        float distCovered = (Time.timeSinceLevelLoad - _startTime) * (open ? openSpeed : closeSpeed);
        float fracJourney = distCovered / _distanceStartEnd;
        transform.localPosition = Vector2.Lerp(_startPosition,(open ? openPosition : closePosition),fracJourney);
        
        if (prevFracJourney < edgeBarrier && fracJourney >= edgeBarrier)
        {
            if (isInit)
            {
                audioManager.Stop(doorLoopSound);
                audioManager.Play(doorEndSound);
                            }
            isInit = true;
        }

        if (openDoor) {
            Open();
            openDoor = false;
            Debug.Log("openDoor");
        }
        if (closeDoor) {
            Close();
            closeDoor = false;
            Debug.Log("closeDoor");
        }
        prevFracJourney = fracJourney;
    }

    public void Open() {
        _startTime = Time.timeSinceLevelLoad;
        _startPosition = transform.localPosition;
        _distanceStartEnd = Vector2.Distance(_startPosition,openPosition);
        open = true;
        audioManager.Play(doorLoopSound);
    }

    public void Close() {
        _startTime = Time.timeSinceLevelLoad;
        _startPosition = transform.localPosition;
        _distanceStartEnd = Vector2.Distance(_startPosition,closePosition);
        open = false;
        audioManager.Play(doorLoopSound);

    }
}
