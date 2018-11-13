using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AnimationComboMenuPlaceHolder : MonoBehaviour {

    private Sprite[] placeHolderArray;
    private int index;

    private void Awake()
    {
        placeHolderArray = new Sprite[3];
        index = 0;
        ImportPlaceHolder();
    }

    private void ImportPlaceHolder()
    {
        placeHolderArray[0] = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Sprites/AnimationComboMenuPlaceHolder.png", typeof(Sprite));
        placeHolderArray[1] = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Sprites/AnimationComboMenuPlaceHolder2.png", typeof(Sprite));
        placeHolderArray[2] = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Sprites/AnimationComboMenuPlaceHolder3.png", typeof(Sprite));
    }

    public void SwitchPlaceHolderRight()
    {
        index++;
        if (index > 2)
        {
            index = 0;
        }
        this.GetComponent<SpriteRenderer>().sprite = placeHolderArray[index];
    }
    
    public void SwitchPlaceHolderLeft()
    {
        index--;
        if (index < 0)
        {
            index = 2;
        }
        this.GetComponent<SpriteRenderer>().sprite = placeHolderArray[index];
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
