using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordManager : MonoBehaviour
{
    // Guardar el récord de distancia si es mayor y sumar las monedas
    public void SaveHighScore(float distance, int coins)
    {
        float highScore = PlayerPrefs.GetFloat("HighScore", 0);
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);

        // Siempre sumar las monedas al total
        totalCoins += coins;
        PlayerPrefs.SetInt("TotalCoins", totalCoins);

        // Si la distancia recorrida es mayor que el récord anterior
        if (distance > highScore)
        {
            PlayerPrefs.SetFloat("HighScore", distance);
        }

        PlayerPrefs.Save();
    }

    // Obtener el récord y la cantidad total de monedas
    public string GetHighScoreText()
    {
        float highScore = PlayerPrefs.GetFloat("HighScore", 0);
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);

        return "Record: " + Mathf.Floor(highScore).ToString() + " meters\n" +
               "Total Coins: " + totalCoins.ToString();
    }

    // Obtener la cantidad total de monedas
    public int GetTotalCoins()
    {
        return PlayerPrefs.GetInt("TotalCoins", 0);
    }
}
