using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IncrementGameSpeed : MonoBehaviour
{
    public Camera cameraScript; // Referencia al script de la cámara
    public DistanceCounter distanceCounter; // Referencia al contador de distancia
    private float nextSpeedIncrease = 50f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Obtener la distancia recorrida del DistanceCounter
        float distanceTravelled = distanceCounter.GetDistanceTravelled();

        // Aumentar la velocidad de la cámara cada 50 metros
        if (distanceTravelled >= nextSpeedIncrease)
        {
            cameraScript.IncreaseSpeed(10f); // Aumenta la velocidad en un 10%
            nextSpeedIncrease += 50f; // Establecer la nueva distancia objetivo para el próximo aumento
        }
    }
}
