using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoDance : MonoBehaviour {

    private SpriteRenderer _sprite;

    public float startTime;
    public float flipTime;
    private float _startTime;
    
    private void Awake() {
        _sprite = GetComponent<SpriteRenderer>();
        _startTime = Time.timeSinceLevelLoad + startTime;
    }
    
    void Update () {
        float _time = Time.timeSinceLevelLoad;

        if(_time - _startTime > flipTime) {
            _sprite.flipX = !_sprite.flipX;
            _startTime = Time.timeSinceLevelLoad;
        }
    }
}
