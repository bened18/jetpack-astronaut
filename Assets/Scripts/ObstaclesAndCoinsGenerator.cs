using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesAndCoinsGenerator : MonoBehaviour
{
    public GameObject player;
    
    // Prefabs para obstáculos (láseres) y monedas
    public GameObject laserPrefab;
    public GameObject lineCoinsPrefab;
    public GameObject stairsCoinsPrefab;

    // Configuración para generación
    public float minYPosition;
    public float maxYPosition;
    public float minSpacingBetweenGenerations;
    public float maxSpacingBetweenGenerations;

    // Probabilidades de generación
    [Range(0, 100)]
    public int obstacleProbability = 70; // Probabilidad de generar un obstáculo (en porcentaje)
    [Range(0, 100)]
    public int coinProbability = 30; // Probabilidad de generar monedas (en porcentaje)

    // Distancia inicial para generar el primer objeto
    private float nextGenerationX;

    void Start()
    {
        // Inicializar la posición de generación en función de la posición del jugador
        nextGenerationX = player.transform.position.x + 50f;
        
        // Generar el primer elemento
        GenerateNextElement();
    }

    void GenerateNextElement()
    {
        // Determinar si se generará un obstáculo o un grupo de monedas
        int randomChance = Random.Range(0, 100);
        if (randomChance < obstacleProbability)
        {
            GenerateLaser(nextGenerationX);
        }
        else
        {
            GenerateCoinGroup(nextGenerationX);
        }

        // Calcular la siguiente posición de generación
        float spacing = Random.Range(minSpacingBetweenGenerations, maxSpacingBetweenGenerations);
        nextGenerationX += spacing;
    }

    void GenerateLaser(float referenceX)
    {
        float posY = Random.Range(minYPosition, maxYPosition);
        GameObject laser = Instantiate(laserPrefab, new Vector3(referenceX, posY, 0), Quaternion.identity);
        // Aquí puedes añadir más lógica de configuración del láser si lo deseas
    }

    void GenerateCoinGroup(float referenceX)
    {
        float posY = Random.Range(minYPosition, maxYPosition);
        GameObject coinPrefab = (Random.Range(0, 2) == 0) ? lineCoinsPrefab : stairsCoinsPrefab;
        Instantiate(coinPrefab, new Vector3(referenceX, posY, 0), Quaternion.identity);
    }

    void Update()
    {
        // Verificar si el jugador ha pasado el punto de generación para generar el siguiente elemento
        if (player.transform.position.x > nextGenerationX - 20f)
        {
            GenerateNextElement();
        }
    }
}
