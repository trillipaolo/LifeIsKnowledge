using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    public int lastHit = -1;
    public float health = 10;

	// Use this for initialization
	void Awake () {
        lastHit = -1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(int hitId, float damage) {
        if(hitId == lastHit) {
            return;
        }

        FloatingTextController.CreateFloatingText(damage.ToString(),gameObject.transform);

        hitId = lastHit;
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
        FloatingTextController.CreateFloatingText("REKT <o/",gameObject.transform);
        Destroy(gameObject);
    }
}
