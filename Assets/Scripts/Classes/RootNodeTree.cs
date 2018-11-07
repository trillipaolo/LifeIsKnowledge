using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootNodeTree {

    public GameObject node;
    public NodeTree left;
    public NodeTree right;

    public RootNodeTree (GameObject rootNode)
    {
        left = null;
        right = null;
        node = rootNode;
    }

}
