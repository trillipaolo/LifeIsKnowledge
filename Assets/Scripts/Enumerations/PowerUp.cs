using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    [Header("Reference to Joel Basic Attack Damage")]
    public BasicAttack basicAttack;

    [Header("Reference to Joel Combos Damage")]
    public Combo[] combos;


    [Header("Reference to Joel Health Scriptable Object")]
    public Health health;

    [Header("Reference to Joel Health Component")]
    public JoelHealth joelHealth;

    [Header("Type of PowerUp")]
    public PowerUpType powerUpType;

    [Header("PowerUp Multiplier")]
    public float damageMultiplier;
    public int healthAugment;

    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void IncreaseAttack()
    {   
        //Increase damage of the basic attacks
        for (int i = 0; i < basicAttack.damage.Length; i++)
        {
            basicAttack.damage[i] = basicAttack.damage[i] * damageMultiplier;
        }

        //Increase damage of the combos
        for (int i = 0; i < combos.Length; i++)
        {
            for(int j = 0; j < combos[i].damage.Length; j++)
            {
                combos[i].damage[j] = combos[i].damage[j] * damageMultiplier;
            }
        }
    }

    public void IncreaseHealth()
    {
        health.health = health.health + healthAugment;

    }
}
