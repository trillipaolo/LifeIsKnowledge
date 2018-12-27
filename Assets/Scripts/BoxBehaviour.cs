using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBehaviour : EnemyBehaviour {

    public override void TakeDamage(Collider2D collider,float baseDamage,bool unique, int comboNum, int attackNum) {
        float multiplier = GetMultiplier(collider);
        float damage = ComputeDamage(baseDamage,multiplier,unique);
        Color color = ComputeColor(multiplier,unique);

        Vector3 textPosition = (Vector3)collider.offset + transform.position;
        FloatingTextController.CreateFloatingText(damage.ToString(),textPosition,color);
    }
}
