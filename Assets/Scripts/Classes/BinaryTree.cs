using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryTree {

    public Card value;
    public GameObject node;
    public int depth;
    public BinaryTree left;
    public BinaryTree right;

    public BinaryTree() {
        value = null;
        left = null;
        right = null;
    }

    public BinaryTree(Card card, GameObject currentNode, int currentDepth) {
        value = card;
        left = null;
        right = null;
        node = currentNode;
        depth = currentDepth;
    }

    
    // TODO erick code, test if the merge is correct


}
