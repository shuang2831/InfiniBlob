using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;            // The amount of health the enemy starts the game with.
    public int currentHealth;                   // The current health the enemy has.
    public float sinkSpeed = 2.5f;              // The speed at which the enemy sinks through the floor when dead.
    public int scoreValue = 10;                 // The amount added to the player's score when the enemy dies.
    public AudioClip deathClip;                 // The sound to play when the enemy dies.
    BlobController blobController;

    //Animator anim;                              // Reference to the animator.
    AudioSource enemyAudio;                     // Reference to the audio source.
    private ParticleSystem[] childrenParticleSytems;
    SphereCollider sphereCollider;            // Reference to the capsule collider.
    bool isDead;                                // Whether the enemy is dead.
    bool isSinking;                             // Whether the enemy has started sinking through the floor.

    private Rigidbody rb;

    GameObject coin;
    GameObject speedCoin;
    GameObject powerCoin;
    GameObject rateCoin;
    GameObject heart;

    UIBehaviour uiCanvas;

    void Awake ()
    {
        // Setting up the references.

        uiCanvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<UIBehaviour>();

        blobController = GetComponent<BlobController>();
        rb = GetComponent<Rigidbody>();
        //sanim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        childrenParticleSytems = GetComponentsInChildren<ParticleSystem>();
        sphereCollider = GetComponent <SphereCollider> ();
       
        coin = (GameObject)Resources.Load("coin");
        speedCoin = (GameObject)Resources.Load("speedCoin");
        powerCoin= (GameObject)Resources.Load("powerCoin");
        rateCoin = (GameObject)Resources.Load("rateCoin") ;
        heart = (GameObject)Resources.Load("heartPre");


        // Setting the current health when the enemy first spawns.
        switch (blobController.chosenIdx)
        {
            case 0: // green
                currentHealth = 15;
                break;
            case 1: // yellow
                currentHealth = 50;
                break;
            case 2: // blue
                currentHealth = 20;
                break;
            case 3: // white
                currentHealth = 10;
                break;
            case 4: // pink
                currentHealth = 10;
                break;
            default:
                currentHealth = 15;
                break;
        }
        // = startingHealth;
    }

    void Update ()
    {
        // If the enemy should be sinking...
        if(isSinking)
        {
            // ... move the enemy down by the sinkSpeed per second.
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }

    private ParticleSystem GetSystem(string systemName)
    {
        foreach (ParticleSystem childParticleSystem in childrenParticleSytems)
        {
            if (childParticleSystem.name == systemName)
            {
                return childParticleSystem;
            }
        }
        return null;
    }

    public void TakeDamage (int amount)//, Vector3 hitPoint)
    {
        //Debug.Log("enemy hit");
        // If the enemy is dead...
        if(isDead)
            // ... no need to take damage so exit the function.
            return;

        // Play the hurt sound effect.
        //enemyAudio.Play ();

        // Reduce the current health by the amount of damage sustained.
        currentHealth -= amount;

        // Set the position of the particle system to where the hit was sustained.
        //hitParticles.transform.position = hitPoint;

        // And play the particles.
        ParticleSystem.EmitParams ep = new ParticleSystem.EmitParams();
        ep.startColor = blobController.chosenColor;
        GetSystem("hitParticle").Emit(ep, 40);
        blobController.flashOnce();

        // If the current health is less than or equal to zero...
        //if(currentHealth <= 0)
        //{
        //    // ... the enemy is dead.
           
        //}
    }

    //public void TakeShotDamage(int amount)//, Vector3 hitPoint)
    //{
    //    // If the enemy is dead...
    //    if (isDead)
    //        // ... no need to take damage so exit the function.
    //        return;

    //    // Play the hurt sound effect.
    //    enemyAudio.Play();

    //    // Reduce the current health by the amount of damage sustained.
    //    currentHealth -= amount;

    //    // Set the position of the particle system to where the hit was sustained.
    //    hitParticles.transform.position = hitPoint;

    //    // And play the particles.
    //    hitParticles.Play();

    //    // If the current health is less than or equal to zero...
    //    if (currentHealth <= 0)
    //    {
    //        // ... the enemy is dead.
    //        Death();
    //    }
    //}

    public void enemyKnockBack(Vector3 enemydir)
    {
        enemydir.y = 0;
        //transform.position += enemydir * 3;
        blobController.isMoving = false;
        blobController.isKnockback = true;
        if (currentHealth <= 0)
        {
            blobController.startFlash();
        }
        //blobController.startFlash();
        //transform.position = Vector3.MoveTowards(transform.position, transform.position + enemydir * 3, 30*Time.deltaTime);
        
        //rb.AddForce(enemydir*200);
        //impact = rb.velocity;
        //blobController.isMoving = false;
        //while (rb.velocity != Vector3.zero)
        //{ 
        //    rb.velocity = Vector3.Lerp(impact, Vector3.zero, Time.deltaTime);
        //    //rb.velocity = impact;
        //}
        //blobController.isMoving = true;
    }

    public void Death()
    {
        // The enemy is dead.
        GetSystem("deathParticle").Emit(25);
        isDead = true;
        uiCanvas.addScore(10);
        int numCoins = 0;

        if (blobController.chosenIdx == 1) // yellow
        {
            numCoins = (int)Mathf.Ceil(rb.mass);
        }
        else if (blobController.chosenIdx == 3) // white
        {
            int prob = Random.Range(0, 10);
            if (prob < 1)
            {
                GameObject pUp = Instantiate(heart, transform.position + Vector3.up, Random.rotation);
            }
            else if (prob < 4)
            {
                GameObject pUp = Instantiate(speedCoin, transform.position + Vector3.up, Random.rotation);
                pUp.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1, 1), 5, Random.Range(-1, 1)), ForceMode.Impulse);
            }
            else if (prob < 7)
            {
                GameObject pUp = Instantiate(powerCoin, transform.position + Vector3.up, Random.rotation);
                pUp.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1, 1), 5, Random.Range(-1, 1)), ForceMode.Impulse);
            }
            else if (prob < 10)
            {
                GameObject pUp = Instantiate(rateCoin, transform.position + Vector3.up, Random.rotation);
                pUp.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1, 1), 5, Random.Range(-1, 1)), ForceMode.Impulse);
            }
        }
        else
        {
            numCoins = Random.Range(0, 11);
        }
        for (int i = 0; i < numCoins; i++)
        {
            
           GameObject c = Instantiate(coin, transform.position, Random.rotation);
           c.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1, 1), 5, Random.Range(-1, 1)) * 5, ForceMode.Impulse);
            
        }
        

        // Turn the collider into a trigger so shots can pass through it.
        sphereCollider.isTrigger = true;

        // Tell the animator that the enemy is dead.
       // anim.SetTrigger("Dead");

        // ... move the enemy down by the sinkSpeed per second.
        transform.Translate(-Vector3.up * sinkSpeed);

        // After 2 seconds destory the enemy.
        Destroy(gameObject, 3f);


        // Change the audio clip of the audio source to the death clip and play it (this will stop the hurt clip playing).
        //enemyAudio.clip = deathClip;
        //enemyAudio.Play();
    }


    public void StartSinking()
    {
        // Find and disable the Nav Mesh Agent.
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;

        // Find the rigidbody component and make it kinematic (since we use Translate to sink the enemy).
        GetComponent<Rigidbody>().isKinematic = true;

        // The enemy should no sink.
        isSinking = true;

        // Increase the score by the enemy's score value.
        //ScoreManager.score += scoreValue;

        // After 2 seconds destory the enemy.
        Destroy(gameObject, 2f);
    }
}