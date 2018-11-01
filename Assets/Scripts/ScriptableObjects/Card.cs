using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "ScriptableObjects/Card")]
public class Card : ScriptableObject {

    public Weapon weapon;
    public float damage = 0f;
    public Animation animation;

    public Transform playerTransform;
    public Vector2[] colliderCenter = new Vector2[EnumColliderPosition.Size()];
    public Vector2[] colliderSize = new Vector2[EnumColliderPosition.Size()];

    public void Awake() {
        CheckFields();
    }

    private void CheckFields() {
        if (weapon == null)
            Debug.Log("weapon field into Card ScriptableObject is NOT initialized!");
        if (damage == 0)
            Debug.Log("weapon field into Card ScriptableObject is NOT initialized!");
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Vector2 playerPosition = new Vector2(playerTransform.position.x,playerTransform.position.y);
        for (int i = 0; i < colliderCenter.Length; i++) {
            Gizmos.DrawWireCube(colliderCenter[i] + playerPosition,colliderSize[i]);
        }
    }
}
