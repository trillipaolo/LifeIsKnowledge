using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeTree{

    public GameObject node;
    public NodeTree left;
    public NodeTree right;

    public NodeTree(GameObject newNode)
    {
        left = null;
        right = null;
        node = newNode;
    }
}
