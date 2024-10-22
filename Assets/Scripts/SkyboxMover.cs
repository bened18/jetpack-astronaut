using UnityEngine;

public class SkyboxMover : MonoBehaviour
{
    public Material skyboxMaterial; // Material del Skybox (m20 en este caso)
    public float rotationSpeed = 3.0f; // Velocidad de rotación del Skybox

    void Update()
    {
        if (skyboxMaterial != null)
        {
            // Calcular la nueva rotación en función de la velocidad y el tiempo
            float newRotation = skyboxMaterial.GetFloat("_Rotation") + (rotationSpeed * Time.deltaTime);
            skyboxMaterial.SetFloat("_Rotation", newRotation);
        }
    }
}
