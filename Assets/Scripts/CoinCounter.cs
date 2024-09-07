using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Necesario para usar TextMeshPro

public class CoinCounter : MonoBehaviour
{
    public int coinCount = 0; // Contador de monedas
    public TextMeshProUGUI coinCounterText; // Texto en la UI para mostrar las monedas

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

            // Destruir la moneda para que desaparezca
            Destroy(other.gameObject);
        }
    }

    // Función para actualizar el texto del contador en la UI
    private void UpdateCoinCounterUI()
    {
        coinCounterText.text = coinCount + " $";
    }
}

