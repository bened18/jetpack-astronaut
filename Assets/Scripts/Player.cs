using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if (Input.GetMouseButton(0)) 
        {
            rb.AddForce(new Vector3(0, 50, 0), ForceMode.Acceleration);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            rb.velocity *= 0.50f;
        }
    }
}
