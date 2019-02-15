using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoController : MonoBehaviour {

    private LaserMachineScientist _scientist;
    private LaserMachineDoor _entranceDoor;
    private LaserMachineDoor _exitDoor;
    private BoxCollider2D _activeCollider;
    private AudioSource[] _laserSounds;

    public SpriteRenderer laserTop1;
    public SpriteRenderer laserTop2;
    public Transform laserBottom1;
    public Transform laserBottom2;
    
    public bool active = false;
    public bool defeated = false;
    public bool laserOn = false;
    private float _startSpeed;

    private float _lastTimeHit;
    public float timeToDefeat;
    public float timeDoor;
    public float timeScientist;
    public float timeLaser;

    public float offTop;
    public float onTop;
    public float bottomDegree = 30f;
    public float bottomSpeed = 30f;
    
    private Quaternion _endAngle;

    private AudioManager _audioManager;

    private void Awake() {
        _scientist = GetComponentInChildren<LaserMachineScientist>();
        _entranceDoor = transform.Find("EntranceDoor").GetComponent<LaserMachineDoor>();
        _exitDoor = transform.Find("ExitDoor").GetComponent<LaserMachineDoor>();
        _activeCollider = GetComponent<BoxCollider2D>();
        _laserSounds = GetComponentsInChildren<AudioSource>();
    }

    private void Start() {
        _audioManager = AudioManager.instance;

        _endAngle = Quaternion.Euler(0,0,bottomDegree);
        _top = true;
        _startTop = Time.timeSinceLevelLoad;
    }

    private void Update() {
        ActivateTop();

        RotateBottom();
    }

    private float _startTop;
    private bool _top;
    public Color topColor;

    private void ActivateTop() {
        float _time = Time.timeSinceLevelLoad - _startTop;
        if (_top) {
            if(_time < onTop) {
                laserTop1.color = topColor;
            }
            else if(_time - onTop < offTop) {
                laserTop1.color = Color.clear;
            }
            else {
                _top = !_top;
                _startTop = Time.timeSinceLevelLoad;
            }
        } else {
            if (_time < onTop) {
                laserTop2.color = topColor;
            } else if (_time - onTop < offTop) {
                laserTop2.color = Color.clear;
            } else {
                _top = !_top;
                _startTop = Time.timeSinceLevelLoad;
            }
        }
    }

    private void CurrentTop() {
        float _time = Time.timeSinceLevelLoad - _startTop;

        if (_time < onTop) {
            laserTop1.color = topColor;
            laserTop2.color = topColor;
        } else if (_time - onTop < offTop) {
            laserTop1.color = Color.clear;
            laserTop2.color = Color.clear;
        } else {
            _startTop = Time.timeSinceLevelLoad;
        }
    }

    private void RotateBottom() {
        laserBottom1.rotation = Quaternion.RotateTowards(laserBottom1.rotation, _endAngle, bottomSpeed * Time.deltaTime);
        laserBottom2.rotation = Quaternion.RotateTowards(laserBottom2.rotation, Quaternion.Inverse(_endAngle),bottomSpeed * Time.deltaTime);
        if(laserBottom1.rotation == _endAngle) {
            _endAngle = Quaternion.Inverse(_endAngle);
        }
    }
}
