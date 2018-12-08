using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/DialogueText")]
public class DialogueText : ScriptableObject {

    [Header("Dialogue Properties")]
    public DialogueType dialogueType;

    [TextArea()]
    public string text;
}
