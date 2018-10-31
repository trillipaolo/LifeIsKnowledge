using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "ScriptableObjects/Card")]
public class Card : ScriptableObject {

    public Weapon weapon;
    public float damage = 0f;
    public Animation animation;
    public List<TimeAndCollider> hitboxes = new List<TimeAndCollider>();

    public void Awake() {
        Initialize();

        CheckFields();
    }

    private void Initialize() {
        hitboxes.Add(new TimeAndCollider(2.5f,ColliderPosition.BACKDOWN));
        hitboxes.Add(new TimeAndCollider(3f,ColliderPosition.BACKDOWN));
        hitboxes.Add(new TimeAndCollider(1f,ColliderPosition.FRONTDOWN));

        // OPTIMIZATION: this method can be expensive in term of memory, but this list should be very small (at maximum 15 elements)
        List<TimeAndCollider> sortedHitboxes = hitboxes.OrderBy(h => h.time).ToList();
        hitboxes = sortedHitboxes;
    }

    private void CheckFields() {
        if (weapon == null)
            Debug.Log("weapon field into Card ScriptableObject is NOT initialized!");
        if (damage == 0)
            Debug.Log("weapon field into Card ScriptableObject is NOT initialized!");
    }
}
