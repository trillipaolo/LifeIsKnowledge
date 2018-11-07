using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryTree {

    //FOR CONSISTENCY W.R.T. PAOLO'S CODE
    public Card value;
    public BinaryTree left;
    public BinaryTree right;


    public RootNodeTree rootNode;
    public int depth;

    //FOR CONSISTENCY W.R.T. PAOLO'S CODE
    public BinaryTree()
    {
        value = null;
        left = null;
        right = null;
    }

    //FOR CONSISTENCY W.R.T. PAOLO'S CODE
    public BinaryTree (Card card)
    {
        value = card;
        left = null;
        right = null;
    }

    public BinaryTree (RootNodeTree root)
    {
        rootNode = root;
        depth = 0;
    }

    public void AddNode (BinaryTree currentNode, GameObject addedNodeLeft, GameObject addedNodeRight)
    {
        if (currentNode == null)
        {
            Debug.Log("CurrentNode is Null");
        }



    }


}
