using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienGenerator : MonoBehaviour
{
    public GameObject player;
    public GameObject alienPrefab;

    public float minTimeBetweenAliens = 15f; // Tiempo mínimo entre apariciones
    public float maxTimeBetweenAliens = 20f; // Tiempo máximo entre apariciones
    public float groundYPosition = 0f; // La altura del suelo donde aparecerá el Alien
    public float alienSpawnDistanceX = 15f;  // Distancia fija en X del jugador donde aparece el Alien

    private GameObject currentAlien; // Referencia al alien actual

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

            // Generar un Alien
            GenerateAlien();
        }
    }

    void GenerateAlien()
    {
        // Determinar la posición en el suelo
        Vector3 alienPosition = new Vector3(player.transform.position.x + alienSpawnDistanceX, groundYPosition, 0);

        // Generar el Alien en la posición determinada con una rotación inicial en Y de -160 grados
        currentAlien = Instantiate(alienPrefab, alienPosition, Quaternion.Euler(0, -160, 0));
    }

    void Update()
    {
        // Mantener la rotación del alien en el eje Y en -160 grados
        if (currentAlien != null)
        {
            currentAlien.transform.rotation = Quaternion.Euler(0, -160, 0);
        }
    }
}
