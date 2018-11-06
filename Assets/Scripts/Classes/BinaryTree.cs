using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryTree {

    public Card value;
    public BinaryTree left;
    public BinaryTree right;

    public BinaryTree() {
        value = null;
        left = null;
        right = null;
    }

    public BinaryTree(Card card) {
        value = card;
        left = null;
        right = null;
    }


    // TODO erick code, test if the merge is correct


}
