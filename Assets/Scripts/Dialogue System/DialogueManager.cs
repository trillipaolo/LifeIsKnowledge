using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour {

    public static DialogueManager Instance { get; private set; }
    
    public DialogueText[] beginLevelDialogues;
    public DialogueText[] endLevelDialogues;
    public DialogueText[] droneAdviceDialogues;
    public DialogueText[] joelDialogues;
    public Text displayDialogue;
    public TextMeshProUGUI displayDialogueTMP;
    public float timeToDisappear;

    //Control variables
    private int _beginLevelNR;
    private int _endLevelNR;
    private int _droneAdviceNR;
    private int _joelNR;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        _beginLevelNR = 0;
        _endLevelNR = 0;
        _droneAdviceNR = 0;
        _joelNR = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayDialogue(DialogueType dialogueType)
    {
        switch(dialogueType)
        {
            case DialogueType.BEGINLEVEL:
                {
                    PlayBeginLevelDialogue();
                    break;
                }
            case DialogueType.ENDLEVEL:
                {
                    PlayEndLevelDialogue();
                    break;
                }
            case DialogueType.ADVICE_DRONE:
                {
                    PlayDroneAdviceDialogue();
                    break;
                }
            case DialogueType.JOEL:
                {
                    PlayJoelDialogue();
                    break;
                }
        }
    }

    private void PlayBeginLevelDialogue()
    {
        //Search for "First time" dialogues (Not repeatable ones)
        foreach (DialogueText dialogue in beginLevelDialogues)
        {
            if (dialogue.repeatable == false)
            {
                if(dialogue.played == false)
                {
                    StartCoroutine("TypeDialogue", dialogue.text);
                    dialogue.played = true;
                    _beginLevelNR += 1;
                    return;
                }
            }
        }

        int randomSelect = Random.Range(_beginLevelNR, beginLevelDialogues.Length);

        StartCoroutine("TypeDialogue", beginLevelDialogues[randomSelect].text);
    }

    private void PlayEndLevelDialogue()
    {
        //Search for "First time" dialogues (Not repeatable ones)
        foreach (DialogueText dialogue in endLevelDialogues)
        {
            if (dialogue.repeatable == false)
            {
                if (dialogue.played == false)
                {
                    StartCoroutine("TypeDialogue", dialogue.text);
                    dialogue.played = true;
                    _endLevelNR += 1;
                    return;
                }
            }
        }

        int randomSelect = Random.Range(_endLevelNR, endLevelDialogues.Length);

        StartCoroutine("TypeDialogue", endLevelDialogues[randomSelect].text);
    }
       
    private void PlayDroneAdviceDialogue()
    {
        int randomSelect = Random.Range(_droneAdviceNR, droneAdviceDialogues.Length);

        StartCoroutine("TypeDialogue", droneAdviceDialogues[randomSelect].text);
    }

    private void PlayJoelDialogue()
    {
        int randomSelect = Random.Range(_joelNR, joelDialogues.Length);

        StartCoroutine("TypeDialogue", joelDialogues[randomSelect].text);
    }

    IEnumerator TypeDialogue(string dialogue)
    {
        displayDialogue.text = "";
        displayDialogueTMP.text = "";
        foreach (char letter in dialogue.ToCharArray())
        {
            //displayDialogue.text += letter;
            displayDialogueTMP.text += letter;
            yield return null;
        }

        yield return new WaitForSeconds(timeToDisappear);

        displayDialogue.text = "";
        displayDialogueTMP.text = "";
    }
}
