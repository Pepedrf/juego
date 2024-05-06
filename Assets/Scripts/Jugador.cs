using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 5f;
    public float rotationSpeed = 5f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal ,0, moveVertical);
        movement.Normalize();

        transform.position = transform.position + movement * speed * Time.deltaTime;
        if(movement!=Vector3.zero)transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(movement),rotationSpeed * Time.deltaTime);

    }

}
