using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerPhysics))]
public class PlayerInput : MonoBehaviour {

	PlayerPhysics player;
	private bool  _inputDisabled;


    void Start ()
	{
		player = GetComponent<PlayerPhysics>();

    }
	
	void Update () {
		if(!_inputDisabled)
		{
			Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
			player.SetDirectionalInput(directionalInput);

			if (Input.GetButtonDown("Jump"))
			{
				player.OnJumpInputDown();
			}

			if (Input.GetButtonUp("Jump"))
			{
				player.OnJumpInputUp();
			}

			if (Input.GetButtonDown("Roll"))
			{
				player.Roll();
			}
		}
	}

	public void DisableInput()
	{
		_inputDisabled = true;
	}
}
