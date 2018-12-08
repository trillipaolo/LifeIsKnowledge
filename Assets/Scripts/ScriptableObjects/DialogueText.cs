using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/DialogueText")]
public class DialogueText : ScriptableObject {

    [Header("Dialogue Properties")]
    public DialogueType dialogueType;
    public bool repeatable;
    public bool played;

    [TextArea()]
    public string text;
}
