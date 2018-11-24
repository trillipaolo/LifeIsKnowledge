using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboMenuManagerOld : MonoBehaviour
{

    public static ComboMenuManagerOld Instance { get; private set; }

    public BinaryTree knifeBinaryTree;
    public GameObject rootNode;
    public GameObject node;
    public GameObject leftArrow;
    public GameObject rightArrow;

    //GameObject Tree variables
    public GameObject[] tree;
    public GameObject[] arrowTree;
    public float[] arrowRotations; //Degrees
    public Vector3[] arrowPositions;
    public int maxDepth;
    private int _numberOfNodes;
    private int _currentNodeIndex;

    [Header("Arrows Offset and Rotation")]
    public float lenghtArrow;
    public float xOffsetArrow;
    public float yOffsetArrow;
    public float zRotationArrow;

    [Header("Local Adjustments on Arrows")]
    public float xLocalOffset;
    public float yLocalOffset;

    [Header("ChildNode Offset")]
    private float xOffsetChild;
    private float yOffsetChild;

    [Header("RootNode Offset")]
    public float xOffsetNode;

    [Header("TextNode Offset")]
    public float xOffsetText;
    public float yOffsetText;

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
    void Start()
    {
        InitTree();

        InitChildren();
    }

    // Update is called once per frame
    void Update()
    {

        //Move between cards (attacks per node)
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //Switch CurrentNode's Placeholder at Left
            Node currentNode = tree[_currentNodeIndex].GetComponent<Node>();
            currentNode.SwitchPlaceHolderLeft();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //Switch CurrentNode's Placeholder at Right
            Node currentNode = tree[_currentNodeIndex].GetComponent<Node>();
            currentNode.SwitchPlaceHolderLeft();
        }
        //Move between unset nodes (fringe of the tree)
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //Switch CurrentNode down
            int nextCurrentNode = SearchDownUnsetNode();

            if (_currentNodeIndex == nextCurrentNode)
            {
                //Do Nothing
                Debug.Log("The next Current node is the last CurrentNode");
            }
            else
            {
                Arrow lastArrow = arrowTree[_currentNodeIndex].GetComponent<Arrow>();
                lastArrow.FromFastToSlow();

                _currentNodeIndex = nextCurrentNode;

                Arrow nextArrow = arrowTree[_currentNodeIndex].GetComponent<Arrow>();
                nextArrow.FromSlowToFast();
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //Switch CurrentNode Up

            int nextCurrentNode = SearchUpUnsetNode();

            if (_currentNodeIndex == nextCurrentNode)
            {
                //Do Nothing
                Debug.Log("The next Current node is the last CurrentNode");
            }
            else
            {
                Arrow lastArrow = arrowTree[_currentNodeIndex].GetComponent<Arrow>();
                lastArrow.FromFastToSlow();

                _currentNodeIndex = nextCurrentNode;

                Debug.Log("CurrentIndex when trying to go above the highest arrow should be 1 but it is: " + _currentNodeIndex);
                Arrow nextArrow = arrowTree[_currentNodeIndex].GetComponent<Arrow>();
                nextArrow.FromSlowToFast();
            }
        }

        //CONFIRM A CARD
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //Stop arrow fading
            Arrow currentArrow = arrowTree[_currentNodeIndex].GetComponent<Arrow>();
            currentArrow.StopFadingEffect();

            //Set the currentNode
            Node currentNode = tree[_currentNodeIndex].GetComponent<Node>();
            currentNode.SetChosen(true);

            

            int nextCurrentNode = 2 * _currentNodeIndex + 1;
            if (nextCurrentNode < _numberOfNodes)
            {
                //Initialize new children
                InitChildren();
            }
            else
            {
                Debug.Log("MaxDepth reached: No other nodes can be created");
                //Select another node
                _currentNodeIndex = SelectOtherNode();
            }
        }

        //DESELECT A CARD 
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (_currentNodeIndex > 2)
            {
                //Eliminate currentNode and its brother
                GameObject eliminatedNode = tree[_currentNodeIndex];
                Node node = eliminatedNode.GetComponent<Node>();
                node.DestroyPlaceHolder();
                Destroy(eliminatedNode);
                tree[_currentNodeIndex] = null;

                int oddOrEven = _currentNodeIndex % 2;
                if (oddOrEven == 1)
                {
                    eliminatedNode = tree[_currentNodeIndex + 1];
                    node = eliminatedNode.GetComponent<Node>();
                    node.DestroyPlaceHolder();
                    Destroy(eliminatedNode);
                    tree[_currentNodeIndex + 1] = null;
                    _currentNodeIndex = (_currentNodeIndex - 1) / 2;
                }
                else
                {
                    eliminatedNode = tree[_currentNodeIndex - 1];
                    node = eliminatedNode.GetComponent<Node>();
                    node.DestroyPlaceHolder();
                    Destroy(eliminatedNode);
                    tree[_currentNodeIndex - 1] = null;
                    _currentNodeIndex = (_currentNodeIndex - 2) / 2;
                }

                //Destroy Arrows
                DeactivateLeftArrow();
                DeactivateRightArrow();

                //Re-enable the father node
                Node currentNode;
                try
                {
                    currentNode = tree[_currentNodeIndex].GetComponent<Node>();
                }
                catch
                {
                    _currentNodeIndex = SelectOtherNode();
                }
                currentNode = tree[_currentNodeIndex].GetComponent<Node>();
                currentNode.SetChosen(false);
                Arrow currentArrow = arrowTree[_currentNodeIndex].GetComponent<Arrow>();
                currentArrow.StartFadingEffect();
            }
            else
            {
                Debug.Log("You cannot eliminate the first two nodes");
            }
        }
    }

    //Initialize the Tree (Array) with its Root Node
    public void InitTree()
    {
        //Calculate trees basic properties
        _numberOfNodes = (int)(Mathf.Pow(2, maxDepth) - 1);
        tree = new GameObject[_numberOfNodes];
        arrowTree = new GameObject[_numberOfNodes];
        arrowRotations = new float[_numberOfNodes];
        arrowPositions = new Vector3[_numberOfNodes];

        //Create the root node and Instantiate it at the right position
        Vector3 rootNodePosition = this.GetComponent<Transform>().position;
        Vector3 rootNodeOffset = new Vector3(xOffsetNode, 0, 0);
        rootNodePosition = rootNodePosition + rootNodeOffset;
        Quaternion rootNodeRotation = new Quaternion(0, 0, 0, 0);

        GameObject clone = Instantiate(rootNode, rootNodePosition, rootNodeRotation);

        //Initialize the Array that store the Binary tree and its index and Arrow Positions/Rotations
        _currentNodeIndex = 0;
        tree[_currentNodeIndex] = clone;
        arrowRotations[_currentNodeIndex] = 0;
        arrowRotations[2 * _currentNodeIndex + 2] = zRotationArrow;
        arrowRotations[2 * _currentNodeIndex + 1] = -zRotationArrow;
        arrowPositions[_currentNodeIndex] = new Vector3(0, 0, 0);
        arrowPositions[2 * _currentNodeIndex + 2] = new Vector3(xOffsetArrow, yOffsetArrow, 0);
        arrowPositions[2 * _currentNodeIndex + 1] = new Vector3(xOffsetArrow, -yOffsetArrow, 0);
    }

    //Initialize and Create the next two Children w.r.t. the father in the tree and their arrows
    public void InitChildren()
    {
        Vector3 fatherPosition = tree[_currentNodeIndex].GetComponent<Transform>().position;

        SetLeftArrow(fatherPosition);
        SetRightArrow(fatherPosition);

        InitLeftChild(fatherPosition);
        InitRightChild(fatherPosition);
    }

    //Instantiate Left Arrow in the correct position w.r.t its father
    public void SetLeftArrow(Vector3 fatherPosition)
    {
        //Calculate Arrow position and rotation
        xOffsetArrow = arrowPositions[2 * _currentNodeIndex + 1].x;
        yOffsetArrow = arrowPositions[2 * _currentNodeIndex + 1].y;
        Vector3 leftArrowOffset = new Vector3(xOffsetArrow, yOffsetArrow, 0);
        Vector3 leftArrowPosition = fatherPosition + leftArrowOffset;
        Quaternion leftArrowRotation = Quaternion.Euler(0, 0, arrowRotations[2 * _currentNodeIndex + 1]);

        //Instantiate the Arrow and save it in the arrowTree
        GameObject leftArrowClone = Instantiate(leftArrow, leftArrowPosition, leftArrowRotation);
        arrowTree[2 * _currentNodeIndex + 1] = leftArrowClone;

        //Prepare grand children arrow rotation
        GrandChildrenArrowRotationInit(2 * _currentNodeIndex + 1);

        //Prepare grand children arrow position
        GrandChildrenArrowPositionInit(2 * _currentNodeIndex + 1);
    }

    //Destroy the currentLeftArrow and set Null in the array
    public void DeactivateLeftArrow()
    {
        Destroy(arrowTree[2 * _currentNodeIndex + 1]);
        arrowTree[2 * _currentNodeIndex + 1] = null;
    }

    //Instantiate Right Arrow in the correct position w.r.t its father
    public void SetRightArrow(Vector3 fatherPosition)
    {
        //Calculate Arrow position and rotation
        xOffsetArrow = arrowPositions[2 * _currentNodeIndex + 2].x;
        yOffsetArrow = arrowPositions[2 * _currentNodeIndex + 2].y;
        Vector3 rightArrowOffset = new Vector3(xOffsetArrow, yOffsetArrow, 0);
        Vector3 rightArrowPosition = fatherPosition + rightArrowOffset;
        Quaternion rightArrowRotation = Quaternion.Euler(0, 0, arrowRotations[2 * _currentNodeIndex + 2]);

        //Instantiate the Arrow and save it in the ArrowTree
        GameObject rightArrowClone = Instantiate(rightArrow, rightArrowPosition, rightArrowRotation);
        arrowTree[2 * _currentNodeIndex + 2] = rightArrowClone;

        //Impose the right node as Selected node after children creation
        rightArrowClone.GetComponent<Arrow>().FromSlowToFast();

        //Prepare grand-children arrow rotation
        GrandChildrenArrowRotationInit(2 * _currentNodeIndex + 2);

        //Prepare grand children arrow position
        GrandChildrenArrowPositionInit(2 * _currentNodeIndex + 2);
    }

    //Destroy the currentRightArrow and set Null in the array
    public void DeactivateRightArrow()
    {
        Destroy(arrowTree[2 * _currentNodeIndex + 2]);
        arrowTree[2 * _currentNodeIndex + 2] = null;
    }

    public void InitLeftChild(Vector3 fatherPosition)
    {
        //Calculate Node position and rotation
        float zRotationArrowRad = (Mathf.PI / 180) * arrowRotations[2 * _currentNodeIndex + 1];
        xOffsetChild = lenghtArrow * Mathf.Cos(zRotationArrowRad);
        yOffsetChild = lenghtArrow * Mathf.Sin(zRotationArrowRad);

        Vector3 leftChildOffset = new Vector3(xOffsetChild, yOffsetChild, 0);
        Vector3 leftChildPosition = fatherPosition + leftChildOffset;
        Quaternion leftChildRotation = Quaternion.Euler(0, 0, 0);

        //Instantiate the Node
        GameObject leftClone = Instantiate(node, leftChildPosition, leftChildRotation);

        //Insert the node in the tree
        tree[2 * _currentNodeIndex + 1] = leftClone;

        //Adjust placeholder position
        Node leftNode = leftClone.GetComponent<Node>();
        leftNode.AdjustPlaceHolderPosition(xOffsetText, -yOffsetText);
    }

    public void InitRightChild(Vector3 fatherPosition)
    {
        //Calculate Node position and rotation
        float zRotationArrowRad = (Mathf.PI / 180) * arrowRotations[2 * _currentNodeIndex + 2];
        xOffsetChild = lenghtArrow * Mathf.Cos(zRotationArrowRad);
        yOffsetChild = lenghtArrow * Mathf.Sin(zRotationArrowRad);
        
        Vector3 rightChildOffset = new Vector3(xOffsetChild, yOffsetChild, 0);
        Vector3 rightChildPosition = fatherPosition + rightChildOffset;
        Quaternion rightChildRotation = Quaternion.Euler(0, 0, 0);

        //Instantiate the Node
        GameObject rightClone = Instantiate(node, rightChildPosition, rightChildRotation);

        //Insert the node in the tree
        tree[2 * _currentNodeIndex + 2] = rightClone;
        _currentNodeIndex = 2 * _currentNodeIndex + 2;

        //Adjust placeholder position
        Node rightNode = rightClone.GetComponent<Node>();
        rightNode.AdjustPlaceHolderPosition(xOffsetText, yOffsetText);
    }

    public int SearchUpUnsetNode()
    {
        int nextCurrentNode = _currentNodeIndex;
        for (int i = _numberOfNodes - 1; i > _currentNodeIndex; i--)
        {
            try
            {
                Node examinedNode = tree[i].GetComponent<Node>();
                if (!examinedNode.GetChosen())
                {
                    nextCurrentNode = i;
                }
            }
            catch
            {
                //Do Nothing, if we found a in tree[i] a null node, 
                //it means that the tree is momentarily unbalanced
            }

        }

        return nextCurrentNode;
    }

    public int SearchDownUnsetNode()
    {
        int nextCurrentNode = _currentNodeIndex;
        for (int i = 1; i < _currentNodeIndex; i++)
        {
            try
            {
                Node examinedNode = tree[i].GetComponent<Node>();
                if (!examinedNode.GetChosen())
                {
                    nextCurrentNode = i;
                }
            }
            catch
            {
                //Do Nothing, if we found a in tree[i] a null node, 
                //it means that the tree is momentarily unbalanced
            }
        }

        return nextCurrentNode;
    }

    public int SelectOtherNode()
    {
        int currentNextNode = SearchUpUnsetNode();
        if (currentNextNode == _currentNodeIndex)
        {
            currentNextNode = SearchDownUnsetNode();
        }
        
        return currentNextNode;
    }

    public void GrandChildrenArrowRotationInit(int childIndex)
    {
        try
        {   
            int nextLeftChildIndex = 2 * childIndex + 1;
            int nextRightChildrenIndex = 2 * childIndex + 2;
            arrowRotations[nextLeftChildIndex] = - 1 * (Mathf.Abs(arrowRotations[childIndex]) / 2);
            arrowRotations[nextRightChildrenIndex] = Mathf.Abs(arrowRotations[childIndex]) / 2;
        }
        catch
        {
            Debug.Log("Grand-Children Node exceed max depth: No Arrow Rotation needed");
        }
    }

    public void GrandChildrenArrowPositionInit(int childIndex)
    {
        try
        {
            int leftGrandChildIndex = 2 * childIndex + 1;
            int rightGrandChildrenIndex = 2 * childIndex + 2;

            float xLocalPosition = arrowPositions[childIndex].x + xLocalOffset;
            float yLocalPosition;
            float yLocalInvertedPosition;

            int oddOrEven = childIndex % 2;
            if (oddOrEven == 0)
            {   
                //If the Child is a Right Node
                yLocalPosition = arrowPositions[childIndex].y - yLocalOffset;
                yLocalInvertedPosition = -1 * arrowPositions[childIndex].y + yLocalOffset;

                arrowPositions[leftGrandChildIndex] = new Vector3(xLocalPosition, yLocalInvertedPosition, 0);
                arrowPositions[rightGrandChildrenIndex] = new Vector3(xLocalPosition, yLocalPosition, 0);
            }
            else
            {
                //If the Child is a Left Node
                yLocalPosition = arrowPositions[childIndex].y + yLocalOffset;
                yLocalInvertedPosition = -1 * arrowPositions[childIndex].y - yLocalOffset;

                arrowPositions[leftGrandChildIndex] = new Vector3(xLocalPosition, yLocalPosition, 0);
                arrowPositions[rightGrandChildrenIndex] = new Vector3(xLocalPosition, yLocalInvertedPosition, 0);
            }
        }
        catch
        {
            Debug.Log("Grand-Children Node exceed max depth: No Arrow Position needed");
        }
    }
}
