using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsGenerator : MonoBehaviour
{
    public GameObject player;
    public GameObject coinGroup1;
    public GameObject coinGroup2;
    public GameObject coinGroup3;
    public GameObject coinGroup4;
    public GameObject lineCoinsPrefab;
    public GameObject stairsCoinsPrefab;

    public float minCoinY;
    public float maxCoinY;

    public float minCoinSpacing;
    public float maxCoinSpacing;

    void Start()
    {
        // Inicializamos generando los primeros grupos de monedas
        coinGroup1 = GenerateCoinGroup(player.transform.position.x + 20f);
        coinGroup2 = GenerateCoinGroup(coinGroup1.transform.position.x);
        coinGroup3 = GenerateCoinGroup(coinGroup2.transform.position.x);
        coinGroup4 = GenerateCoinGroup(coinGroup3.transform.position.x);
    }

    // Función para generar aleatoriamente un grupo de monedas
    GameObject GenerateCoinGroup(float referenceX)
    {
        // Elegimos aleatoriamente entre los dos prefabs
        GameObject coinPrefab = (Random.Range(0, 2) == 0) ? lineCoinsPrefab : stairsCoinsPrefab;
        
        // Instanciamos el prefab elegido
        GameObject coinGroup = GameObject.Instantiate(coinPrefab);
        
        // Establecemos la posición del grupo de monedas
        SetTransform(coinGroup, referenceX);
        
        return coinGroup;
    }

    // Función para establecer la posición y espaciado del grupo de monedas
    void SetTransform(GameObject coinGroup, float referenceX)
    {
        // Reactivar el grupo padre si está desactivado
        if (!coinGroup.activeSelf)
        {
            coinGroup.SetActive(true);
        }

        // Reactivar todas las monedas hijas
        foreach (Transform coin in coinGroup.transform)
        {
            coin.gameObject.SetActive(true);
        }

        coinGroup.transform.position = new Vector3(referenceX + Random.Range(minCoinSpacing, maxCoinSpacing), Random.Range(minCoinY, maxCoinY), 0);
    }

    void Update()
    {
        // Verificamos si el jugador ha pasado el segundo grupo de monedas
        if (player.transform.position.x > coinGroup2.transform.position.x)
        {
            // Reorganizamos los grupos de monedas para mantener el ciclo
            var tempCoinGroup = coinGroup1;
            coinGroup1 = coinGroup2;
            coinGroup2 = coinGroup3;
            coinGroup3 = coinGroup4;

            // Generamos nuevas monedas basadas en la posición del último grupo
            SetTransform(tempCoinGroup, coinGroup3.transform.position.x);
            coinGroup4 = tempCoinGroup;
        }
    }
}
