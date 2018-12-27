using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelBehaviour : EnemyBehaviour {

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

            health -= damage;

            if (health < 0) {
                Die();
            }
        }
    }

    public override void Die() {
        _broken = true;

        for (int i = 0; i < colliders.Length; i++) {
            colliders[i].enabled = false;
        }

        _animator.SetTrigger("Broken");
    }
}
