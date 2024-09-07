using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    Rigidbody rb;
    public bool gameOver = false;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if (gameOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene("Game");
            }
            return;
        }else if (Input.GetMouseButton(0)) 
        {
            rb.AddForce(new Vector3(0, 50, 0), ForceMode.Acceleration);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            rb.velocity *= 0.50f;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Coins"))
        {
            gameOver = true;
            rb.isKinematic = true;
        }
    }
}
