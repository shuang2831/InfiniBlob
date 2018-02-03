using UnityEngine;
using System.Collections;

public class BlobController : MonoBehaviour {

    public float Distance;
    private Transform Target;
    private float lookAtDistance;
    private float chaseRange = 10.0f;
    private float moveSpeed;
    private int rotationSpeed = 5;
    private Rigidbody rb;
    //Animator anim;
    public bool isMoving;
    public bool isKnockback;

    public Texture[] textures;

    private EnemyHealth enemyHealth;

    Vector3 dir;
    float timeLimit = 2.2f; // 10 seconds.

    public new SkinnedMeshRenderer renderer;
    private Color[] colors = { Color.white, Color.red };
    public Color chosenColor;
    public int chosenIdx; // green, yellow, blue, white, pink
    private Color[] blobColors = { new Color(0.53f, 1.0f, 0.3f, 1.0f), new Color(1.0f, 1.0f, 0.5f, 1.0f), new Color(0.3f, 0.79f, 1.0f, 1.0f), Color.white, new Color(1.0f, 0.4f, 0.7f, 1.0f) };
    Coroutine coFlash;
    float savedTime;
    float timeLeft;

    //public Material mat;

    void Awake()
    {
        renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        chosenIdx = Random.Range(0, blobColors.Length);
        chosenColor = blobColors[chosenIdx];
        renderer.material.color = chosenColor;
        colors[0] = chosenColor;

        switch (chosenIdx)
        {
            case 0: // green
                moveSpeed = 3.0f;
                lookAtDistance = 10.0f;
                break;
            case 1: // yellow
                moveSpeed = 3.0f;
                lookAtDistance = 10.0f;
                break;
            case 2: // blue
                moveSpeed = 2.0f;
                lookAtDistance = 10.0f;
                break;
            case 3: // white
                moveSpeed = 4.0f;
                lookAtDistance = 10.0f;
                break;
            case 4: // pink
                moveSpeed = 3.0f;
                lookAtDistance = 25.0f;
                break;
            default:
                moveSpeed = 3.0f;
                lookAtDistance = 10.0f;
                break;
        }
        //Debug.Log(colors[0]);
    }
 
     void Start () 
     {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        //renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        enemyHealth = GetComponent<EnemyHealth>();
        //mat = GetComponentInChildren<Material>();
        rb = GetComponent<Rigidbody>();
        //anim = GetComponent<Animator>();
        rb.freezeRotation = true;
        isMoving = true;
        isKnockback = false;
        savedTime = Time.time;
        timeLeft = 3.0f;

        Vector3 randomLook = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        transform.rotation = Quaternion.LookRotation(randomLook.normalized, Vector3.up);

        //target = GameObject.Find("Player").transform;
    }
 
     void Update ()
     {
        if (transform.position.y < -10)
        {
            Destroy(gameObject, 5.0f);
        }
        switch (chosenIdx)
        {
            case 0: // green
                greenLogic();
                break;
            case 1: // yellow
                yellowLogic();
                break;
            case 3: // white
                whiteLogic();
                break;
            case 4: // pink
                pinkLogic();
                break;
            default:
                greenLogic();
                break;
        }







       
         

     }

     IEnumerator Flash(float time, float intervalTime)
     {
         float elapsedTime = 0f;
         int index = 0;
         while (elapsedTime < time)
         {
             renderer.material.color = colors[index % 2];

             elapsedTime += Time.deltaTime;
             index++;
             yield return new WaitForSeconds(intervalTime);
         }
     }
     public void startFlash()
     {
         coFlash = StartCoroutine(Flash(5f, 0.05f));
     }
     public void flashOnce()
     {
         renderer.material.color = Color.red;
         Invoke("resetColor", 0.1f);
     }
     void resetColor()
     {
        renderer.material.color = chosenColor;
     }

