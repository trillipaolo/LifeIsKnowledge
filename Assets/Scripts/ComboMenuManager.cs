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

    //GameObject Tree variables
    public GameObject[] tree;
    public GameObject[] arrowTree;
    public int maxDepth;
    private int _numberOfNodes;
    private int _currentNodeIndex;

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
        InitTree();

        InitChildren();
    }

    // Update is called once per frame
    void Update() {

        //Move between cards (attacks per node)
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //Switch CurrentNode's Placeholder at Left
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //Switch CurrentNode's Placeholder at Right
        }

        //Move between unset nodes (fringe of the tree)
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //Switch CurrentNode down
            int nextCurrentNode = 1;
            for (int i = 1; i < _currentNodeIndex; i++)
            {
                try
                {
                    Node examinedNode = tree[i].GetComponent<Node>();
                    if (!examinedNode.GetSet())
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
            int nextCurrentNode = _numberOfNodes;
            for (int i = _numberOfNodes; i > _currentNodeIndex; i--)
            {   
                try
                {
                    Node examinedNode = tree[i].GetComponent<Node>();
                    if (!examinedNode.GetSet())
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

        //CONFIRM A CARD
        if(Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            //Stop arrow fading
            //Save Node in the Tree
            //Initialize new children
        }

        //DESELECT A CARD 
        if(Input.GetKeyDown(KeyCode.Backspace))
        {   
            //Eliminate currentNode and its brother
            //Eliminate Father from the Tree
            //Instantiate FatherNode Gameobject with its arrow fading
        }
    }

    //Initialize the Tree (Array) with its Root Node
    public void InitTree ()
    {   
        //Calculate trees basic properties
        _numberOfNodes = (int)Mathf.Pow(2, maxDepth);
        tree = new GameObject[_numberOfNodes];
        arrowTree = new GameObject[_numberOfNodes];

        //Create the root node and Instantiate it at the right position
        Vector3 rootNodePosition = this.GetComponent<Transform>().position;
        Vector3 rootNodeOffset = new Vector3(xOffsetNode, 0, 0);
        rootNodePosition = rootNodePosition + rootNodeOffset;
        Quaternion rootNodeRotation = new Quaternion(0, 0, 0, 0);

        GameObject clone = Instantiate(rootNode, rootNodePosition, rootNodeRotation);

        //Initialize the Array that store the Binary tree and its index;
        _currentNodeIndex = 0;
        tree[_currentNodeIndex] = clone;
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
        Vector3 leftArrowOffset = new Vector3(xOffsetArrow, -yOffsetArrow, 0);
        Vector3 leftArrowPosition = fatherPosition + leftArrowOffset;
        Quaternion leftArrowRotation = Quaternion.Euler(0, 0, -zRotationArrow);
        
        //Instantiate the Arrow and save it in the arrowTree
        GameObject leftArrowClone = Instantiate(leftArrow, leftArrowPosition, leftArrowRotation);
        arrowTree[2 * _currentNodeIndex + 1] = leftArrowClone;
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
        Vector3 rightArrowOffset = new Vector3(xOffsetArrow, yOffsetArrow, 0);
        Vector3 rightArrowPosition = fatherPosition + rightArrowOffset;
        Quaternion rightArrowRotation = Quaternion.Euler(0, 0, zRotationArrow);

        //Instantiate the Arrow and save it in the ArrowTree
        GameObject rightArrowClone = Instantiate(rightArrow, rightArrowPosition, rightArrowRotation);
        arrowTree[2 * _currentNodeIndex + 2] = rightArrowClone;

        //Impose the right node as Selected node after children creation
        rightArrowClone.GetComponent<Arrow>().FromSlowToFast();
    }

    //Destroy the currentRightArrow and set Null in the array
    public void DeactivateRightArrow()
    {
        Destroy(arrowTree[2 * _currentNodeIndex + 1]);
        arrowTree[2 * _currentNodeIndex + 2] = null;
    }

    public void InitLeftChild(Vector3 fatherPosition)
    {
        

        //Calculate Node position and rotation
        Vector3 leftChildOffset = new Vector3(xOffsetChild, -yOffsetChild, 0);
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
}
