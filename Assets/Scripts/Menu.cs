using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Necesario para usar TextMeshPro

public class Menu : MonoBehaviour
{
    public TextMeshProUGUI highScoreText; // Texto para mostrar el récord y las monedas

    private RecordManager recordManager; // Referencia al RecordManager

    private void Start()
    {
        // Crear una referencia al RecordManager
        recordManager = FindObjectOfType<RecordManager>();

        // Si existe un RecordManager, mostrar el récord y las monedas
        if (recordManager != null)
        {
            highScoreText.text = recordManager.GetHighScoreText();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1); // Cargar la escena del juego
    }

    public void QuitGame()
    {
        Application.Quit(); // Salir del juego
    }
}
