using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboMenuManager : MonoBehaviour {

    public static ComboMenuManager instance { get; private set; }

    public BinaryTree knifeBinaryTree;
    public BinaryTree rootNode;
    public BinaryTree leftArrow;
    public BinaryTree rightArrow;

    [Header("Arrows Offset and Rotation")]
    public int xOffset;
    public int yOffset;
    public int zRotation;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start() {
        knifeBinaryTree = new BinaryTree();


        Transform rootNodePosition = this.GetComponent<Transform>();
        // why you need to instantiate it? it is only a data structure
        //Instantiate(knifeBinaryTree, rootNodePosition);

        InitChildren(rootNodePosition);
    }

    // Update is called once per frame
    void Update() {

    }

    public void InitChildren(Transform fatherPosition)
    {
        SetLeftArrow(fatherPosition);
        SetRightArrow(fatherPosition);
    }

    public void SetLeftArrow(Transform fatherTransform)
    {
        Vector3 leftArrowOffset = new Vector3(xOffset, yOffset, 0);
        Vector3 leftArrowPosition = fatherTransform.position + leftArrowOffset;
        Quaternion leftArrowRotation = Quaternion.Euler(0, 0, zRotation);
        //same here
        //Instantiate(leftArrow, leftArrowPosition, leftArrowRotation);
    }

    public void SetRightArrow(Transform fatherPosition)
    {

    }
}
