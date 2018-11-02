using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayColliderGizmos : MonoBehaviour {

    public Vector2[] colliderCenter = new Vector2[EnumColliderPosition.Size()];
    public Vector2[] colliderSize = new Vector2[EnumColliderPosition.Size()];

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        for (int i = 0; i < colliderCenter.Length; i++) {
            Gizmos.DrawWireCube(colliderCenter[i] + (Vector2)gameObject.transform.position,colliderSize[i]);
        }
    }
}
