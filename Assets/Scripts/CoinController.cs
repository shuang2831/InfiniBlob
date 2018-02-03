using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour {
    // Use this for initialization

    Transform player;
    private float dist;
    private float coinSpeed;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        dist = 2.0f;
        coinSpeed = 10.0f;
        Physics.IgnoreCollision(player.GetComponent<CapsuleCollider>(), GetComponent<BoxCollider>());
    }
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(transform.position, player.position) < dist)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * coinSpeed);
        }
    }
}
