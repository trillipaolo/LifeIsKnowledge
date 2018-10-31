using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAndCollider {

    public float time;
    public ColliderPosition colliderPosition;

    public TimeAndCollider(float time, ColliderPosition colliderPosition) {
        this.time = time;
        this.colliderPosition = colliderPosition;
    }

    public void SetTime(float time) {
        this.time = time;
        return;
    }

    public void SetColliderPosition(int intColliderPosition) {
        this.colliderPosition = EnumColliderPosition.ToColliderPosition(intColliderPosition);
        return;
    }

    public void SetColliderPosition(ColliderPosition colliderPosition) {
        this.colliderPosition = colliderPosition;
        return;
    }
}