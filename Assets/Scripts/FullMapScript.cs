using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullMapScript : MonoBehaviour {

    public Camera minimapCamera;
    private RectTransform _rect;
    private Image _border;

    private bool _isMinimap = true;

    void Awake() {
        _rect = GetComponent<RectTransform>();
        _border = transform.Find("MinimapImage").Find("Border").GetComponent<Image>();
    }
    
    void Update () {
        if (Input.GetKeyDown(KeyCode.M)) {
            if (_isMinimap) {
                SetAsFullmap();
                _isMinimap = false;
            } else {
                SetAsMinimap();
                _isMinimap = true;
            }
        }
    }

    private void SetAsFullmap() {
        _rect.anchorMin = new Vector2(0,0);
        _rect.anchorMax = new Vector2(1,1);
        _rect.pivot = new Vector2(0,0);
        _rect.anchoredPosition = new Vector2(0,0);

        _border.enabled = false;

        minimapCamera.transform.position = new Vector3(47,71,-10);
        minimapCamera.orthographicSize = 120;
        Time.timeScale = 0.01f;
    }

    private void SetAsMinimap() {
        _rect.anchorMin = new Vector2(1,0);
        _rect.anchorMax = new Vector2(1,0);
        _rect.pivot = new Vector2(1,0);
        _rect.anchoredPosition = new Vector2(-28,30);

        _border.enabled = true;

        minimapCamera.transform.localPosition = new Vector3(0,0,-10);
        minimapCamera.orthographicSize = 50;
        Time.timeScale = 1;
    }
}
