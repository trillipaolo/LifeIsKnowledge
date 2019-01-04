using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableDoorBehaviour : EnemyBehaviour {

    private bool _broken = false;

    private Animator _animator;
    private GameObject _obstacle;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _obstacle = transform.Find("Obstacle").gameObject;
    }

    public override void TakeDamage(Collider2D collider,float baseDamage,bool unique,int comboNum,int attackNum) {
        if(!_broken){
            if (comboNum == 1 && attackNum == 1) {
                _broken = true;
                _animator.SetTrigger("Open");
                _obstacle.SetActive(false);
            }
        }
    }
}
