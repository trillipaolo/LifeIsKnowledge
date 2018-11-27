using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    private Animator _animator;
    private Transform _tr;
    private Transform _tr_player;
    private Vector3 _direction = Vector3.zero;
    private Vector3 _diff = Vector3.zero;
    private Vector3 frameMovement = Vector3.zero;

    [Header("Enemy Velocity")]
    public float velocity = 5f;
    [Header("Distance wrt Player (module)")]
    public float dst = 1f;
    public float directionFacing = 1;   //initially turned to right

    //TODO: use raytracing and layers to set right Y coordinate for the enemy

    // Use this for initialization
    void Awake() {
        _tr = GetComponent<Transform>();

        _animator = GetComponentInChildren<Animator>();
        Debug.Log("_animator" + _animator);

        _tr_player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update() {

        _diff = _tr_player.position - _tr.position;
        _direction = _diff.normalized;
        frameMovement = _direction * velocity;
        Vector3 tmpVect = _tr.position + frameMovement * Time.deltaTime;
        directionFacing = Mathf.Sign(frameMovement.x);
        _tr.localScale = new Vector3(directionFacing * Mathf.Abs(_tr.localScale.x), _tr.localScale.y, _tr.localScale.z);



        if (_diff.magnitude >= dst) {
            _tr.position = new Vector3(tmpVect.x, _tr.position.y, _tr.position.z);
            _animator.SetInteger("isAttacking", 0);
            _animator.SetBool("isMoving", true);
        }
        else {
            _animator.SetBool("isMoving", false);
            _animator.SetInteger("isAttacking", Random.Range(1, 3));
        }
    }
}
