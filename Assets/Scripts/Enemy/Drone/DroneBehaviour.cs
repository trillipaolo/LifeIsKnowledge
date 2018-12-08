using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBehaviour : EnemyBehaviour {

    private Collider2D _platformCollider;
    private BoxCollider2D _physicsCollider;

    void Awake() {
        _platformCollider = this.transform.Find("PlatformCollider").GetComponent<BoxCollider2D>();
        _physicsCollider = this.transform.Find("Drone").GetComponent<BoxCollider2D>();
        _platformCollider.enabled = false;
    }

    public override void Die() {
        ChangePhysicsCollider();

        Invoke("EnablePlatform",0);
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
