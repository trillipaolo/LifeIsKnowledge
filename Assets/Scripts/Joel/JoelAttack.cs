using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    private int _comboEnum = 0;
    private float[] _cooldownCombo = new float[2];

    public JoelCombos joelCombos;
    private Combo[] _combos;
    private float[] _lastTimeUsed;
    private bool _combo1 = false;
    private bool _combo2 = false;

    public LayerMask attackLayerMask;
    private ContactFilter2D _attackContactFilter;
    private int _hitId = 0;

    void Awake() {
        _animator = GetComponent<Animator>();
        InitializeAttackColliders();
        InitializeAttackContactFilter();
        InitializeComboVariables();
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

    private void InitializeComboVariables() {
        _combos = joelCombos.combos.ToArray();

        _lastTimeUsed = new float[_combos.Length];
        for(int i = 0; i < _lastTimeUsed.Length; i++) {
            _lastTimeUsed[i] = -300;
        }
    }

    void Update () {
        GetInput();
        SetComboEnum();
        UpdateAnimatorParameters();
        UpdateInputVariables();
	}

    private void GetInput() {
        _currentTopInput = Input.GetAxis("AttackTop") > 0;
        _currentMiddleInput = Input.GetAxis("AttackMiddle") > 0;
        _currentDownInput = Input.GetAxis("AttackDown") > 0;
        _combo1 = Input.GetAxis("Combo1") > 0;
        _combo2 = Input.GetAxis("Combo2") > 0;
        //_combo1 = Input.GetButtonDown("Combo1");
        //_combo2 = Input.GetButtonDown("Combo2");
        Debug.Log("combo1" + _combo1);
        Debug.Log("combo2" + _combo2);

        _topInput = (_lastTopInput != _currentTopInput) ? _currentTopInput : false;
        _middleInput = (_lastMiddleInput != _currentMiddleInput) ? _currentMiddleInput : false;
        _downInput = (_lastDownInput != _currentDownInput) ? _currentDownInput : false;

    }

    private void SetComboEnum() {
        if (_combo1 || _combo2) {
            for (int i = 0; i < _combos.Length; i++) {
                Combo _currentCombo = _combos[i];
                if (_combo1 == _currentCombo.combo1Key && _combo2 == _currentCombo.combo2Key &&
                    _topInput == _currentCombo.topKey && _middleInput == _currentCombo.middleKey && _downInput == _currentCombo.downKey) {
                    if(Time.time - _lastTimeUsed[i] > _currentCombo.cooldown) {
                        _comboEnum = (int)_currentCombo.enumCombo;
                    }
                    return;
                }
            }
        }
    }

    private void UpdateAnimatorParameters() {
        if (_inputActive) {
            if(_topInput || _middleInput || _downInput) {
                _animator.SetBool("AttackTop",_topInput);
                _animator.SetBool("AttackMiddle",_middleInput);
                _animator.SetBool("AttackDown",_downInput);
                _animator.SetInteger("ComboEnum",_comboEnum);
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

    private void ComboStarted() {
        for (int i = 0; i < _combos.Length; i++) {
            if (_combos[i].enumCombo == (EnumCombo)_comboEnum) {
                _lastTimeUsed[i] = Time.time;
                _comboEnum = 0;
                return;
            }
        }
        _comboEnum = 0;
    }

    private void ResetAnimatorParameters() {
        _animator.SetBool("AttackTop",false);
        _animator.SetBool("AttackMiddle",false);
        _animator.SetBool("AttackDown",false);
        _animator.SetInteger("ComboEnum",0);
    }

    private void Attack(string colliderPositionString) {
        bool[] _colliderPositions = EnumColliderPosition.StringToArray(colliderPositionString);
        int _damage = GetDamage(colliderPositionString);

        for (int i = 0; i < _attackColliders.Length; i++) {
            if (_colliderPositions[i]) {
                Collider2D[] enemiesCollider = new Collider2D[100];
                int _enemyNumber = _attackColliders[i].OverlapCollider(_attackContactFilter,enemiesCollider);
                for (int j = 0; j < _enemyNumber; j++) {
                    enemiesCollider[j].GetComponent<EnemyBehaviour>().TakeDamage(_hitId,_damage);
                }
            }
        }
        UpdateHitId();
    }

    private int GetDamage(string colliderPositionString) {
        int _defaultDamage = 100;
        
        if(colliderPositionString == null) {
            return _defaultDamage;
        }

        string _damageString = colliderPositionString.Substring(EnumColliderPosition.Size());
        if(_damageString == null) {
            return _defaultDamage;
        }

        int _damage = 0;
        if(Int32.TryParse(_damageString, out _damage)) {
            return (_damage >= 0) ? _damage : _defaultDamage;
        } else {
            return _defaultDamage;
        }
    }

    private int UpdateHitId() {
        _hitId++;
        if (_hitId > 2000000000) {
            _hitId = 0;
        }

        return _hitId;
    }
}
