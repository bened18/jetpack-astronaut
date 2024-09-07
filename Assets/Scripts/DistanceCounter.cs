using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Necesario para usar TextMeshPro

public class DistanceCounter : MonoBehaviour
{
    public Transform player;  // El transform del personaje
    public TextMeshProUGUI distanceText; // Componente de TextMeshPro para mostrar el texto
    private Vector3 startPosition;

    private float distance;

    private void Start()
    {
        // Guardar la posición inicial del personaje para calcular la distancia recorrida
        startPosition = player.position;
    }

    private void Update()
    {
        // Calcular la distancia recorrida en metros
        distance = Vector3.Distance(startPosition, player.position);
       
        // Mostrar la distancia en el texto, redondeada a un número entero
        distanceText.text = Mathf.Floor(distance).ToString() + " m";
    }

    public float GetDistanceTravelled()
    {
        return distance;
    }
}


