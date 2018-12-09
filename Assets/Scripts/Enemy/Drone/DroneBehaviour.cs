using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBehaviour : EnemyBehaviour {

    private Collider2D _platformCollider;
    private BoxCollider2D _physicsCollider;
    private DroneMovement _movementScript;
    public Animator animator;

    void Awake() {
        _platformCollider = this.transform.Find("PlatformCollider").GetComponent<BoxCollider2D>();
        _physicsCollider = this.transform.GetComponent<BoxCollider2D>();
        _movementScript = this.transform.GetComponent<DroneMovement>();
        _platformCollider.enabled = false;
    }

    public override void TakeDamage(Collider2D collider,float baseDamage,bool unique) {
        float multiplier = base.GetMultiplier(collider);
        float damage = base.ComputeDamage(baseDamage,multiplier,unique);
        Color color = base.ComputeColor(multiplier,unique);

        Vector3 textPosition = (Vector3)collider.offset + transform.position;
        FloatingTextController.CreateFloatingText(damage.ToString(),textPosition,color);

        health -= damage;

        if (health < 0) {
            Die();
        }
    }

    public override void Die() {
        animator.SetTrigger("Dead");
        ChangePhysicsCollider();
        _movementScript.Dying();
        _platformCollider.enabled = true;
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
