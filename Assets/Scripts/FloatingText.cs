using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TMPro {

    public class FloatingText : MonoBehaviour {

        public Animator animator;

        void Awake() {
            AnimatorClipInfo[] animatorInfo = animator.GetCurrentAnimatorClipInfo(0);
            Destroy(gameObject,animatorInfo[0].clip.length);
        }

        public void SetText(string text) {
            if (animator.GetComponent<Text>()) {
                animator.GetComponent<Text>().text = text;
            } else if (animator.GetComponent<TextMeshProUGUI>()) {
                animator.GetComponent<TextMeshProUGUI>().text = text;
            }
        }
    }
}
