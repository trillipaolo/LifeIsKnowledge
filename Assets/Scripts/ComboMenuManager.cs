using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboMenuManager : MonoBehaviour {

    public static ComboMenuManager Instance { get; private set; }
    
    public BinaryTree knifeBinaryTree;
    public GameObject rootNode;
    public GameObject leftArrow;
    public GameObject rightArrow;

    [Header("Arrows Offset and Rotation")]
    public float xOffsetArrow;
    public float yOffsetArrow;
    public float zRotationArrow;

    [Header("RootNode Offset")]
    public float xOffsetNode;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start() {
        

        //Create the root node and Instantiate it at the right position
        Vector3 rootNodePosition = this.GetComponent<Transform>().position;
        Vector3 rootNodeOffset = new Vector3(xOffsetNode, 0, 0);
        rootNodePosition = rootNodePosition + rootNodeOffset;
        Quaternion rootNodeRotation = new Quaternion(0, 0, 0, 0);
        Instantiate(rootNode, rootNodePosition, rootNodeRotation);

        knifeBinaryTree = new BinaryTree();

        InitChildren(rootNodePosition);
    }

    // Update is called once per frame
    void Update() {

    }

    public void InitChildren(Vector3 fatherPosition)
    {
        SetLeftArrow(fatherPosition);
        SetRightArrow(fatherPosition);
    }

    public void SetLeftArrow(Vector3 fatherPosition)
    {
        Vector3 leftArrowOffset = new Vector3(xOffsetArrow, yOffsetArrow, 0);
        Vector3 leftArrowPosition = fatherPosition + leftArrowOffset;
        Quaternion leftArrowRotation = Quaternion.Euler(0, 0, zRotationArrow);
        
        Instantiate(leftArrow, leftArrowPosition, leftArrowRotation);
    }

    public void SetRightArrow(Vector3 fatherPosition)
    {
        Vector3 rightArrowOffset = new Vector3(xOffsetArrow, -yOffsetArrow, 0);
        Vector3 rightArrowPosition = fatherPosition + rightArrowOffset;
        Quaternion rightArrowRotation = Quaternion.Euler(0, 0, -zRotationArrow);

        Instantiate(rightArrow, rightArrowPosition, rightArrowRotation);
    }
}
