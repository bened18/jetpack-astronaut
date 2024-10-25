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
    public TextMeshProUGUI highScoreText; // Texto para mostrar el récord

    // Referencias a los scripts de contadores
    public CoinCounter coinCounter; // Referencia al contador de monedas
    public DistanceCounter distanceCounter; // Referencia al contador de distancia
    public RecordManager recordManager; // Referencia al script RecordManager

    // Referencia al script del PowerUpGenerator o similar que maneja la invencibilidad
    public PowerUpGenerator powerUpGenerator;

    // Material del jugador (el cubo) para modificar su transparencia
    private Material playerMaterial;

    private Color originalColor;
    public Color invincibleColor = Color.green; // El color que quieres cuando sea invencible

    public AudioSource audioSource;  // Referencia al componente de AudioSource
    public AudioClip deathScream; // Sonido de alerta (beep)

    public ParticleSystem jetpackParticles; // Sistema de partículas del jetpack

    private Animator animator; // Referencia al Animator

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); // Obtener el Animator del jugador
        playAgainButton.SetActive(false);
        resultText.gameObject.SetActive(false);
        // Obtener el material del jugador y su color original
        playerMaterial = GetComponent<Renderer>().material;
        originalColor = playerMaterial.color;
        // Mostrar el récord guardado
        DisplayHighScore();
        // Desactivar las partículas del jetpack al inicio
        jetpackParticles.Stop();
    }

    private void FixedUpdate() {
        if (playerCanMove && Input.GetMouseButton(0)) 
        {
            rb.AddForce(new Vector3(0, 50, 0), ForceMode.Acceleration);
            // Activar las partículas del jetpack
            if (!jetpackParticles.isPlaying)
            {
                jetpackParticles.Play();
            }
        }
        else
        {
            // Detener las partículas del jetpack si no se está presionando el clic
            if (jetpackParticles.isPlaying)
            {
                jetpackParticles.Stop();
            }

            if (Input.GetMouseButtonUp(0))
            {
                rb.velocity *= 0.50f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Floor"))
        {
            // Si el jugador toca el suelo, IsFlying es false
            animator.SetBool("IsFlying", false);
        }
        if (collision.gameObject.CompareTag("PrevFloor"))
        {
            // Si el jugador toca el suelo, IsFlying es false
            animator.SetBool("IsFlying", false);
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.CompareTag("Floor"))
        {
            // Si el jugador deja de tocar el suelo, IsFlying es true
            animator.SetBool("IsFlying", true);
        }
        if (collision.gameObject.CompareTag("PrevFloor"))
        {
            // Si el jugador deja de tocar el suelo, IsFlying es true
            animator.SetBool("IsFlying", true);
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
            playAgainButton.SetActive(true);
            resultText.gameObject.SetActive(true);

            if (audioSource != null && deathScream != null)
            {
                audioSource.PlayOneShot(deathScream);
            }

            // Obtener la distancia recorrida desde el script DistanceCounter
            float distanceTravelled = distanceCounter.GetDistanceTravelled();
            int coinsCollected = coinCounter.coinCount;
            // Mostrar los resultados de monedas y distancia
            resultText.text = "you traveled: " + Mathf.Floor(distanceTravelled).ToString() + " meters\n" +
                              "you picked up: " + coinCounter.coinCount + " coins";

            // Guardar el récord a través del RecordManager (sumar las monedas y actualizar la distancia si es necesario)
            recordManager.SaveHighScore(distanceTravelled, coinsCollected);

            // Activar la animación de muerte
            animator.SetBool("IsDeath", true);

            // Congelar el tiempo después de 3 segundos
            StartCoroutine(FreezeGameAfterDelay(3f));
        }
    }

     IEnumerator FreezeGameAfterDelay(float delay)
    {
        // Esperar el tiempo especificado antes de congelar el juego
        yield return new WaitForSeconds(delay);
        Time.timeScale = 0f; // Congelar el tiempo
    }

    public void OnPlayAgainButtonPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reinicia la escena
    }

    private void DisplayHighScore()
    {
        highScoreText.text = recordManager.GetHighScoreText(); // Obtener el récord desde el RecordManager
    }
}
