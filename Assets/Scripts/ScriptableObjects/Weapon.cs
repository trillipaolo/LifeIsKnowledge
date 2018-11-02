using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Weapon")]
public class Weapon : ScriptableObject {

    public Vector2[] collidersCenter = new Vector2[EnumColliderPosition.Size()];
    public Vector2[] collidersSize = new Vector2[EnumColliderPosition.Size()];
    public BinaryTree comboTree = new BinaryTree();
}
