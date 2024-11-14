using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText; // Skorun g�r�nece�i UI Text
    private int score = 0;

    // Skoru ba�lat
    private void Start()
    {
        UpdateScoreUI();
    }

    // Skoru artt�r
    public void IncreaseScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    // Skoru azalt
    public void DecreaseScore(int amount)
    {
        score -= amount;
        if (score < 0) score = 0; // Negatif skor olmas�n
        UpdateScoreUI();
    }

    // Skor UI'�n� g�ncelle
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    // Oyunun tamamlanma durumu
    public void ShowFinalScore()
    {
        // Oyunun tamamland���n� g�steren bir UI veya animasyon tetikleyebiliriz
        Debug.Log("Game Finished! Final Score: " + score);
    }
}
