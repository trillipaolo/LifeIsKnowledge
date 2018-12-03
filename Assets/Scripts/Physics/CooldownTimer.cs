using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CooldownTimer : MonoBehaviour
{
	private Image fillImage;
	public float cooldownTime;
	private PlayerPhysics _playerPhysics;
	private float time;
	public bool readyToUse;

	// Use this for initialization
	public void Awake ()
	{
		// TODO: find better solution
		_playerPhysics = FindObjectOfType<PlayerPhysics>();
		cooldownTime = _playerPhysics.rollColdownTime;
		fillImage = this.GetComponent<Image>();
		time = cooldownTime;
		readyToUse = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (time > 0)
		{
			time -= Time.deltaTime;
			fillImage.fillAmount = time / cooldownTime;
		}
		else
		{
			readyToUse = true;
		}
		Destroy(gameObject, cooldownTime);
	}
//	public void 
}
