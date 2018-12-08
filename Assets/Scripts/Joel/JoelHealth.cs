﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoelHealth : MonoBehaviour {

    public int startingHealth = 100;                            // The amount of health the player starts the game with.
    public int currentHealth;                                   // The current health the player has.
    public Slider healthSlider;                                 // Reference to the UI's health bar.
    public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
    //public AudioClip deathClip;                                 // The audio clip to play when the player dies.
    public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
    public Color flashColour = new Color(1f,1f,1f,0.4f);
    public float timeBetweenDamage = 0.7f;
    private float _lastTimeHit = -300;

    private Animator _animator;                                              // Reference to the Animator component.
    //public AudioSource playerAudio;                                    // Reference to the AudioSource component.
    //PlayerMovement playerMovement;                              // Reference to the player's movement.
    //PlayerShooting playerShooting;                              // Reference to the PlayerShooting script.
    private bool _isDead;                                                // Whether the player is dead.
    private bool _damaged;

    private void Awake() {
        // Setting up the references.
        _animator = GetComponent<Animator>();
        //playerAudio = GetComponent<AudioSource>();
        //playerMovement = GetComponent<PlayerMovement>();
        //playerShooting = GetComponentInChildren<PlayerShooting>();

        // Set the initial health of the player.
        currentHealth = startingHealth;
    }

    void Update () {
        // If the player has just been damaged...
        if (_damaged) {
            // ... set the colour of the damageImage to the flash colour.
            damageImage.color = flashColour;
        }
        // Otherwise...
        else {
            // ... transition the colour back to clear.
            damageImage.color = Color.Lerp(damageImage.color,Color.clear,flashSpeed * Time.deltaTime);
        }

        // Reset the damaged flag.
        _damaged = false;
    }

    public void TakeDamage(int amount) {
        if (Time.time - _lastTimeHit > timeBetweenDamage) {
            // Set the damaged flag so the screen will flash.
            _damaged = true;

            // Reduce the current health by the damage amount.
            currentHealth -= amount;

            // Set the health bar's value to the current health.
            healthSlider.value = currentHealth;

            // Play the hurt sound effect.
            //playerAudio.Play();

            // If the player has lost all it's health and the death flag hasn't been set yet...
            if (currentHealth <= 0 && !_isDead) {
                // ... it should die.
                Death();
            }

            _lastTimeHit = Time.time;
        }
    }

    void Death() {
        // Set the death flag so this function won't be called again.
        _isDead = true;

        // Turn off any remaining shooting effects.
        //playerShooting.DisableEffects();

        // Tell the animator that the player is dead.
        //anim.SetTrigger("Die");

        // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
        //playerAudio.clip = deathClip;
        //playerAudio.Play();

        // Turn off the movement and shooting scripts.
        //playerMovement.enabled = false;
        //playerShooting.enabled = false;
    }
}