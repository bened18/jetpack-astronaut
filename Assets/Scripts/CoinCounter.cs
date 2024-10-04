using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Necesario para usar TextMeshPro

public class CoinCounter : MonoBehaviour
{
    public int coinCount = 0; // Contador de monedas
    public TextMeshProUGUI coinCounterText; // Texto en la UI para mostrar las monedas
    // Referencia al grupo padre (el grupo de monedas)
    private Transform coinGroupParent;

    public AudioSource coinSound; // Referencia al componente AudioSource


    private void Start()
    {
        // Inicializar el texto del contador de monedas
        UpdateCoinCounterUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto con el que colisionamos es una moneda
        if (other.CompareTag("Coins"))
        {
            coinCount++; // Aumentar el contador de monedas
            UpdateCoinCounterUI(); // Actualizar la UI

             // Reproducir el sonido de la moneda
            if (coinSound != null)
            {
                coinSound.Play();
            }

            // Desactivar la moneda
            other.gameObject.SetActive(false);
            // Obtener referencia al padre (grupo de monedas)
            coinGroupParent = other.transform.parent;

            // Comprobar si todas las monedas del grupo padre han sido recogidas
            CheckAndDisableParent(coinGroupParent);
        }
    }

    // Función para actualizar el texto del contador en la UI
    private void UpdateCoinCounterUI()
    {
        coinCounterText.text = coinCount + " $";
    }
    // Función para desactivar el padre si todas las monedas hijas han sido recogidas
    private void CheckAndDisableParent(Transform parent)
    {
        bool allCoinsCollected = true;

        // Verificar si todas las monedas hijas están desactivadas
        foreach (Transform coin in parent)
        {
            if (coin.gameObject.activeSelf)
            {
                allCoinsCollected = false;
                break;
            }
        }

        // Si todas las monedas hijas están desactivadas, desactivar el padre
        if (allCoinsCollected)
        {
            parent.gameObject.SetActive(false);
        }
    }
}

