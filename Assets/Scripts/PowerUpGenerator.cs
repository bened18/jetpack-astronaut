using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpGenerator : MonoBehaviour
{
    public GameObject player;
    public GameObject powerUpPrefab;
    public GameObject shieldPrefab; // Prefab del escudo

    public float minTimeBetweenPowerUps = 60f; // Tiempo mínimo entre Power-Ups
    public float maxTimeBetweenPowerUps = 120f; // Tiempo máximo entre Power-Ups
    public float powerUpDuration = 10f; // Duración del efecto del Power-Up
    public float powerUpSpawnDistanceX = 20f; // Distancia en X del jugador donde aparecerá el Power-Up
    public float minYPosition;
    public float maxYPosition;

    private bool isPlayerInvincible = false;
    private GameObject activeShield; // Referencia al escudo activo

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

        // Crear el escudo y posicionarlo alrededor del jugador
        activeShield = Instantiate(shieldPrefab, player.transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
        activeShield.transform.SetParent(player.transform); // Hacer que el escudo siga al jugador

        // Obtener el material del escudo
        Material shieldMaterial = activeShield.GetComponent<Renderer>().material;

        // Esperar 7 segundos antes de comenzar a parpadear (para dejar 3 segundos de parpadeo)
        yield return new WaitForSeconds(powerUpDuration - 3f);

        // Parpadeo del escudo
        for (int i = 0; i < 3; i++)
        {
            // Reducir el alpha
            shieldMaterial.SetFloat("_InnerAlpha", -0.5f);
            yield return new WaitForSeconds(0.5f);

            // Restaurar el alpha
            shieldMaterial.SetFloat("_InnerAlpha", 0.1f);
            yield return new WaitForSeconds(0.5f);
        }

        // Esperar el tiempo restante
        yield return new WaitForSeconds(1f);

        // Desactivar la invencibilidad
        isPlayerInvincible = false;
        Debug.Log("Jugador ya no es invencible");

        // Destruir el escudo
        if (activeShield != null)
        {
            Destroy(activeShield);
        }
    }

    // Método para verificar si el jugador es invencible
    public bool IsPlayerInvincible()
    {
        return isPlayerInvincible;
    }
}
