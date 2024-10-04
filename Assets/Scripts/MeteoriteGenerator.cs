using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteGenerator : MonoBehaviour
{
    public GameObject player;
    public GameObject meteoritePrefab;
    public GameObject alertPrefab;

    public float meteoriteSpeed = 10f;
    public float timeBeforeLaunch = 2f; // Tiempo entre la alerta y el lanzamiento

    public float minYPosition;
    public float maxYPosition;

    public float alertDistanceX = 15f;  // Distancia fija en X del jugador donde aparece la alerta

    public float minTimeBetweenMeteorites = 15f; // Tiempo mínimo entre meteoritos
    public float maxTimeBetweenMeteorites = 30f; // Tiempo máximo entre meteoritos

    void Start()
    {
        // Iniciar la corrutina para generar meteoritos en intervalos aleatorios
        StartCoroutine(MeteoriteGenerationLoop());
    }

    IEnumerator MeteoriteGenerationLoop()
    {
        while (true) // Bucle infinito para generar meteoritos indefinidamente
        {
            yield return new WaitForSeconds(Random.Range(minTimeBetweenMeteorites, maxTimeBetweenMeteorites)); // Esperar un tiempo aleatorio

            // Llamar a la corrutina que genera un meteorito
            StartCoroutine(GenerateMeteorite());
        }
    }

    IEnumerator GenerateMeteorite()
    {
        // Determinar la posición inicial de la alerta en el eje Y
        float spawnY = Random.Range(minYPosition, maxYPosition);
        Vector3 alertPosition = new Vector3(player.transform.position.x + alertDistanceX, spawnY, 0);

        // Generar la alerta en la posición determinada
        GameObject alert = Instantiate(alertPrefab, alertPosition, Quaternion.identity);

        // Mover la alerta junto con el jugador mientras se espera antes del lanzamiento
        float elapsedTime = 0f;
        while (elapsedTime < timeBeforeLaunch)
        {
            // Actualizar la posición de la alerta para seguir al jugador en el eje X
            alert.transform.position = new Vector3(player.transform.position.x + alertDistanceX, spawnY, 0);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Guardar la posición final de la alerta para disparar el meteorito desde ahí
        Vector3 finalAlertPosition = alert.transform.position;

        // Destruir la alerta (ya no es necesaria)
        Destroy(alert);

        // Generar el meteorito en la posición final de la alerta
        GameObject meteorite = Instantiate(meteoritePrefab, finalAlertPosition, Quaternion.identity);

        // Lanzar el meteorito hacia la posición actual del jugador
        Vector3 targetPosition = player.transform.position;
        while (meteorite != null && Vector3.Distance(meteorite.transform.position, targetPosition) > 0.1f)
        {
            meteorite.transform.position = Vector3.MoveTowards(meteorite.transform.position, targetPosition, meteoriteSpeed * Time.deltaTime);
            yield return null;
        }

        // Destruir el meteorito si ha alcanzado al jugador o ha pasado de largo
        if (meteorite != null)
        {
            Destroy(meteorite);
        }
    }
}
