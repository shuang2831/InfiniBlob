using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;     // The time in seconds between each attack.
    public int attackDamage;               // The amount of health taken away per attack.

    Animator anim;                              // Reference to the animator component.
    GameObject player;                          // Reference to the player GameObject.
    PlayerHealth playerHealth;                  // Reference to the player's health.
    float timer;                                // Timer for counting up to the next attack.

    void Start()
    {
        attackDamage = 5;
    }

    /**
     * attackEnemy() handles attacking enemies by instantiating an overlapshpere and checks to see if anything within the sphere's
     * radius is an enemy and is facing the player. If it is, the function tells the enemy to take damage as well as put
     * the enemy in a knockback state.
     */
    public void attackEnemy()
    {
        List<EnemyHealth> enemyList = new List<EnemyHealth>(); // Use a list and make sure no repeats
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.7f);
        int i = 0;
        while (i < hitColliders.Length)
        {
            EnemyHealth enemyHealth = hitColliders[i].GetComponent<EnemyHealth>();
            // If the EnemyHealth component exist...
            if (enemyHealth != null && !enemyList.Contains(enemyHealth))
            {
                enemyList.Add(enemyHealth);
                Vector3 dir = (hitColliders[i].transform.position - transform.position).normalized;
                float angleToEnemy = Vector3.Angle(transform.forward, dir);
                if (Mathf.Abs(angleToEnemy) < 90) // If enemy is in front
                {
                    enemyHealth.TakeDamage(attackDamage); // the enemy should take damage.
                    enemyHealth.enemyKnockBack(dir); // knockback 
                }
            }
            i++;
        }
    }
}