using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Coordinates")]
public class Coordinates : ScriptableObject {

    [SerializeField]
    private float x;

    [SerializeField]
    private float y;

    public float GetX()
    {
        return x;
    }

    public float GetY()
    {
        return y;
    }
}
