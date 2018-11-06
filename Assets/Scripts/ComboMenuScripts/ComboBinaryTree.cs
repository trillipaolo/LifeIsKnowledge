using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboBinaryTree {

    public GameObject card;
    public ComboBinaryTree left;
    public ComboBinaryTree right;

    public ComboBinaryTree ()
    {
        card = null;
        left = null;
        right = null;
    }

    public ComboBinaryTree(GameObject card)
    {
        this.card = card;
        this.left = null;
        this.right = null;

        Debug.Log("TopNode initialized");
    }

}
