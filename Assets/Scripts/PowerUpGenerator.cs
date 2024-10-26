using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpGenerator : MonoBehaviour
{
    public GameObject player;
    public GameObject powerUpPrefab;
    public GameObject shieldPrefab; // Prefab del escudo

    public float minTimeBetweenPowerUps = 60f;
    public float maxTimeBetweenPowerUps = 120f;
    public float powerUpDuration = 10f;
    public float powerUpSpawnDistanceX = 20f;
    public float minYPosition;
    public float maxYPosition;

    private bool isPlayerInvincible = false;
    private GameObject activeShield;

    void Start()
    {
        StartCoroutine(PowerUpGenerationLoop());
    }

    IEnumerator PowerUpGenerationLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTimeBetweenPowerUps, maxTimeBetweenPowerUps));
            GeneratePowerUp();
        }
    }

    void GeneratePowerUp()
    {
        float spawnY = Random.Range(minYPosition, maxYPosition);
        Vector3 powerUpPosition = new Vector3(player.transform.position.x + powerUpSpawnDistanceX, spawnY, 0);
        Instantiate(powerUpPrefab, powerUpPosition, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colisión detectada con: " + other.gameObject.name); // Para ver qué está colisionando
        if (other.gameObject.CompareTag("PowerUp"))
        {
            Debug.Log("PowerUp detectado"); // Confirma si el PowerUp está siendo detectado
            Destroy(other.gameObject);
            StartCoroutine(ActivateInvincibility());
        }
    }

    public IEnumerator ActivateInvincibility()
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
            shieldMaterial.SetFloat("_InnerAlpha", -0.5f); // Reducir el alpha
            yield return new WaitForSeconds(0.5f);
            shieldMaterial.SetFloat("_InnerAlpha", 0.1f); // Restaurar el alpha
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1f);

        // Desactivar la invencibilidad y el escudo
        isPlayerInvincible = false;
        Debug.Log("Jugador ya no es invencible");
        if (activeShield != null)
        {
            Destroy(activeShield);
        }
    }


    public bool IsPlayerInvincible()
    {
        return isPlayerInvincible;
    }
}
