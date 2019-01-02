using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneBehaviour : EnemyBehaviour {

    private Collider2D _platformCollider;
    private BoxCollider2D _physicsCollider;
    private DroneMovement _movementDroneScript;
    public Animator animator;

    

    void Awake() {
        _platformCollider = this.transform.Find("PlatformCollider").GetComponent<BoxCollider2D>();
        _physicsCollider = this.transform.GetComponent<BoxCollider2D>();
        _movementDroneScript = this.transform.GetComponent<DroneMovement>();
        _platformCollider.enabled = false;
        _unlockScript = GameObject.FindWithTag("Player").GetComponentInParent<JoelUnlockCombos>();

        _healthBar = GetComponentInChildren<Slider>();
        _healthBar.maxValue = health;
        _healthBar.value = health;

        
    }

    private void Start() {
        audioManager = AudioManager.instance;
    }

    public override void TakeDamage(Collider2D collider,float baseDamage,bool unique, int comboNum, int attackNum) {
        float multiplier = base.GetMultiplier(collider);
        float damage = base.ComputeDamage(baseDamage,multiplier,unique);
        Color color = base.ComputeColor(multiplier,unique);
        
        base.audioManager.Play(base.enemyHitSound);

        Vector3 textPosition = (Vector3)collider.offset + transform.position;
        FloatingTextController.CreateFloatingText(damage.ToString(),textPosition,color);

        health -= damage;

        if (health < 0) {
            Die();
        }

        _healthBar.value = health;
    }

    public override void Die() {
        for(int i = 0; i < base.colliders.Length; i++) {
            base.colliders[i].enabled = false;
        }
        _unlockScript.KilledEnemy(enemyType);
        animator.SetTrigger("Dead");
        //ChangePhysicsCollider();
        _movementDroneScript.Dying();
        _platformCollider.enabled = true;
        _healthBar.value = 0;
        _healthBar.GetComponentInChildren<Image>().color = Color.clear;
        GetComponent<AudioSource>().mute = true;

        
    }

    public void Deactivate() {
        for (int i = 0; i < base.colliders.Length; i++) {
            base.colliders[i].enabled = false;
        }
        animator.SetTrigger("Dead");
        //ChangePhysicsCollider();
        _movementDroneScript.Dying();
        _platformCollider.enabled = true;
        _healthBar.value = 0;
        _healthBar.GetComponentInChildren<Image>().color = Color.clear;
        GetComponent<AudioSource>().mute = true;

        Debug.Log("deattivato");
    }

    private void ChangePhysicsCollider() {
        _physicsCollider.offset = new Vector2(0f,-0.32f);
        _physicsCollider.size = new Vector2(1.17f, 0.58f);
    }

    private void EnablePlatfor() {
        if (true) {
            _platformCollider.enabled = true;
        } else {
            Invoke("EnablePlatform",0.5f);
        }
    }
}
