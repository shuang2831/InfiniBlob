using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    private int startingHealth = 3;                            // The amount of health the player starts the game with.
    public int currentHealth;                                   // The current health the player has.
    public Slider healthSlider;                                 // Reference to the UI's health bar.
    public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
    public AudioClip deathClip;                                 // The audio clip to play when the player dies.
    public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.


    Animator anim;                                              // Reference to the Animator component.
    AudioSource playerAudio;                                    // Reference to the AudioSource component.
    PlayerController playerMovement;                              // Reference to the player's movement.
    bool isDead;                                                // Whether the player is dead.
    bool damaged;                                               // True when the player gets damaged.

    UIBehaviour uiCanvas;

    void Awake ()
    {
        // Get Components and references
        uiCanvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<UIBehaviour>();
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerController> ();

        // Set the initial health of the player.
        currentHealth = startingHealth;   
    }

    void Update ()
    {
        // Reset the damaged flag.
        damaged = false;
    }


    public void TakeDamage (int amount)
    {
        // Set the damaged flag so the screen will flash.
        damaged = true;
 
        // Reduce the current health by the damage amount.
        currentHealth -= amount;

        // Play the hurt sound effect.
        //playerAudio.Play ();

        // Set updated Health in UI
        uiCanvas.setHealth(currentHealth);

        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (currentHealth <= 0 && !isDead)
        {
            // ... it should die.
            Death ();
        }
    }

    /**
     * KnockBack should take in the enemy's direction and set all the required parameters for a player to go into
     * a knockback state. It essentially changes a bunch of variables in playerController. Am considering moving
     * this into playerController and calling it from there, probably would make a lot more sense.
     */
    public void KnockBack(Vector3 enemydir)
    {
        playerMovement.isMoving = false; // Don't allow movement
        playerMovement.isKnockback = true; // knockback state is true
        playerMovement.startFlash(); // start flashing frames
        playerMovement.dir = enemydir; // set the enemy's direction
    }



    void Death()
    {
        // Set the death flag so this function won't be called again.
        isDead = true;

        // Tell the animator that the player is dead.
        //anim.SetTrigger ("Die");
        transform.Translate(Vector3.up * 5);
        transform.Rotate(Vector3.forward * 90);
        anim.Play("Idle");

        // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
        //playerAudio.clip = deathClip;
        //playerAudio.Play();

        // Turn off the movement and shooting scripts.
        playerMovement.enabled = false;
    }
  
}