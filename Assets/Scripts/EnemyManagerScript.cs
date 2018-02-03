using UnityEngine;

public class EnemyManagerScript : MonoBehaviour
{
    public PlayerHealth playerHealth;       // Reference to the player's heatlh.
    public GameObject enemy;                // The enemy prefab to be spawned.
    public float spawnTime = 1f;            // How long between each spawn.
    public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.

    private float MinX = -30;
    private float MaxX = 60;
    private float MinY = 0;
    private float MaxY = 75;
    private float MinZ = -15;
    private float MaxZ = 70;

    float createRate = 3.0f, createRateTimer;
    float rateIncrease = 0.1f, initialCreateDelay = 3.0f;
    int callCounter = 0, callsBeforeRateIncrease = 5;


    void Start()
    {
        // Initialize the timer
        createRateTimer = createRate + initialCreateDelay;
    }

    /**
     * SpawnObject essentially spawns an enemy in a random place on the field way 
     * above the player so that it can drop below. 
     */
    void SpawnObject()
    {
        float x = Random.Range(MinX, MaxX);
        //float y = Random.Range(MinY, MaxY);
        float z = Random.Range(MinZ, MaxZ);

        // spawn enemy
        Instantiate(enemy, new Vector3(x, 75, z), Quaternion.identity);
    }
    void Update()
    {
        // decrease timer
        createRateTimer -= Time.deltaTime;

        // If timer finished...
        if (createRateTimer <= 0)
        {
            invokeSpawn();
        }
    }

    /**
     * invokeSpawn() first spawns the object, and then initial time of the timer (after a certain number of spawns)
     * before resetting it again so that it takes less time to spawn a new horde.
     */
    void invokeSpawn()
    {
        SpawnObject();
        callCounter++;
        if (callCounter >= callsBeforeRateIncrease)
        {
            createRate -= rateIncrease;
            callCounter = 0;
        }
        createRateTimer = createRate;
    }
}
