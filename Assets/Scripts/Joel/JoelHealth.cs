using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    private Collider2D _hitbox;
    private Animator _animator;                                              // Reference to the Animator component.
    //public AudioSource playerAudio;                                    // Reference to the AudioSource component.
    //PlayerMovement playerMovement;                              // Reference to the player's movement.
    //PlayerShooting playerShooting;                              // Reference to the PlayerShooting script.
    private bool _isDead;                                                // Whether the player is dead.
    private bool _damaged;
    private bool _healed;
    private PlayerInput _input;
    public float timeBetweenReloading = 3f;

    AudioManager audioManager;
    public string joelTakeDmgSound = "JoelDamage";

    private void Awake() {
        // Setting up the references.
        _hitbox = GetComponent<BoxCollider2D>();
        _animator = GetComponentInParent<Animator>();
        //playerAudio = GetComponent<AudioSource>();
        //playerMovement = GetComponent<PlayerMovement>();
        //playerShooting = GetComponentInChildren<PlayerShooting>();

        // Set the initial health of the player.
        currentHealth = startingHealth;
        _input = GetComponentInParent<PlayerInput>();
    }

    private void Start() {
        audioManager = AudioManager.instance;
    }

    void Update () {
        UpdateCollider();

        // If the player has just been damaged...
        if (_damaged) {
            // ... set the colour of the damageImage to the flash colour.
            audioManager.Play(joelTakeDmgSound);
            damageImage.color = flashColour;

            if (_healed) {
                damageImage.color = new Color32(0,255,0,100);
            }
        }
        // Otherwise...
        else {
            // ... transition the colour back to clear.
            damageImage.color = Color.Lerp(damageImage.color,Color.clear,flashSpeed * Time.deltaTime);
        }

        // Reset the damaged flag.
        _damaged = false;
        _healed = false;
    }

    private void UpdateCollider() {
        bool _isRolling = _animator.GetCurrentAnimatorStateInfo(0).IsName("Roll") || _animator.GetCurrentAnimatorStateInfo(0).IsName("RollExit");
        if (_hitbox.enabled && _isRolling){
            _hitbox.enabled = false;
        } else if(!_hitbox.enabled && !_isRolling){
            _hitbox.enabled = true;
        }
    }

    public void TakeDamage(int amount) {
        if (Time.timeSinceLevelLoad - _lastTimeHit > timeBetweenDamage) {
            // Set the damaged flag so the screen will flash.
            _damaged = true;
            if(amount < 0) {
                _healed = true;
            }

            // Reduce the current health by the damage amount.
            currentHealth -= amount;

            currentHealth = currentHealth > startingHealth ? startingHealth : currentHealth;

            // Set the health bar's value to the current health.
            healthSlider.value = currentHealth;

            // Play the hurt sound effect.
            //playerAudio.Play();

            // If the player has lost all it's health and the death flag hasn't been set yet...
            if (currentHealth <= 0 && !_isDead) {
                // ... it should die.
                Death();
            }

            _lastTimeHit = Time.timeSinceLevelLoad;
        }
    }

    void Death() {
        // Set the death flag so this function won't be called again.
        _isDead = true;
        _animator.SetBool("Dead", true);
        _input.DisableInput();
        PlayerPhysics pl = GetComponentInParent<PlayerPhysics>();
        pl.directionalInput.x = 0;
        pl.velocity.x = 0;
        
        Invoke("ReloadScene", timeBetweenReloading);
        
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

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
