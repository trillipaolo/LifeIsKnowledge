using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour {

    public Animator animator;

	void Awake () {
        Debug.Log("aaa");
        AnimatorClipInfo[] animatorInfo = animator.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject,animatorInfo[0].clip.length);
	}
	
	public void SetText(string text) {
        animator.GetComponent<Text>().text = text;
    }
}
