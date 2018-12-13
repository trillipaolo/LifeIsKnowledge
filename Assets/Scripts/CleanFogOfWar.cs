using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanFogOfWar : MonoBehaviour {

    public float updateTime = 0.5f;
    public GameObject fogPlane;
    public Transform joel;
    public LayerMask fogLayer;
    public float cleanRadius = 5f;
    private float cleanRadiusSquare { get { return cleanRadius * cleanRadius; } }

    private Mesh _mesh;
    private Vector3[] _vertices;
    private Color[] _colors;

    void Start() {
        Initialize();
        InvokeRepeating("UpdateFogOfWar", 0f, updateTime);
    }

    private void Initialize() {
        _mesh = fogPlane.GetComponent<MeshFilter>().mesh;
        _vertices = _mesh.vertices;

        _colors = new Color[_vertices.Length];
        for (int i = 0; i < _colors.Length; i++) {
            _colors[i] = Color.black;
        }
        UpdateColors();
    }

    private void UpdateColors() {
        _mesh.colors = _colors;
    }

    /*private void Update() {
        UpdateFogOfWar();
    }*/

    private void UpdateFogOfWar() {
        Ray r = new Ray(transform.position,joel.position - transform.position);
        RaycastHit hit;

        if (Physics.Raycast(r,out hit,1000,fogLayer,QueryTriggerInteraction.Collide)) {
            for (int i = 0; i < _vertices.Length; i++) {
                Vector3 v = fogPlane.transform.TransformPoint(_vertices[i]);
                float dist = Vector3.SqrMagnitude(v - hit.point);
                if (dist < cleanRadiusSquare) {
                    float alpha = Mathf.Min(_colors[i].a,dist / cleanRadiusSquare);
                    _colors[i].a = alpha;
                }
            }
            UpdateColors();
        }
    }
}
