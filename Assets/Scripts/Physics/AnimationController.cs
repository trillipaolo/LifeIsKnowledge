using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
	private Animator _animator;

	private DroneMovement _physics;
	// Use this for initialization
	void Start ()
	{
		_animator = GetComponent<Animator>();
		_physics = GetComponentInParent<DroneMovement>();
	}
	
	// Update is called once per frame
	void Update () {
		if (_physics.foundTarget)
		{
			_animator.SetBool("FoundTarget", true);
		}
	}

	private void DisableMovement()
	{
		_physics.DisableMovement();
	}

	private void EnableMovement()
	{
		_physics.EnableMovement();
	}
}
