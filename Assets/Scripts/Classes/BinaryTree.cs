using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryTree {

    //FOR CONSISTENCY W.R.T. PAOLO'S CODE
    public Card value;
    public BinaryTree left;
    public BinaryTree right;

    public RootNodeTree rootNode;
    
    public BinaryTree()
    {
        value = null;
        left = null;
        right = null;
    }
    
    public BinaryTree (Card card)
    {
        value = card;
        left = null;
        right = null;
    }

    public BinaryTree (RootNodeTree root)
    {
        rootNode = root;
    }


}
