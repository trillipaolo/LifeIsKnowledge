using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderArea : MonoBehaviour {

    [Header("Joel Reference")]
    public GameObject target;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == target)
        {
            GetComponentInParent<SafeArea>().SetIsColliding(true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == target)
        {
            GetComponentInParent<SafeArea>().SetIsColliding(false);
        }
    }
}
