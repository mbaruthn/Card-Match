using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public GameObject cardPrefab; // Kart prefab referans�
    public Transform cardContainer; // Kartlar�n yerle�tirilece�i ana obje (container)

    private List<Card> cards = new List<Card>(); // Kartlar� depolayaca��m�z liste
    private Card firstSelectedCard; // �lk se�ilen kart
    private Card secondSelectedCard; // �kinci se�ilen kart
    private ScoreManager scoreManager;

    public GridLayout3D gridLayout; // Grid d�zenleyici referans�

    private void Start()
    {
        // ScoreManager'i bulup referans al
        scoreManager = FindObjectOfType<ScoreManager>();
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

    private void OnCardSelected(Card selectedCard)
    {
        if (firstSelectedCard == null)
        {
            firstSelectedCard = selectedCard;
            firstSelectedCard.FlipCard();
        }
        else if (secondSelectedCard == null && firstSelectedCard != selectedCard)
        {
            secondSelectedCard = selectedCard;
            secondSelectedCard.FlipCard();
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(1f);

        if (firstSelectedCard.CardID == secondSelectedCard.CardID)
        {
            firstSelectedCard.SetMatched();
            secondSelectedCard.SetMatched();
            scoreManager.IncreaseScore(10);
        }
        else
        {
            firstSelectedCard.FlipCard();
            secondSelectedCard.FlipCard();
            scoreManager.DecreaseScore(5);
        }

        firstSelectedCard = null;
        secondSelectedCard = null;

        // T�m kartlar e�le�tiyse bir sonraki seviyeye ge�
        if (cards.TrueForAll(card => card.IsMatched))
        {
            scoreManager.ShowFinalScore();
            FindObjectOfType<LevelManager>().NextLevel();
        }
    }
}
