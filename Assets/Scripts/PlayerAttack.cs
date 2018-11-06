using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    
    // TODO we can change Physics2D.OverlapBox with Collider2D.OverlapCollider (to have the possibility to use different shapes)

    public Weapon currentWeapon;
    private Vector2[] _collidersCenter;
    private Vector2[] _collidersSize;
    private BinaryTree _comboTree;
    private BinaryTree _currentAttack;

    // there is written the last input arrived into the window in which a player can do an action
    private BinaryTree _nextAttack;
    // it tells is the player is into the window in which the input will be accepted
    private bool _inputActive;
    // tells if the player is ready to play the animation of the next attack
    private bool _readyToAttack;

    private float _damage;
    private int _hitId;
    public LayerMask enemyLayerMask;
    public PlayerMovement playerMovement;
    private Animator _animator;

    private void Awake() {
        // TODO DELETE IT!!!
        InitializeComboTree();


        ImportWeaponParameters();

        _hitId = 0;
        _inputActive = true;
        _readyToAttack = true;
        _animator = gameObject.GetComponent<Animator>();

        //TODO needs to be moved in another part
        FloatingTextController.Initialize();

        Debug.Log(_comboTree.ToString());
        Debug.Log(_currentAttack.value);

        Debug.Log(_currentAttack.left);
        Debug.Log(_currentAttack.right);
    }
	
	void Update () {
        if (_inputActive) {
            if (_readyToAttack) {
                if (Input.GetKeyDown(KeyCode.K)) {

                    Debug.Log("into K-down");

                    PlayAttackAnimation(_currentAttack.left);
                }
                if (Input.GetKeyDown(KeyCode.L)) {
                    PlayAttackAnimation(_currentAttack.right);
                }
            } else {
                if (Input.GetKeyDown(KeyCode.K)) {
                    _nextAttack = _currentAttack.left;
                }
                if (Input.GetKeyDown(KeyCode.L)) {
                    _nextAttack = _currentAttack.right;
                }
            }
        }
    }

    public void Attack(string colliderPositionString) {
        bool[] colliderPositions = EnumColliderPosition.StringToArray(colliderPositionString);
        float directionFacing = playerMovement.directionFacing;

        for(int i = 0; i < _collidersCenter.Length; i++) {
            if (colliderPositions[i]) {
                Vector2 overlapBoxCenter = (new Vector2(_collidersCenter[i].x * directionFacing,_collidersCenter[i].y)) + (Vector2)gameObject.transform.position;
                Collider2D[] enemiesCollider = Physics2D.OverlapBoxAll(overlapBoxCenter,_collidersSize[i],0,enemyLayerMask);
                for (int j = 0; j < enemiesCollider.Length; j++) {
                    enemiesCollider[j].GetComponent<EnemyBehaviour>().TakeDamage(_hitId,_damage);
                }
            }
        }
        UpdateHitId();
    }

    private int UpdateHitId() {
        _hitId++;
        if(_hitId > 2000000000) {
            _hitId = 0;
        }

        return _hitId;
    }

    private void ImportWeaponParameters() {
        _collidersCenter = currentWeapon.collidersCenter;
        _collidersSize = currentWeapon.collidersSize;

        //TODO import from weapon
        //_comboTree = currentWeapon.comboTree;
        _comboTree = comboTree;

        _currentAttack = _comboTree;
    }

    private void ActiveInput() {
        _inputActive = true;
    }

    private void DeactivateInput() {
        _inputActive = false;
    }

    private void ResetCombo() {
        _currentAttack = _comboTree;
    }

    private void DoQueuedAttack() {
        PlayAttackAnimation(_nextAttack);
    }

    private void ReadyToAttack() {
        _readyToAttack = true;
    }

    private void PlayAttackAnimation(BinaryTree nextAttack) {

        Debug.Log("into play-attack-animation");

        if (nextAttack != null) {

            Debug.Log("into next-attack != null");
            Debug.Log(nextAttack.value.name);

            _animator.Play(nextAttack.value.name);
            _damage = nextAttack.value.damage;
            _readyToAttack = false;
            _nextAttack = null;

            if(nextAttack.left == null && nextAttack.right == null) {
                _currentAttack = _comboTree;
            } else {
                _currentAttack = nextAttack;
            }
        }
    }




    



    // TODO change initialization of combo tree, this MUST NOT be here!!!!

    public BinaryTree comboTree = new BinaryTree();
    public Card[] cards = new Card[1];

    private void InitializeComboTree() {
        comboTree.left = new BinaryTree(cards[0]);
        comboTree.left.left = new BinaryTree(cards[0]);
        comboTree.left.right = new BinaryTree(cards[1]);

        comboTree.right = new BinaryTree(cards[2]);
        comboTree.right.right = new BinaryTree(cards[3]);
    }
}
