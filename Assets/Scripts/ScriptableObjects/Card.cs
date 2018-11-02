using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "ScriptableObjects/Card")]
public class Card : ScriptableObject {

    public Weapon weapon;
    public float damage = 0f;
    public AnimationClip animationClip;

    public void Awake() {
        CheckFields();
    }

    private void CheckFields() {
        if (weapon == null) {
            Debug.Log("weapon field into Card ScriptableObject is NOT initialized!");
        }
        if (damage == 0) {
            Debug.Log("damage field into Card ScriptableObject is set to zero!");
        }
        if(animationClip == null) {
            Debug.Log("animationClip field into Card ScriptableObject is NOT initialized!");
        }
    }
}
