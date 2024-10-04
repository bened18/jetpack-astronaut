using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienGenerator : MonoBehaviour
{
    public GameObject player;
    public GameObject alienPrefab;

    public float alienSpeed = 3f;
    public float minTimeBetweenAliens = 15f; // Tiempo mínimo entre apariciones
    public float maxTimeBetweenAliens = 20f; // Tiempo máximo entre apariciones
    public float groundYPosition = 0f; // La altura del suelo donde aparecerá el Alien
    public float alienSpawnDistanceX = 15f;  // Distancia fija en X del jugador donde aparece el Alien

    void Start()
    {
        // Iniciar la corrutina para generar Aliens en intervalos aleatorios
        StartCoroutine(AlienGenerationLoop());
    }

    IEnumerator AlienGenerationLoop()
    {
        while (true) // Bucle infinito para generar Aliens indefinidamente
        {
            yield return new WaitForSeconds(Random.Range(minTimeBetweenAliens, maxTimeBetweenAliens)); // Esperar un tiempo aleatorio

            // Llamar a la corrutina que genera un Alien
            StartCoroutine(GenerateAlien());
        }
    }

    IEnumerator GenerateAlien()
    {
        // Determinar la posición en el suelo
        Vector3 alienPosition = new Vector3(player.transform.position.x + alienSpawnDistanceX, groundYPosition, 0);

        // Generar el Alien en la posición determinada
        GameObject alien = Instantiate(alienPrefab, alienPosition, Quaternion.identity);

        // Lógica adicional para el comportamiento del Alien
        while (alien != null && alien.transform.position.x > player.transform.position.x - 10f)
        {
            // Aquí puedes agregar cualquier lógica de movimiento si el Alien se mueve hacia el jugador
            alien.transform.position += Vector3.left * alienSpeed * Time.deltaTime;

            yield return null;
        }

        // Destruir el Alien cuando sale de la pantalla o si ya no es necesario
        if (alien != null)
        {
            Destroy(alien);
        }
    }
}
