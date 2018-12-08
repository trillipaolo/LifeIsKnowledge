using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour{

    public int lastHit = -1;
    public float health = 200;

    public Collider2D[] colliders;
    public float[] damageMultipliers;
    
	void Awake () {
        lastHit = -1;
	}

    public virtual void TakeDamage(Collider2D collider, float baseDamage, bool unique) {
        float multiplier = GetMultiplier(collider);
        float damage = ComputeDamage(baseDamage,multiplier,unique);
        Color color = ComputeColor(multiplier, unique);
        
        Vector3 textPosition = (Vector3)collider.offset + transform.position;
        FloatingTextController.CreateFloatingText(damage.ToString(), textPosition, color);
        
        health -= damage;

        if (health < 0) {
            Die();
        }
    }

    public float GetMultiplier(Collider2D collider) {
        for(int i = 0; i < colliders.Length; i++) {
            if(colliders[i] == collider) {
                return damageMultipliers[i];
            }
        }
        return 0;
    }

    public Color ComputeColor(float multiplier, bool unique) {
        if(multiplier > 0 && unique) {
            return new Color(1f,Mathf.Min(0f,1 - multiplier),0f);
        } else if(multiplier < 0){
            return new Color(1f,1f,Mathf.Max(1f,-multiplier));
        } else {
            return new Color(1f,1f,0f);
        }
    }

    public float ComputeDamage(float baseDamage, float multiplier, bool unique) {
        if (multiplier > 0 && unique) {
            return baseDamage + baseDamage * multiplier;
        } else if (multiplier < 0) {
            return Mathf.Max(0, baseDamage + baseDamage * multiplier);
        } else {
            return baseDamage;
        }
    }

    public virtual void Die() {
        Invoke("Rekt",0.5f);

        gameObject.SetActive(false);
    }

    private void Rekt() {
        // TODO delete it :(
        FloatingTextController.CreateFloatingText("REKT <o/",gameObject.transform.position, new Color(0f,0f,0f));
        Destroy(gameObject);
    }
}
