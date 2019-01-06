using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RespawnPoint")]
public class JoelRespawn : ScriptableObject {

    [Header("Coordinates for Respawn: Initial")]
    public float x_initial;
    public float y_initial;

    [Header("Coordinates for Respawn: Current")]
    public float x;
    public float y;

    private float offsetJoelSafeZonePivot = 0.56f;
    private float offsetToAvoidClipping = 0.1f;

	public void SetRespawnPoint(Transform transform)
    {
        x = transform.position.x;
        y = transform.position.y + offsetJoelSafeZonePivot + offsetToAvoidClipping;
    }

    public void ResetRespawnPoint()
    {
        x = x_initial;
        y = y_initial;
    }

    public float GetX()
    {
        return x;
    }

    public float GetY()
    {
        return y;
    }
}
