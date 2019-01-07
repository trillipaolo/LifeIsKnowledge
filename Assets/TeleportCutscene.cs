using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCutscene : MonoBehaviour {

    private bool _teleport = false;

	// Update is called once per frame
	void Update () {
		if (_teleport)
        {
            bool teleportInput = Input.GetAxis("Teleport") > 0.75f;

            if (teleportInput)
            {

            }
        }
	}

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == GameObject.FindGameObjectWithTag("Player"))
        {
            _teleport = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == GameObject.FindGameObjectWithTag("Player"))
        {
            _teleport = false;
        }
    }
}
