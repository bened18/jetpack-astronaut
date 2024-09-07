using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Necesario para usar TextMeshPro
using UnityEngine.UI; // Necesario para usar botones

public class Player : MonoBehaviour
{

    Rigidbody rb;
    public bool gameOver = false;
    public bool playerCanMove = true;

    public GameObject playAgainButton;
    public TextMeshProUGUI resultText; 

    // Referencias a los scripts de contadores
    public CoinCounter coinCounter; // Referencia al contador de monedas
    public DistanceCounter distanceCounter; // Referencia al contador de distancia

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        playAgainButton.SetActive(false);
        resultText.gameObject.SetActive(false);
    }

    private void FixedUpdate() {
        if (playerCanMove && Input.GetMouseButton(0)) 
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
            playerCanMove = false;
            rb.AddForce(new Vector3(500, 0, 0), ForceMode.Acceleration);
            playAgainButton.SetActive(true);
            resultText.gameObject.SetActive(true);

             // Obtener la distancia recorrida desde el script DistanceCounter
            float distanceTravelled = distanceCounter.GetDistanceTravelled();
            // Mostrar los resultados de monedas y distancia
             // Activar el texto
            resultText.text = "Recorriste: " + Mathf.Floor(distanceTravelled).ToString() + " metros\n" +
                              "Recogiste: " + coinCounter.coinCount + " monedas";
        }
    }

    public void OnPlayAgainButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reinicia la escena
    }
}
