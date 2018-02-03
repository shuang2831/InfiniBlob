using UnityEngine;
using System.Collections;


public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;     // The time in seconds between each attack.
    private int attackDamage = 1;               // The amount of health taken away per attack.

    //Animator anim;                              // Reference to the animator component.
    GameObject player;                          // Reference to the player GameObject.
    PlayerHealth playerHealth;                  // Reference to the player's health.
    EnemyHealth enemyHealth;                    // Reference to this enemy's health.
    bool playerInRange;                         // Whether player is within the trigger collider and can be attacked.
    float timer;                                // Timer for counting up to the next attack.
    BlobController blobController;
    PlayerController playerController;
    GameObject Ground;


    void Awake ()
    {
        Ground = GameObject.FindGameObjectWithTag("ground");
        Physics.IgnoreCollision(GetComponent<SphereCollider>(), Ground.GetComponent<TerrainCollider>());
        // Setting up the references.
        player = GameObject.FindGameObjectWithTag ("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        playerController = player.GetComponent<PlayerController>();
        enemyHealth = GetComponent<EnemyHealth>();
        blobController = GetComponent<BlobController>();
        //anim = GetComponent <Animator> ();
    }

   

    void OnCollisionEnter (Collision other)
    {
        // If the entering collider is the player...
        if(other.gameObject == player && !blobController.isKnockback && !playerController.isKnockback)
        {
            //Debug.Log("entered");
            // ... the player is in range.
            playerInRange = true;
            Vector3 dir = (other.transform.position - transform.position).normalized;
            playerHealth.KnockBack(dir); // knock player back
          
        }
    }


    void OnCollisionExit (Collision other)
    {
        // If the exiting collider is the player...
        if(other.gameObject == player)
        {
            // ... the player is no longer in range.
            playerInRange = false;
        }
    }


    void Update ()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
        if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        {
            // ... attack.
            Attack();
        }

        // If the player has zero or less health...
        if (playerHealth.currentHealth <= 0)
        {
            // ... tell the animator the player is dead.
            //anim.SetTrigger("PlayerDead");
        }
    }


    void Attack()
    {
        // Reset the timer.
        timer = 0f;

        // If the player has health to lose...
        if (playerHealth.currentHealth > 0)
        {
            // ... damage the player.
            playerHealth.TakeDamage(attackDamage);
        }
    }
}