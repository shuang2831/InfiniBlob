using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public GameObject player;

    private Vector3 offset;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0, -9.81F, 0);
        Screen.SetResolution(1920, 1080, true);
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        //transform.position = player.transform.position + offset;
    }
    void FixedUpdate()
    {
        rb.MovePosition(player.transform.position + offset);
    }
}