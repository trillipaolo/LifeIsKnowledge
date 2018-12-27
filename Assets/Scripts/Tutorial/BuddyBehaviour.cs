using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuddyBehaviour : EnemyBehaviour {

    private bool _broken = false;
    private Animator _animator;

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    public override void TakeDamage(Collider2D collider,float baseDamage,bool unique, int comboNum, int attackNum) {
        if (!_broken) {
            float multiplier = GetMultiplier(collider);
            float damage = ComputeDamage(baseDamage,multiplier,unique);
            Color color = ComputeColor(multiplier,unique);

            Vector3 textPosition = (Vector3)collider.offset + transform.position;
            FloatingTextController.CreateFloatingText(damage.ToString(),textPosition,color);

            _animator.SetTrigger("Hit");
        }
    }
}
