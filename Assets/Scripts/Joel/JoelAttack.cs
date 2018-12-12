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

    public BasicAttack basicAttack;
    public JoelCombos joelCombos;
    private float[] _lastTimeUsed;
    private bool _combo1 = false;
    private bool _combo2 = false;

    public LayerMask attackLayerMask;
    private ContactFilter2D _attackContactFilter;
    private int _hitId = 0;

    AudioManager audioManager;
    public string knifeSlashSounds1Coll = "KnifeSlash2";
    public string knifeSlashSounds2Coll = "KnifeSlash1";
    public string knifeSlashSounds3Coll = "KnifeSlash3";


    void Awake() {
        _animator = GetComponent<Animator>();
        InitializeAttackColliders();
        InitializeAttackContactFilter();
        InitializeComboVariables();

        basicAttack.InitializeDamage();
        for(int i = 0; i < joelCombos.combos.Length; i++) {
            joelCombos.combos[i].InitializeDamage();
        }
    }

    private void Start() {
        audioManager = AudioManager.instance;
    }

    private void InitializeAttackColliders() {
        Transform _colliders = this.transform.Find("AttackColliders");
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
        _lastTimeUsed = new float[joelCombos.combos.Length];
        for(int i = 0; i < _lastTimeUsed.Length; i++) {
            _lastTimeUsed[i] = -300;
        }
    }

    void Update () {
        UpdateComboCooldown();
        GetInput();
        SetComboEnum();
        UpdateAnimatorParameters();
        UpdateInputVariables();
	}

    private void UpdateComboCooldown() {
        if (_lastTimeUsed.Length != joelCombos.combos.Length) {
            _lastTimeUsed = new float[joelCombos.combos.Length];
            for (int i = 0; i < _lastTimeUsed.Length; i++) {
                _lastTimeUsed[i] = Time.timeSinceLevelLoad;
            }
        }
    }

    private void GetInput() {
        _currentTopInput = Input.GetAxis("AttackTop") > 0;
        _currentMiddleInput = Input.GetAxis("AttackMiddle") > 0;
        _currentDownInput = Input.GetAxis("AttackDown") > 0;
        _combo1 = Input.GetAxis("Combo1") > 0;
        _combo2 = Input.GetAxis("Combo2") > 0;

        _topInput = (_lastTopInput != _currentTopInput) ? _currentTopInput : false;
        _middleInput = (_lastMiddleInput != _currentMiddleInput) ? _currentMiddleInput : false;
        _downInput = (_lastDownInput != _currentDownInput) ? _currentDownInput : false;

    }

    private void SetComboEnum() {
        if (_combo1 || _combo2) {
            for (int i = 0; i < joelCombos.combos.Length; i++) {
                Combo _currentCombo = joelCombos.combos[i];
                if (_combo1 == _currentCombo.combo1Key && _combo2 == _currentCombo.combo2Key &&
                    _topInput == _currentCombo.topKey && _middleInput == _currentCombo.middleKey && _downInput == _currentCombo.downKey) {
                    if(Time.timeSinceLevelLoad - _lastTimeUsed[i] > _currentCombo.cooldown) {
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
        for (int i = 0; i < joelCombos.combos.Length; i++) {
            if (joelCombos.combos[i].enumCombo == (EnumCombo)_comboEnum) {
                _lastTimeUsed[i] = Time.timeSinceLevelLoad;
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

    private void BasicAttack(AnimationEvent animationData) {
        bool[] _colliderPositions = EnumColliderPosition.StringToArray(animationData.stringParameter);

        float _damage = basicAttack.damage[animationData.intParameter];
        
        if (ActivateColliders(_colliderPositions, _damage)) {
            basicAttack.damage[animationData.intParameter] += 3.0f;
        }

        PlayAttackSound(animationData.stringParameter);
    }

    private void ComboAttack(AnimationEvent animationData) {
        bool[] _colliderPositions = EnumColliderPosition.StringToArray(animationData.stringParameter);
        float _damage = 0;
        float[] _tempDamage = new float[10];
        for (int i = 0; i < joelCombos.combos.Length; i++) {
            if ((int)joelCombos.combos[i].enumCombo == animationData.intParameter) {
                int index = Mathf.FloorToInt(animationData.floatParameter);
                _damage = joelCombos.combos[i].damage[index];
                _tempDamage = joelCombos.combos[i].damage;
            }
        }

        if (ActivateColliders(_colliderPositions,_damage)) {
            for (int i = 0; i < _tempDamage.Length; i++) {
                _tempDamage[i] += 3.0f;
            }
        }
    }

    private bool ActivateColliders(bool[] colliderPositions, float damage) {
        const int MAXCOLLIDERS = 100;
        // activate one collider at time and get collision, if one enemy is already hit don't add anymore and set OneHit false,
        // return true if it hits at least one enemy, false otherwise
        Collider2D[] _enemyColliders = new Collider2D[MAXCOLLIDERS];
        Collider2D[] _colliderToDamage = new Collider2D[MAXCOLLIDERS];
        bool[] _uniqueAttack = new bool[MAXCOLLIDERS];
        int _currLenght = 0;


        for(int i = 0; i < colliderPositions.Length; i++) {
            // for each collider of joel if is activated by the attack..
            if (colliderPositions[i]) {
                // ..take the collisions with enemies
                int _enemyNumber = _attackColliders[i].OverlapCollider(_attackContactFilter,_enemyColliders);

                // for each collider of the enemy..
                for(int j = 0; j < _enemyNumber; j++) {
                    int _enemyId = _enemyColliders[j].transform.parent.GetInstanceID();

                    bool _toAdd = true;
                    bool _unique = true;

                    // check if is already in previous colliders stored
                    for (int k = 0; k < _currLenght; k++) {

                        // if two collider are of the same gameObject..
                        if (_colliderToDamage[k].transform.parent.GetInstanceID() == _enemyId) {

                            // ..and they are two different colliders
                            if (_colliderToDamage[k].offset != _enemyColliders[j].offset) {
                                // set unique to false
                                _uniqueAttack[k] = false;
                                _unique = false;
                            }
                            // ..and it is the same collider
                            else {
                                // set not to add
                                _toAdd = false;
                            }
                        }
                    }

                    if (_toAdd) {
                        _colliderToDamage[_currLenght] = _enemyColliders[j];
                        _uniqueAttack[_currLenght] = _unique;
                        _currLenght++;
                    }
                }
            }
        }

        for (int i = 0; i < _currLenght; i++) {
            if (_colliderToDamage[i].GetComponentInParent<EnemyBehaviour>() != null) {
                _colliderToDamage[i].GetComponentInParent<EnemyBehaviour>().TakeDamage(_colliderToDamage[i],damage,_uniqueAttack[i]);
            }
        }

        return _currLenght > 0;
    }

    /*private void Attack(string colliderPositionString) {
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

        PlayAttackSound(colliderPositionString);
        
        UpdateHitId();
    }*/

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

    void PlayAttackSound(string posStr) {
        int x = Int32.Parse(posStr);
        int sum = 0;

        while (x != 0) {
            sum += x % 10;
            x /= 10;
        }

        if (sum == 1 && posStr != "010000")
            audioManager.Play(knifeSlashSounds1Coll);
        else
        if (sum == 2)
            audioManager.Play(knifeSlashSounds2Coll);
        else
        if (sum == 3)
            audioManager.Play(knifeSlashSounds3Coll);
        else
            Debug.Log("Sound attack not found or mid-mid played!");
    }

    void PlaySoundMidMid() {
        audioManager.Play(knifeSlashSounds1Coll);
    }
}
