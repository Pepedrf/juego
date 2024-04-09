using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    
    private Rigidbody rb;

    public float speed = 5f;


    private void Awake() {
        rb= GetComponent<Rigidbody>();
    }

    private void Update() {
        float moveHorizontal = Input.GetAxis("Horizontal");

        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal*speed, rb.velocity.y, moveVertical*speed);

        rb.velocity = movement;
    }

}
