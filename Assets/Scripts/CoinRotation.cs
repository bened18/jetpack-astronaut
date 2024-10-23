using UnityEngine;

public class CoinRotation : MonoBehaviour
{
    public float rotationSpeed = 100f; // Velocidad de rotación en grados por segundo

    void Update()
    {
        // Rotar la moneda alrededor de su propio eje Y
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
    }
}

