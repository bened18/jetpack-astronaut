using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpGenerator : MonoBehaviour
{
    public GameObject player;
    public GameObject powerUpPrefab;

    public float minTimeBetweenPowerUps = 60f; // Tiempo mínimo entre Power-Ups
    public float maxTimeBetweenPowerUps = 120f; // Tiempo máximo entre Power-Ups
    public float powerUpDuration = 10f; // Duración del efecto del Power-Up
    public float powerUpSpawnDistanceX = 20f; // Distancia en X del jugador donde aparecerá el Power-Up
    public float minYPosition;
    public float maxYPosition;

    private bool isPlayerInvincible = false;

    void Start()
    {
        // Iniciar la corrutina para generar Power-Ups en intervalos aleatorios
        StartCoroutine(PowerUpGenerationLoop());
    }

    IEnumerator PowerUpGenerationLoop()
    {
        while (true)
        {
            // Esperar un tiempo aleatorio entre 60 y 120 segundos antes de generar el siguiente Power-Up
            yield return new WaitForSeconds(Random.Range(minTimeBetweenPowerUps, maxTimeBetweenPowerUps));

            // Generar el Power-Up
            StartCoroutine(GeneratePowerUp());
        }
    }

    IEnumerator GeneratePowerUp()
    {
        // Determinar la posición en la pantalla
        float spawnY = Random.Range(minYPosition, maxYPosition);
        Vector3 powerUpPosition = new Vector3(player.transform.position.x + powerUpSpawnDistanceX, spawnY, 0);

        // Generar el Power-Up en la posición determinada
        GameObject powerUp = Instantiate(powerUpPrefab, powerUpPosition, Quaternion.identity);

        // Esperar a que el jugador colisione con el Power-Up
        while (powerUp != null && Vector3.Distance(player.transform.position, powerUp.transform.position) > 0.5f)
        {
            yield return null;
        }

        // El jugador ha recogido el Power-Up
        if (powerUp != null)
        {
            Destroy(powerUp);
            StartCoroutine(ActivateInvincibility());
        }
    }

    IEnumerator ActivateInvincibility()
    {
        // Activar la invencibilidad
        isPlayerInvincible = true;
        Debug.Log("Jugador invencible");

        // Esperar 10 segundos de invencibilidad
        yield return new WaitForSeconds(powerUpDuration);

        // Desactivar la invencibilidad
        isPlayerInvincible = false;
        Debug.Log("Jugador ya no es invencible");
    }

    // Este método simula la verificación de invencibilidad, puedes agregarlo a tu script de colisiones o de vida del jugador
    public bool IsPlayerInvincible()
    {
        return isPlayerInvincible;
    }
}
