using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour {

    Rigidbody rb;
    PlayerAttack pa;
    UIBehaviour uiCanvas;
    private float dmg;


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        pa = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        uiCanvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<UIBehaviour>();
        //dmg = uiCanvas.barVal();
        //Debug.Log("dmg" + dmg);
    }
	
	//// Update is called once per frame
	//void Update () {
		
	//}

    void OnTriggerEnter(Collider other)
    {
        // If the entering collider is the player...
        if (other.gameObject.tag == "enemy")
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            BlobController blobController = other.GetComponent<BlobController>();
            if (enemyHealth != null && blobController.isKnockback == false)
            {
                rb.isKinematic = true;
                transform.Translate(0.005f * Vector3.forward);
                transform.parent = other.transform;
                Vector3 dir = (other.transform.position - transform.position).normalized;
                enemyHealth.TakeDamage((int) (dmg * (2 * pa.attackDamage))); // the enemy should take damage.
                //Debug.Log("DAMAGE" + ((int) (dmg * (2 * pa.attackDamage))));
                Debug.Log(pa.attackDamage);
                enemyHealth.enemyKnockBack(dir); // knockback 
            }
        }
    }

    public void setDmg(float val)
    {
        dmg = val;
    }
}
