using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    //End point of the Teleport
    public GameObject endPosition;

    //Player GameObject
    public GameObject player;

    //Teleport Kind
    public TeleportType teleportType;

    //True if the player is in front of the stairs (Colliders are actually colliding)
    //False otherwise
    private bool _teleport;

    private void Awake()
    {
        _teleport = false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {   
            _teleport = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _teleport = false;
        }
    }

    private void Update()
    {
        if (_teleport)
        {      
            if (teleportType == TeleportType.UPWARDS)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    StartCoroutine("Teleporting");
                }
            }
            if (teleportType == TeleportType.DOWNWARDS)
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    StartCoroutine("Teleporting");
                }
            }
        }
    }

    IEnumerator Teleporting()
    {
        yield return new WaitForSeconds(0.01f);

        player.transform.position = endPosition.GetComponent<Transform>().position;

    }
}
