using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueArea : MonoBehaviour {

    //Player GameObject
    public GameObject target;

    //Dialogue Type of the current area
    public DialogueType dialogueType;

    //Status of the Area
    private bool _used;

    private void Start()
    {
        _used = false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {   
        if (!_used)
        {
            if (other.gameObject == target)
            {
                DialogueManager.Instance.PlayDialogue(dialogueType);
                _used = true;
            }
        }
    }
}
