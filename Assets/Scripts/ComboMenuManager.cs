using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboMenuManager : MonoBehaviour {

    public static ComboMenuManager Instance { get; private set; }
    
    public BinaryTree knifeBinaryTree;
    public GameObject rootNode;
    public GameObject node;
    public GameObject leftArrow;
    public GameObject rightArrow;

    [Header("Arrows Offset and Rotation")]
    public float xOffsetArrow;
    public float yOffsetArrow;
    public float zRotationArrow;

    [Header("ChildNode Offset")]
    public float xOffsetChild;
    public float yOffsetChild;

    [Header("RootNode Offset")]
    public float xOffsetNode;

    [Header("TextNode Offset")]
    public float xOffsetText;
    public float yOffsetText;

    private Vector3 _lastNodePosition;

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
        InitBinaryTree();

        InitChildren(_lastNodePosition);

        
    }

    // Update is called once per frame
    void Update() {

        
    }

    public void InitBinaryTree ()
    {
        //Create the root node and Instantiate it at the right position
        Vector3 rootNodePosition = this.GetComponent<Transform>().position;
        Vector3 rootNodeOffset = new Vector3(xOffsetNode, 0, 0);
        rootNodePosition = rootNodePosition + rootNodeOffset;
        Quaternion rootNodeRotation = new Quaternion(0, 0, 0, 0);
        Instantiate(rootNode, rootNodePosition, rootNodeRotation);

        RootNodeTree rootNodeTree = new RootNodeTree(rootNode);

        //Initialize the binary tree
        knifeBinaryTree = new BinaryTree(rootNodeTree);

        _lastNodePosition = rootNodePosition;
    }

    //Initialize and Create the next two Children w.r.t. the father
    public void InitChildren(Vector3 fatherPosition)
    {
        SetLeftArrow(fatherPosition);
        SetRightArrow(fatherPosition);

        InitLeftChild(fatherPosition);
        InitRightChild(fatherPosition);
    }

    //Instantiate Left Arrow in the correct position w.r.t its father
    public void SetLeftArrow(Vector3 fatherPosition)
    {
        Vector3 leftArrowOffset = new Vector3(xOffsetArrow, -yOffsetArrow, 0);
        Vector3 leftArrowPosition = fatherPosition + leftArrowOffset;
        Quaternion leftArrowRotation = Quaternion.Euler(0, 0, -zRotationArrow);
        
        Instantiate(leftArrow, leftArrowPosition, leftArrowRotation);
    }

    //Instantiate Right Arrow in the correct position w.r.t its father
    public void SetRightArrow(Vector3 fatherPosition)
    {
        Vector3 rightArrowOffset = new Vector3(xOffsetArrow, yOffsetArrow, 0);
        Vector3 rightArrowPosition = fatherPosition + rightArrowOffset;
        Quaternion rightArrowRotation = Quaternion.Euler(0, 0, zRotationArrow);

        Instantiate(rightArrow, rightArrowPosition, rightArrowRotation);
    }

    public void InitLeftChild(Vector3 fatherPosition)
    {
        Vector3 leftChildOffset = new Vector3(xOffsetChild, -yOffsetChild, 0);
        Vector3 leftChildPosition = fatherPosition + leftChildOffset;
        Quaternion leftChildRotation = Quaternion.Euler(0, 0, 0);

        GameObject leftClone = Instantiate(node, leftChildPosition, leftChildRotation);
        Node leftNode = leftClone.GetComponent<Node>();
        leftNode.AdjustPlaceHolderPosition(xOffsetText, -yOffsetText);

        NodeTree leftNodeTree = new NodeTree(node);
        //TODO ADD
    }

    public void InitRightChild(Vector3 fatherPosition)
    {
        Vector3 rightChildOffset = new Vector3(xOffsetChild, yOffsetChild, 0);
        Vector3 rightChildPosition = fatherPosition + rightChildOffset;
        Quaternion rightChildRotation = Quaternion.Euler(0, 0, 0);

        GameObject rightClone = Instantiate(node, rightChildPosition, rightChildRotation);
        Node rightNode = rightClone.GetComponent<Node>();
        rightNode.AdjustPlaceHolderPosition(xOffsetText, yOffsetText);


        NodeTree rightNodeTree = new NodeTree(node);

        //TODO ADD
    }
}
