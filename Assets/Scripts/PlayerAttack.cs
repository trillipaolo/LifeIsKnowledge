using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    //TODO import from Weapon Sciptable object
    public Vector2[] colliderCenter = new Vector2[EnumColliderPosition.Size()];
    public Vector2[] colliderSize = new Vector2[EnumColliderPosition.Size()];

    public int hitId = 0;
    public LayerMask enemyLayerMask;
    public PlayerMovement playerMovement;

    private void Awake() {
        hitId = 0;

        //TODO needs to be moved in another part
        FloatingTextController.Initialize();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.L))
            gameObject.GetComponent<Animator>().Play("SlashTopDown");
    }

    public void Attack(string colliderPositionString) {
        bool[] colliderPositions = EnumColliderPosition.StringToArray(colliderPositionString);
        float directionFacing = playerMovement.directionFacing;

        for(int i = 0; i < colliderCenter.Length; i++) {
            if (colliderPositions[i]) {
                Collider2D[] enemiesCollider = Physics2D.OverlapBoxAll(colliderCenter[i] * directionFacing + (Vector2)gameObject.transform.position,colliderSize[i],0,enemyLayerMask);
                for (int j = 0; j < enemiesCollider.Length; j++) {
                    enemiesCollider[j].GetComponent<EnemyBehaviour>().TakeDamage(hitId,20);
                }
            }
        }
        UpdateHitId();
    }

    private int UpdateHitId() {
        hitId++;
        if(hitId > 2000000000) {
            hitId = 0;
        }

        return hitId;
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        for (int i = 0; i < colliderCenter.Length; i++) {
            Gizmos.DrawWireCube(colliderCenter[i] + (Vector2)gameObject.transform.position,colliderSize[i]);
        }
    }
}
