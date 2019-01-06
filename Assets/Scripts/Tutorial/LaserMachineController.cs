using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMachineController : MonoBehaviour {

    private LaserMachineScientist _scientist;
    private LaserMachineLaser _laser;
    private LaserMachineDoor _entranceDoor;
    private LaserMachineDoor _exitDoor;
    private BoxCollider2D _activeCollider;
    private AudioSource[] _laserSounds;

    public bool active = false;
    public bool defeated = false;
    public bool laserOn = false;
    private float _startSpeed;

    private float _lastTimeHit;
    public float timeToDefeat;
    public float timeDoor;
    public float timeScientist;
    public float timeLaser;

    private void Awake() {
        _scientist = GetComponentInChildren<LaserMachineScientist>();
        _laser = GetComponentInChildren<LaserMachineLaser>();
        _entranceDoor = transform.Find("EntranceDoor").GetComponent<LaserMachineDoor>();
        _exitDoor = transform.Find("ExitDoor").GetComponent<LaserMachineDoor>();
        _activeCollider = GetComponent<BoxCollider2D>();
        _startSpeed = _laser.moveSpeed;
        _laserSounds = GetComponentsInChildren<AudioSource>();
    }

    void Update () {
        // if is active
		if(!defeated && laserOn) {

            // normal behaviour
            if (_laser.endStop) {
                if (_laser.right) {
                    _laser.Left();
                } else {
                    _laser.Right();
                }
                _laser.SetSpeed(_laser.moveSpeed + 1);
            }

            // defeated
            if(Time.timeSinceLevelLoad - _lastTimeHit > timeToDefeat) {
                Defeated();
            }
        }

        if (_laser.hit) {
            LaserHit();
        }
	}

    private void LaserHit() {
        if (Time.timeSinceLevelLoad - _lastTimeHit > 0.5f) {
            _laser.hit = false;
            _lastTimeHit = Time.timeSinceLevelLoad;
            _laser.SetSpeed(_startSpeed);
        }
    }

    private void Defeated() {
        defeated = true;
        _scientist.Angry();
        _entranceDoor.Open();
        _exitDoor.Open();
        _laser.Off();
        _laser.Stop();
        laserOn = false;
        _laserSounds[0].Stop();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(!active && other.tag == "Player") {
            active = true;
            Invoke("CloseDoor",timeDoor);
            Invoke("MoveScientist",timeScientist);
            Invoke("ActivateLaser",timeLaser);
        }
    }

    private void CloseDoor() {
        _entranceDoor.Close();
    }

    private void MoveScientist() {
        _scientist.Go();
    }

    private void ActivateLaser() {
        _laser.On();
        _laserSounds[1].Play();
        _laserSounds[0].PlayDelayed(0.35f);
        Invoke("AfterActivateLaser",1f);
    }

    private void AfterActivateLaser() {
        laserOn = true;
        _lastTimeHit = Time.timeSinceLevelLoad;
    }
}
