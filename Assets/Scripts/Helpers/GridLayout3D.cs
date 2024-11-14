using System.Collections.Generic;
using UnityEngine;

public class GridLayout3D : MonoBehaviour
{
    public float spacingY = 1.5f; // Kartlar aras�ndaki dikey bo�luk
    public float spacingX = 1.5f; // Kartlar aras�ndaki yatay bo�luk
    public float layoutPercentage = 0.8f; // Ekran�n % ka��n�n kullan�lmas� gerekti�i (�rne�in %80 i�in 0.8)

    // Kartlar� bir �zgaraya dizmek i�in metot
    public void ArrangeCards(List<GameObject> cards, int rows, int columns)
    {
        // Kameran�n ekran boyutlar�n� al�yoruz
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found!");
            return;
        }

        // Ekran�n ortografik boyutlar�n� hesapla
        float screenHeight = 2f * mainCamera.orthographicSize * layoutPercentage; // Y ekseni boyunca ekran y�ksekli�inin %80'i
        float screenWidth = screenHeight * mainCamera.aspect; // X ekseni boyunca ekran geni�li�i, en-boy oran�na ba�l�

        // Dizinin toplam geni�li�i ve y�ksekli�i hesaplan�r
        float gridWidth = (columns - 1) * spacingX;
        float gridHeight = (rows - 1) * spacingY;

        // E�er grid y�ksekli�i ekran�n %80'ini a��yorsa, columns de�erini art�rarak daha fazla s�tun ekleyin
        while (gridHeight > screenHeight)
        {
            columns++;
            gridWidth = (columns - 1) * spacingX; // Geni�li�i yeni columns de�erine g�re g�ncelle
            gridHeight = Mathf.Ceil((float)cards.Count / columns) * spacingY; // Y�ksekli�i yeniden hesapla
        }

        // Ba�lang�� pozisyonunu belirle
        Vector3 startPosition = new Vector3(
            -screenWidth / 2 + (screenWidth - gridWidth) / 2, // X ekseninde %80'lik alandan ortalanm�� ba�lang�� noktas�
            screenHeight / 2 - (screenHeight - gridHeight) / 2, // Y ekseninde %80'lik alandan ortalanm�� ba�lang�� noktas�
            0
        );

        // Kart dizini
        int index = 0;

        // Kartlar� sat�r ve s�tunlara g�re konumland�r
        for (int row = 0; row < Mathf.CeilToInt((float)cards.Count / columns); row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (index >= cards.Count)
                    return;

                // Kart�n pozisyonunu hesapla, X ve Y eksenine g�re ortalanm�� olacak �ekilde
                Vector3 position = startPosition + new Vector3(
                    col * spacingX,
                    -row * spacingY,
                    0);

                // Kart� pozisyonla
                cards[index].transform.position = position;

                // Bir sonraki karta ge�
                index++;
            }
        }
    }
}
