using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullMapScript : MonoBehaviour {

    public Camera minimapCamera;
    public Transform target;

    private RectTransform _rect;
    private Image _border;

    private bool _isMinimap = true;
    [SerializeField]
    private Vector2 mapPosition = new Vector2(47,71);
    [SerializeField]
    private float mapZoom = 120;


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

        if (_isMinimap) {
            MoveMinimap();
        }
    }

    private void SetAsFullmap() {
        _rect.anchorMin = new Vector2(0,0);
        _rect.anchorMax = new Vector2(1,1);
        _rect.pivot = new Vector2(0,0);
        _rect.anchoredPosition = new Vector2(0,0);

        _border.enabled = false;

        minimapCamera.transform.position = new Vector3(mapPosition.x, mapPosition.y, -10);
        minimapCamera.orthographicSize = mapZoom;
    }

    private void SetAsMinimap() {
        _rect.anchorMin = new Vector2(1,0);
        _rect.anchorMax = new Vector2(1,0);
        _rect.pivot = new Vector2(1,0);
        _rect.anchoredPosition = new Vector2(-28,30);

        _border.enabled = true;

        minimapCamera.orthographicSize = 50;
    }

    private void MoveMinimap() {
        minimapCamera.transform.position = target.position - Vector3.forward*10;
    }
}