     // Turn to face the player.
     void lookAt()
     {
         // Rotate to look at player.
         Quaternion rotation = Quaternion.LookRotation(Target.position - transform.position);
         rotation.z = 0;

         transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime*rotationSpeed);
         //transform.LookAt(Target); alternate way to track player replaces both lines above.
     }
    void lookAway()
    {
        // Rotate to look at player.
        Quaternion rotation = Quaternion.LookRotation(transform.position - Target.position);
        rotation.z = 0;

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        //transform.LookAt(Target); alternate way to track player replaces both lines above.
    }

    void wander()
    {
        
        if (Time.time - savedTime <= 2.2 && transform.position.y < 30)
        {
            //transform.rotation = Random.rotation;
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }
        else if (Time.time - savedTime >= 2.2 && Time.time - savedTime <= 5)
        {
            
        }
        else if (Time.time - savedTime > 5)
        {
            Vector3 randomLook = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            transform.rotation = Quaternion.LookRotation(randomLook.normalized, Vector3.up);
            savedTime = Time.time;
        }
    }



    private void greenLogic()
    {
        // Gauge the distance to the player. Line in 3d space. Draws a line from source to Target.
        Distance = Vector3.Distance(Target.position, transform.position);
        //renderer.material.color = Color.white;

        if (isKnockback)
        {
            //Debug.Log(timeLimit);
            //Debug.Log(timeLimit);

            if (timeLimit > 2)
            {

                // Decrease timeLimit.
                timeLimit -= Time.deltaTime;
                dir = (transform.position - Target.position).normalized;
                dir.y = 0;
                transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, 10 * Time.deltaTime);
            }
            else if (timeLimit > 1.5)
            {
                // do nothing
                // Decrease timeLimit.
                timeLimit -= Time.deltaTime;


            }
            else
            {
                if (enemyHealth.currentHealth <= 0)
                {
                    enemyHealth.Death();
                    renderer.enabled = false;
                    StopCoroutine(coFlash);
                }


                isKnockback = false;
                isMoving = true;
                timeLimit = 2.2f;
            }
        }

        // AI begins tracking player.
        if (Distance < lookAtDistance)
        {
            lookAt();

        }


        // Attack! Chase the player until/if player leaves attack range.
        if (Distance < chaseRange && transform.position.y < 30)
        {
            

            //move towards target
            if (isMoving == true)
            {
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }

        }
        else
        {
            wander();
            //chargeSpeed = moveSpeed;
            // rb.AddForce(Vector3.zero, ForceMode.VelocityChange);
        }
    }

    private void yellowLogic()
    {
        // Gauge the distance to the player. Line in 3d space. Draws a line from source to Target.
        Distance = Vector3.Distance(Target.position, transform.position);
        //renderer.material.color = Color.white;

        if (isKnockback)
        {
            //Debug.Log(timeLimit);
            //Debug.Log(timeLimit);

            
            
            if (timeLimit > 1.5)
            {
                // do nothing
                // Decrease timeLimit.
                timeLimit -= Time.deltaTime;


            }
            else
            {
                if (enemyHealth.currentHealth <= 0)
                {
                    transform.localScale = new Vector3(0.5F, 0.5F, 0.5F);
                    enemyHealth.Death();
                    renderer.enabled = false;
                    StopCoroutine(coFlash);
                }


                isKnockback = false;
                isMoving = true;
                timeLimit = 2.2f;
            }
        }

        // AI begins tracking player.
        if (Distance < lookAtDistance)
        {
            lookAt();

        }


        // Attack! Chase the player until/if player leaves attack range.
        if (Distance < chaseRange && transform.position.y < 30)
        {
            
            transform.localScale += new Vector3(0.005F, 0.005F, 0.005F);
            rb.mass += 0.1f;


        }
        else
        {
            wander();
            //chargeSpeed = moveSpeed;
            // rb.AddForce(Vector3.zero, ForceMode.VelocityChange);
        }
    }
    private void pinkLogic()
    {
        // Gauge the distance to the player. Line in 3d space. Draws a line from source to Target.
        Distance = Vector3.Distance(Target.position, transform.position);
        //renderer.material.color = Color.white;

        if (isKnockback)
        {
            //Debug.Log(timeLimit);
            //Debug.Log(timeLimit);

            if (timeLimit > 2)
            {

                // Decrease timeLimit.
                timeLimit -= Time.deltaTime;
                dir = (transform.position - Target.position).normalized;
                dir.y = 0;
                transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, 10 * Time.deltaTime);
            }
            else if (timeLimit > 1.5)
            {
                // do nothing
                // Decrease timeLimit.
                timeLimit -= Time.deltaTime;


            }
            else
            {
                if (enemyHealth.currentHealth <= 0)
                {
                    enemyHealth.Death();
                    renderer.enabled = false;
                    StopCoroutine(coFlash);
                }


                isKnockback = false;
                isMoving = true;
                timeLimit = 2.2f;
            }
        }

        // AI begins tracking player.
        if (Distance < lookAtDistance)
        {
            lookAt();

        }


        // Attack! Chase the player until/if player leaves attack range.
        if (Distance < chaseRange && transform.position.y < 30)
        {
            //Debug.Log("enemy chase");
            //chargeSpeed = (chargeSpeed<0) ? moveSpeed : chargeSpeed - 0.01f;
            //rb.AddForce( (Vector3.Normalize((Target.position - transform.position)) * chargeSpeed));
            //rb.velocity = Vector3.Normalize((Target.position - transform.position))* chargeSpeed;
            //chase();

            //move towards target
            
            if (true)
            {
                timeLeft -= Time.deltaTime;
                if (timeLeft < 0.0f)
                {
                    rb.AddForce(transform.forward * 25, ForceMode.Impulse);//transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    timeLeft = 3.0f;
                }
                //rb.AddForce(transform.forward * 100);//transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
            


        }
        else
        {
            wander();
            //chargeSpeed = moveSpeed;
            // rb.AddForce(Vector3.zero, ForceMode.VelocityChange);
        }
    }
    private void whiteLogic()
    {
        // Gauge the distance to the player. Line in 3d space. Draws a line from source to Target.
        Distance = Vector3.Distance(Target.position, transform.position);
        //renderer.material.color = Color.white;

        if (isKnockback)
        {
            //Debug.Log(timeLimit);
            //Debug.Log(timeLimit);

            if (timeLimit > 2)
            {

                // Decrease timeLimit.
                timeLimit -= Time.deltaTime;
                dir = (transform.position - Target.position).normalized;
                dir.y = 0;
                transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, 10 * Time.deltaTime);
            }
            else if (timeLimit > 1.5)
            {
                // do nothing
                // Decrease timeLimit.
                timeLimit -= Time.deltaTime;


            }
            else
            {
                if (enemyHealth.currentHealth <= 0)
                {
                    enemyHealth.Death();
                    renderer.enabled = false;
                    StopCoroutine(coFlash);
                }


                isKnockback = false;
                isMoving = true;
                timeLimit = 2.2f;
            }
        }

        // AI begins tracking player.
        if (Distance < lookAtDistance)
        {
            lookAway();

        }


        // Attack! Chase the player until/if player leaves attack range.
        if (Distance < chaseRange && transform.position.y < 30)
        {
            //Debug.Log("enemy chase");
            //chargeSpeed = (chargeSpeed<0) ? moveSpeed : chargeSpeed - 0.01f;
            //rb.AddForce( (Vector3.Normalize((Target.position - transform.position)) * chargeSpeed));
            //rb.velocity = Vector3.Normalize((Target.position - transform.position))* chargeSpeed;
            //chase();

            //move towards target
            if (isMoving == true)
            {
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }

        }
        else
        {
            wander();
            //chargeSpeed = moveSpeed;
            // rb.AddForce(Vector3.zero, ForceMode.VelocityChange);
        }
    }
}
