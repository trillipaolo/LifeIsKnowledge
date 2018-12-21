using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour{

    public EnumEnemies enemyType;
    [HideInInspector]
    public JoelUnlockCombos _unlockScript;

    public float health = 200;
    private EnemyMovementPhysics _movementScript;

    [HideInInspector]
    public Slider _healthBar;

    public Collider2D[] colliders;
    public float[] damageMultipliers;

    [HideInInspector]
    public AudioManager audioManager;
    public string enemyHitSound;

    void Awake () {
        _movementScript = transform.GetComponent<EnemyMovementPhysics>();
        _unlockScript = GameObject.FindWithTag("Player").GetComponentInParent<JoelUnlockCombos>();

        _healthBar = GetComponentInChildren<Slider>();
      
        if (_healthBar != null) {
            _healthBar.maxValue = health;
            _healthBar.value = health;
        }
        
	}

    private void Start() {
        audioManager = AudioManager.instance;
    }

    public virtual void TakeDamage(Collider2D collider, float baseDamage, bool unique) {
        float multiplier = GetMultiplier(collider);
        float damage = ComputeDamage(baseDamage,multiplier,unique);
        Color color = ComputeColor(multiplier, unique);

        audioManager.Play(enemyHitSound);

        Vector3 textPosition = (Vector3)collider.offset + transform.position;
        FloatingTextController.CreateFloatingText(damage.ToString(), textPosition, color);

        health -= damage;

        if (health < 0) {
            Die();
        }

        _healthBar.value = health;
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
        for (int i = 0; i < colliders.Length; i++) {
            colliders[i].enabled = false;
        }
        _unlockScript.KilledEnemy(enemyType);
        _movementScript.isDead = true;
        _healthBar.GetComponentInChildren<Image>().color = Color.clear;
    }

    private void Rekt() {
        // TODO delete it :(
        FloatingTextController.CreateFloatingText("REKT <o/",gameObject.transform.position, new Color(0f,0f,0f));
        Destroy(gameObject);
    }
}
