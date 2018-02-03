using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowmanBehaviour : MonoBehaviour {
    private Transform Target;
    private int rotationSpeed = 5;
    // Use this for initialization
    void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update () {
        lookAt();
	}

    void lookAt()
    {
        // Rotate to look at player.
        Quaternion rotation = Quaternion.LookRotation(Target.position - transform.position);
        rotation *= Quaternion.Euler(0, 90, 0);
        rotation.z = 0;

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        //transform.LookAt(Target); alternate way to track player replaces both lines above.
    }
}
