using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootNode : MonoBehaviour {

    public int depth = 0;
    public EnumerationStances.Stance[] stances;
    public GameObject leftChild;
    public GameObject rightChild;

    private Transform _rootPosition;

    [Header("Children Offset")]
    public float xOffset = 1;
    public float yOffset = 1;
    public float zRotation = 30f;

    private void Awake()
    {
        _rootPosition = GetComponent<Transform>();
    }

    // Use this for initialization
    void Start() {
        InitChildren();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //This method is called by children node to check Card compatibility
    public bool CheckCompatibility ()
    {
        return true;
    }

    private void InitChildren ()
    {   
        Vector3 leftChildPosition = _rootPosition.position + new Vector3(xOffset, yOffset, 0);
        Quaternion leftChildRotation = Quaternion.Euler(0, 0, zRotation);
        Instantiate(leftChild, leftChildPosition, leftChildRotation);

        Vector3 rightChildPosition = _rootPosition.position + new Vector3(xOffset, -yOffset, 0);
        Quaternion rightChildRotation = Quaternion.Euler(0, 0, -zRotation);
        Instantiate(rightChild, leftChildPosition, leftChildRotation);
    }
}
