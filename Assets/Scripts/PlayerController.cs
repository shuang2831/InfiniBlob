using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private float speed;
    private Rigidbody rb;
    Animator anim;
    float timeLimit = 3.05f; // About 3 seconds.
    public Vector3 dir;
    public new SkinnedMeshRenderer renderer;

    public GameObject weapon;
    public GameObject armBone;
    public GameObject gun;
    public GameObject gunArmBone;   
    PlayerAttack playerAttack;
    PlayerHealth playerHealth;
    UIBehaviour uiCanvas;

    public bool isMoving;
    public bool isKnockback = false;

    Coroutine coFlash;

    private IWeapon iWeapon;

    private float chargeTimer;

    public Camera camera;
    private float chargeFactor;
    private float timeBeforeAttack;

    void Start()
    {

        // Get relevant components
        uiCanvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<UIBehaviour>();
        renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
        playerHealth = GetComponent<PlayerHealth>();

        // Set initial booleans
        isMoving = true;
        rb.freezeRotation = true; // freeze physics from rotating our body
        speed = 8f;

        // Set initial weapon
        weapon.transform.parent = armBone.transform;
        weapon.transform.localPosition = new Vector3(0.0024f, 0.0004f, -0.0033f);
        weapon.transform.localEulerAngles = new Vector3(5.547f, -56.104f, -25.473f);

        // Set initial weapon
        gun.transform.parent = gunArmBone.transform;
        gun.transform.localPosition = new Vector3(0.00227f, 0.00388f, -0.00114f);
        gun.transform.localEulerAngles = new Vector3(-182.249f, -132.093f, -86.55298f);
        // gun.transform.localRotation = new Quaternion(0, 50f, 50f, 0);

        iWeapon = gameObject.AddComponent<Crossbow>();
        chargeTimer = 0;
        chargeFactor = 1;
        timeBeforeAttack = 0;
    }

    void FixedUpdate()
    {
        // Get input movements
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // If we're in knockback state
        if (isKnockback)
        {
            takeHit();
        }
        if (Input.GetKey("q") && timeBeforeAttack > 1)
        {
            chargeTimer += Time.deltaTime*chargeFactor;
            uiCanvas.setCharge(chargeTimer);
        }
            // Calculate direction of inputs
            Vector3 direction = new Vector3(moveHorizontal, 0.0f, moveVertical);
        if (moveHorizontal != 0 || moveVertical != 0)
        {
            // If we're able to move, move the player accordingly with the correct rotation
            if (isMoving)
            {
                
                if (!Input.GetKey("q") && !Input.GetKeyUp("q"))
                {
                    rb.MovePosition(transform.position + (direction.normalized * speed * Time.deltaTime));
                    Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
                    rb.MoveRotation(rotation);
                    //Debug.Log(chargeTimer);
                    if (!(anim.GetCurrentAnimatorStateInfo(0).IsName("weapon_swing")) && !(anim.GetCurrentAnimatorStateInfo(0).IsName("gun"))) // If not swinging
                    {
                        anim.Play("Walk");
                    }
                }
                else if(timeBeforeAttack > 1)
                {
                    rb.MovePosition(transform.position + (direction.normalized * speed/2 * Time.deltaTime));
                    anim.Play("walkingGun");
                }
                
            }
        }
        else
        {
            // if standing, play idle animation
            if (!(anim.GetCurrentAnimatorStateInfo(0).IsName("weapon_swing")) && !(anim.GetCurrentAnimatorStateInfo(0).IsName("gun")))
            {
                anim.Play("Idle");
            }
        }
    }
    void Update()
    {

        // If press e, attack
        if (rb.isKinematic)
        {
            playerAttack.attackEnemy();
            rb.isKinematic = false;
                
        }else
        {
            timeBeforeAttack += Time.deltaTime;
            //Debug.Log(timeBeforeAttack);
        }
        if (Input.GetKeyDown("space"))
        {
            playerAttack.attackEnemy();
            rb.isKinematic = true;
            anim.Play("weapon_swing");         
        }
        if (Input.GetKeyUp("q") && timeBeforeAttack > 1)
        {
            iWeapon.Shoot(uiCanvas.barVal());
            anim.Play("Idle");
            chargeTimer = 0;
            uiCanvas.setCharge(chargeTimer);
            timeBeforeAttack = 0;
        }else
        {
            timeBeforeAttack += Time.deltaTime;
            //Debug.Log(timeBeforeAttack);
        }
        if (Input.GetKeyDown("q") && timeBeforeAttack > 1)
        {
            //playerAttack.attackEnemy();
            anim.Play("gun");
            //gun.GetComponent <PlayerShooting>().Shoot();
            
        }
        //if (Input.GetKeyUp("q")){
        //    iWeapon.Shoot();
        //    anim.Play("Idle");
        //}
       
        uiCanvas.setCursor(camera.WorldToScreenPoint((transform.forward*1.7f) + rb.position), transform.localEulerAngles.y);
        
        
    }
    
    /**
     * takeHit() is a function that simulates the knockback effect after touching an enemy. It uses timeLimit to determine how long to be knocked back
     * and in what direction given by the vecotr dir. Dir is set in PlayerHealth in case of any confusion. Is this the best way of doing this?
     * Probably not, will improve it later.
     */
    public void takeHit()
    {
        if (timeLimit > 3)
        {
            // Decrease timeLimit.
            timeLimit -= Time.deltaTime;
            dir.y = 0;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, 10);
        }
        else if (timeLimit > 1.0f)
        {
            // do nothing but decrease timeLimit.
            timeLimit -= Time.deltaTime;
            isMoving = true;
        }
        else
        {
            StopCoroutine(coFlash); // stop flashing
            renderer.enabled = true; // make sure renderer is on
            isKnockback = false; // not in knockback anymore
            timeLimit = 3.05f; // reset timeLimit
        }
    }

    /**
     * startFlash() creates and instantiates a coroutine called Flash under coFlash 
     */
    public void startFlash()
    {
        coFlash = StartCoroutine(Flash(5f, 0.1f));
    }

    /**
     * Flash is a coroutine which causes the model renderer to turn on and off repeatedly to give of the
     * flashing frames look when a player is hit
     */
    IEnumerator Flash(float time, float intervalTime)
    {
        float elapsedTime = 0f;
        int index = 0;
        while (elapsedTime < time)
        {
            renderer.enabled = ((index % 2) == 0);//colors[index % 2];

            elapsedTime += Time.deltaTime;
            index++;
            yield return new WaitForSeconds(intervalTime);
        }
    }
    /**
     * OnTriggerEnter decides what happens when the player hits a triggered object. For now it only deals with coins
     * which should send a call to the UI that the score has gone up and destroy the coin.
     */
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("coin"))
        {
            other.gameObject.SetActive(false);
            uiCanvas.addScore(1);
            Destroy(other.gameObject);
        }
        if(other.gameObject.CompareTag("heart"))
        {
            other.gameObject.SetActive(false);
            playerHealth.currentHealth++;
            uiCanvas.setHealth(playerHealth.currentHealth);
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("powerUpSpeed"))
        {
            speed = speed + 0.5f;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("powerUpPower"))
        {
            playerAttack.attackDamage = playerAttack.attackDamage + 5;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("powerUpRate"))
        {
            chargeFactor += 0.5f;
            Destroy(other.gameObject);
        }
    }
}