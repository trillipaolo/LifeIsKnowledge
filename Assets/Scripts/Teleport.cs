using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    //End point of the Teleport
    public GameObject endPosition;

    //Player GameObject
    public GameObject target;

    //Teleport Kind
    public TeleportType teleportType;

    //True if the player is in front of the stairs (Colliders are actually colliding)
    //False otherwise
    private bool _teleport;
    private bool _DPadInUse;

    private bool _lastTeleport = false;

    private void Awake()
    {
        _teleport = false;
        _DPadInUse = true;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == target)
        {   
            _teleport = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == target)
        {
            _teleport = false;
        }
    }

    private void Update()
    {
        bool _teleportInput = Input.GetAxis("Teleport") > 0;

        if(_teleportInput && !_lastTeleport && _teleport)
        {
            StartCoroutine("Teleporting");
        }

        _lastTeleport = _teleportInput;
    }

    IEnumerator Teleporting()
    {
        yield return new WaitForSeconds(0.01f);

        target.transform.position = endPosition.GetComponent<Transform>().position;

    }
}
