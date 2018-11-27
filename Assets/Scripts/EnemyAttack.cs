using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    [Header("Enemy Colliders Container")]
    public GameObject Container;

    private BoxCollider2D[] Colliders;

    Collider2D[] results = new Collider2D[10];
    ContactFilter2D contactFilter = new ContactFilter2D();
    

	void Awake () {
        Colliders = Container.GetComponentsInChildren<BoxCollider2D>();

        /*TO DO: handle cases where colliders are not in order
        for (int i = 0; i < Colliders.Length; i++)
            Debug.Log("Colliders[" + i + "] = " + Colliders[i].name);*/

        contactFilter.SetLayerMask(LayerMask.GetMask("Damage"));
        contactFilter.useTriggers = true;
    }
	

	void Update () {

    }

    public void Damage(int damagePosition) {
        uint dmgPos = (uint) damagePosition;
        uint u = dmgPos & 4;
        uint m = dmgPos & 2;
        uint d = dmgPos & 1;

        

        if ( u>0 ) {
            Debug.Log("Nemico colpiscce UP");
            if (Colliders[0].OverlapCollider(contactFilter, results) > 0) {
                //if (Colliders[0].IsTouchingLayers())
                Debug.Log("GIOCATORE COLPITO CON UP");
                //TODO: PLAYER LOSES LIFE
            }
            
        }

        if (m > 0) {
            Debug.Log("Nemico colpiscce MIDDLE");
            if (Colliders[1].OverlapCollider(contactFilter, results) > 0) {
                //if (Colliders[1].IsTouchingLayers())
                Debug.Log("GIOCATORE COLPITO CON MIDDLE");
                //TODO: PLAYER LOSES LIFE
            }
        }

        if (d > 0) {
            Debug.Log("Nemico colpiscce DOWN");
            if (Colliders[2].OverlapCollider(contactFilter, results) > 0) {
                //if (Colliders[2].IsTouchingLayers())
                Debug.Log("GIOCATORE COLPITO CON DOWN");
                //TODO: PLAYER LOSES LIFE
            }
        }

    }
}
