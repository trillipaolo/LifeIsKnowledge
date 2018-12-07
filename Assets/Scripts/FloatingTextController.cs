using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextController : MonoBehaviour {
    private static FloatingText popupText;
    private static GameObject canvas;

	public static void Initialize() {
        canvas = GameObject.Find("HUDCanvas");
        if (!popupText) {
            popupText = Resources.Load<FloatingText>("Prefabs/PopupTextParent");
        }
    }
	
	public static void CreateFloatingText(string text, Vector3 location, Color color) {
        FloatingText instance = Instantiate(popupText);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(location);

        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = screenPosition;
        instance.SetText(text);
        instance.GetComponentInChildren<Text>().color = color;
    }
}
