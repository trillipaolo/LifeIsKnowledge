using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextController : MonoBehaviour {
    private static FloatingText popupText;
    private static GameObject canvas;

	public static void Initialize() {
        Debug.Log("initialize");
        canvas = GameObject.Find("Canvas");
        if (!popupText) {
            popupText = Resources.Load<FloatingText>("Prefabs/PopupTextParent");
        }
    }
	
	public static void CreateFloatingText(string text, Transform location) {
        Debug.Log("create floating text");
        FloatingText instance = Instantiate(popupText);
        instance.transform.SetParent(canvas.transform, false);
        instance.SetText(text);
    }
}
