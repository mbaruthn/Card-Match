using System.Collections.Generic;
using UnityEngine;

public class GridLayout3D : MonoBehaviour
{
    public float spacingY = 1.5f; // Kartlar aras�ndaki dikey bo�luk
    public float spacingX = 1.5f; // Kartlar aras�ndaki yatay bo�luk

    // Kartlar� bir �zgaraya dizmek i�in metot
    public void ArrangeCards(List<GameObject> cards, int rows, int columns)
    {
        // Dizinin toplam geni�li�i ve y�ksekli�i hesaplan�r
        float gridWidth = (columns - 1) * spacingX;
        float gridHeight = (rows - 1) * spacingY;

        // Kart dizini
        int index = 0;

        // Kartlar� sat�r ve s�tunlara g�re konumland�r
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (index >= cards.Count)
                    return;

                // Kart�n pozisyonunu hesapla, ortalanm�� konumda olacak �ekilde
                Vector3 position = new Vector3(
                    col * spacingX - gridWidth / 2,  // X ekseni boyunca ortalanm�� konum
                    -(row * spacingY - gridHeight / 2),  // Y ekseni boyunca ortalanm�� konum
                    0);  // Z ekseni sabit

                // Kart� pozisyonla
                cards[index].transform.position = position;

                // Bir sonraki karta ge�
                index++;
            }
        }
    }
}
