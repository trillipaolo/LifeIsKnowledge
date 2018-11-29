using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class JoelAttack : MonoBehaviour {

    private Animator _animator;
    private BoxCollider2D[] _attackColliders = new BoxCollider2D[6];

    private bool _inputActive = true;
    private bool _topInput = false;
    private bool _middleInput = false;
    private bool _downInput = false;
    private bool _lastTopInput = false;
    private bool _lastMiddleInput = false;
    private bool _lastDownInput = false;
    private bool _currentTopInput = false;
    private bool _currentMiddleInput = false;
    private bool _currentDownInput = false;

    public LayerMask attackLayerMask;
    private ContactFilter2D _attackContactFilter;
    private int _hitId = 0;

    void Awake() {
        _animator = GetComponent<Animator>();
        InitializeAttackColliders();
        InitializeAttackContactFilter();
    }

    private void InitializeAttackColliders() {
        Transform _colliders = this.transform.Find("Colliders");
        _attackColliders[0] = _colliders.Find("FrontTop").GetComponent<BoxCollider2D>();
        _attackColliders[1] = _colliders.Find("FrontMiddle").GetComponent<BoxCollider2D>();
        _attackColliders[2] = _colliders.Find("FrontDown").GetComponent<BoxCollider2D>();
        _attackColliders[3] = _colliders.Find("BackTop").GetComponent<BoxCollider2D>();
        _attackColliders[4] = _colliders.Find("BackMiddle").GetComponent<BoxCollider2D>();
        _attackColliders[5] = _colliders.Find("BackDown").GetComponent<BoxCollider2D>();
    }

    private void InitializeAttackContactFilter() {
        _attackContactFilter = new ContactFilter2D();
        _attackContactFilter.useTriggers = true;
        _attackContactFilter.useLayerMask = true;
        _attackContactFilter.SetLayerMask(attackLayerMask);
    }

    void Update () {
        GetInput();
        UpdateAnimatorParameters();
        UpdateInputVariables();
	}

    private void GetInput() {
        _currentTopInput = Input.GetAxis("AttackTop") > 0;
        _currentMiddleInput = Input.GetAxis("AttackMiddle") > 0;
        _currentDownInput = Input.GetAxis("AttackDown") > 0;

        _topInput = (_lastTopInput != _currentTopInput) ? _currentTopInput : false;
        _middleInput = (_lastMiddleInput != _currentMiddleInput) ? _currentMiddleInput : false;
        _downInput = (_lastDownInput != _currentDownInput) ? _currentDownInput : false;
    }

    private void UpdateAnimatorParameters() {
        if (_inputActive) {
            if(_topInput || _middleInput || _downInput) {
                _animator.SetBool("AttackTop",_topInput);
                _animator.SetBool("AttackMiddle",_middleInput);
                _animator.SetBool("AttackDown",_downInput);
            }
        }
    }

    private void UpdateInputVariables() {
        _lastTopInput = _currentTopInput;
        _lastMiddleInput = _currentMiddleInput;
        _lastDownInput = _currentDownInput;
    }

    private void ActivateInput() {
        _downInput = true;
        ResetAnimatorParameters();
    }

    private void DeactivateInput() {
        _downInput = false;
        ResetAnimatorParameters();
    }

    private void ResetAnimatorParameters() {
        _animator.SetBool("AttackTop",false);
        _animator.SetBool("AttackMiddle",false);
        _animator.SetBool("AttackDown",false);
    }

    private void Attack(string colliderPositionString) {
        bool[] colliderPositions = EnumColliderPosition.StringToArray(colliderPositionString);

        for (int i = 0; i < _attackColliders.Length; i++) {
            if (colliderPositions[i]) {
                Collider2D[] enemiesCollider = new Collider2D[100];
                int _enemyNumber = _attackColliders[i].OverlapCollider(_attackContactFilter,enemiesCollider);
                for (int j = 0; j < _enemyNumber; j++) {
                    enemiesCollider[j].GetComponent<EnemyBehaviour>().TakeDamage(_hitId,100);
                }
            }
        }
        UpdateHitId();
    }

    private int UpdateHitId() {
        _hitId++;
        if (_hitId > 2000000000) {
            _hitId = 0;
        }

        return _hitId;
    }
}
