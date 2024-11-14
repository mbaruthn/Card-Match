using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public GameObject cardPrefab; // Kart prefab referans�
    public Transform cardContainer; // Kartlar�n yerle�tirilece�i ana obje (container)

    private List<Card> cards = new List<Card>(); // Kartlar� depolayaca��m�z liste
    private ScoreManager scoreManager;

    public GridLayout3D gridLayout; // Grid d�zenleyici referans�
    private LevelManager levelManager; // LevelManager referans�

    private void Start()
    {
        // ScoreManager ve LevelManager'i bulup referans al
        scoreManager = FindObjectOfType<ScoreManager>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    // Kartlar� sahneye yerle�tirme
    public void InitializeCards(int rows, int columns)
    {
        int totalCards = rows * columns;

        ClearCards();

        List<int> cardIDs = new List<int>();
        for (int i = 0; i < totalCards / 2; i++)
        {
            cardIDs.Add(i);
            cardIDs.Add(i);
        }
        cardIDs = ShuffleList(cardIDs); // Kar��t�r�lm�� kart listesi

        for (int i = 0; i < totalCards; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardContainer);
            Card card = cardObj.GetComponent<Card>();
            card.SetCardID(cardIDs[i]);
            card.OnCardSelected = OnCardSelected; // Kart�n se�ilme olay�n� ba�la
            cards.Add(card);
        }

        gridLayout.ArrangeCards(cards.ConvertAll(c => c.gameObject), rows, columns);
    }

    public void ClearCards()
    {
        // Kart listesindeki her kart� sahneden kald�r ve bellekten temizle
        foreach (Card card in cards)
        {
            Destroy(card.gameObject);
        }

        // Kart listesi temizleniyor
        cards.Clear();
    }

    private List<int> ShuffleList(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(0, list.Count);
            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }

    // Se�ilen kartlar� y�netme
    private void OnCardSelected(Card selectedCard)
    {
        if (selectedCard.isFlipped || selectedCard.IsMatched)
            return; // Zaten a��k veya e�le�mi� kartlar� i�lememize gerek yok

        selectedCard.FlipCard();

        // Ayn� ID'ye sahip a��k di�er kartlar� kontrol et
        foreach (Card card in cards)
        {
            if (card != selectedCard && card.isFlipped && card.CardID == selectedCard.CardID)
            {
                // E�le�me sa�land�
                selectedCard.SetMatched();
                card.SetMatched();
                scoreManager.IncreaseScore(10);

                // T�m kartlar e�le�ti mi kontrol et
                if (cards.TrueForAll(c => c.IsMatched))
                {
                    scoreManager.ShowFinalScore(); // Son skoru g�ster
                    levelManager.NextLevel(); // Sonraki seviyeye ge�
                }

                return; // E�le�meyi bulduk ve i�ledik
            }
        }
    }
}
