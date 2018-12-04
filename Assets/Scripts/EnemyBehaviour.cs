using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    public int lastHit = -1;
    public float health = 200;
    
	void Awake () {
        lastHit = -1;
	}

    public void TakeDamage(int hitId, float damage) {
        if (hitId == lastHit) {
            return;
        }

        FloatingTextController.CreateFloatingText(damage.ToString(),gameObject.transform);

        lastHit = hitId;
        health -= damage;

        if (health < 0) {
            Die();
        }
    }

    public void Die() {
        Invoke("Rekt",0.5f);

        gameObject.SetActive(false);
    }

    private void Rekt() {
        // TODO delete it :(
        FloatingTextController.CreateFloatingText("REKT <o/",gameObject.transform);
        Destroy(gameObject);
    }
}
