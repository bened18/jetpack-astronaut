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

    // Referencia al script del PowerUpGenerator o similar que maneja la invencibilidad
    public PowerUpGenerator powerUpGenerator;

    // Material del jugador (el cubo) para modificar su transparencia
    private Material playerMaterial;

    private Color originalColor;
    public Color invincibleColor = Color.green; // El color que quieres cuando sea invencible

    public AudioSource audioSource;  // Referencia al componente de AudioSource
    public AudioClip deathScream; // Sonido de alerta (beep)

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        playAgainButton.SetActive(false);
        resultText.gameObject.SetActive(false);
        // Obtener el material del jugador y su color original
        playerMaterial = GetComponent<Renderer>().material;
        originalColor = playerMaterial.color;
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

        // Verificar si el jugador es invencible y ajustar el color
        if (powerUpGenerator.IsPlayerInvincible())
        {
            SetPlayerColor(invincibleColor); // Cambiar al color invencible
            Debug.Log("Color de Jugador verde");

        }
        else
        {
            SetPlayerColor(originalColor); // Restaurar el color original
        }
    }

    private void OnTriggerEnter(Collider other) {

         // Si el jugador es invencible, ignorar la colisión
        if (powerUpGenerator.IsPlayerInvincible())
        {
            // El jugador es invencible, no hacer nada.
            return;
        }

        if (other.CompareTag("Obstacle"))
        {
            gameOver = true;
            playerCanMove = false;
            rb.AddForce(new Vector3(500, 0, 0), ForceMode.Acceleration);
            playAgainButton.SetActive(true);
            resultText.gameObject.SetActive(true);

            if (audioSource != null && deathScream != null)
            {
                audioSource.PlayOneShot(deathScream);
            }

             // Obtener la distancia recorrida desde el script DistanceCounter
            float distanceTravelled = distanceCounter.GetDistanceTravelled();
            // Mostrar los resultados de monedas y distancia
             // Activar el texto
            resultText.text = "Recorriste: " + Mathf.Floor(distanceTravelled).ToString() + " metros\n" +
                              "Recogiste: " + coinCounter.coinCount + " monedas";
        }
    }

    // Método para ajustar el color del jugador
    private void SetPlayerColor(Color color)
    {
        playerMaterial.color = color; // Cambiar el color del material
    }

    public void OnPlayAgainButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reinicia la escena
    }
}
