using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    public bool gameOver = false;
    public bool playerCanMove = true;
    public GameObject playAgainButton;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI highScoreText;
    public CoinCounter coinCounter;
    public DistanceCounter distanceCounter;
    public RecordManager recordManager;
    public PowerUpGenerator powerUpGenerator;
    public AudioSource audioSource;
    public AudioClip deathScream;
    public AudioClip runningSteps;
    public AudioClip jetpackSound;
    public ParticleSystem jetpackParticles;
    private Animator animator;
    
    private AudioSource stepsAudioSource;
    private AudioSource jetpackAudioSource;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playAgainButton.SetActive(false);
        resultText.gameObject.SetActive(false);
        DisplayHighScore();
        jetpackParticles.Stop();

        // Configurar un AudioSource separado para los pasos
        stepsAudioSource = gameObject.AddComponent<AudioSource>();
        stepsAudioSource.clip = runningSteps;
        stepsAudioSource.volume = 1.0f;
        stepsAudioSource.loop = true;

        // Configurar un AudioSource separado para el jetpack
        jetpackAudioSource = gameObject.AddComponent<AudioSource>();
        jetpackAudioSource.clip = jetpackSound;
        jetpackAudioSource.loop = true; // Activar loop para que continúe mientras se mantiene el botón
    }

    private void FixedUpdate() {
        if (playerCanMove && Input.GetMouseButton(0)) 
        {
            rb.AddForce(new Vector3(0, 50, 0), ForceMode.Acceleration);

            if (!jetpackParticles.isPlaying)
            {
                jetpackParticles.Play();
            }

            // Reproducir el sonido del jetpack si no está sonando
            if (!jetpackAudioSource.isPlaying)
            {
                jetpackAudioSource.Play();
            }

            // Detener el sonido de los pasos cuando el jugador está volando
            if (stepsAudioSource.isPlaying)
            {
                stepsAudioSource.Stop();
            }
        }
        else
        {
            // Detener las partículas del jetpack si no se está presionando el clic
            if (jetpackParticles.isPlaying)
            {
                jetpackParticles.Stop();
            }

            // Detener el sonido del jetpack cuando se suelta el clic
            if (jetpackAudioSource.isPlaying)
            {
                jetpackAudioSource.Stop();
            }

            if (Input.GetMouseButtonUp(0))
            {
                rb.velocity *= 0.50f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("PrevFloor"))
        {
            animator.SetBool("IsFlying", false);
            if (playerCanMove && !stepsAudioSource.isPlaying)
            {
                stepsAudioSource.Play(); // Iniciar el sonido de los pasos al tocar el suelo
            }
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("PrevFloor"))
        {
            animator.SetBool("IsFlying", true);
            stepsAudioSource.Stop(); // Detener el sonido de los pasos al dejar el suelo
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Colisión detectada con: " + other.gameObject.name);
        // Si el jugador ya es invencible, ignorar la colisión
        if (powerUpGenerator.IsPlayerInvincible())
        {
            return;
        }

        if (other.CompareTag("PowerUp"))
        {
            Debug.Log("PowerUp recogido");
            Destroy(other.gameObject); // Elimina el PowerUp de la escena
            StartCoroutine(powerUpGenerator.ActivateInvincibility()); // Activa la invencibilidad y el escudo desde PowerUpGenerator
        }

        // Colisión con el Obstacle
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

            float distanceTravelled = distanceCounter.GetDistanceTravelled();
            int coinsCollected = coinCounter.coinCount;

            resultText.text = "you traveled: " + Mathf.Floor(distanceTravelled).ToString() + " meters\n" +
                            "you picked up: " + coinCounter.coinCount + " coins";

            recordManager.SaveHighScore(distanceTravelled, coinsCollected);

            animator.SetBool("IsDeath", true);
            StartCoroutine(FreezeGameAfterDelay(3f));
        }
    }
    

    IEnumerator FreezeGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = 0f;
    }

    public void OnPlayAgainButtonPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void DisplayHighScore()
    {
        highScoreText.text = recordManager.GetHighScoreText();
    }
}
