using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerClickHandler
{
    public int CardID { get; private set; }
    private bool isFlipped = false;
    public bool IsMatched { get; private set; } = false;

    public System.Action<Card> OnCardSelected;

    public void SetCardID(int id)
    {
        CardID = id;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked!");
        if (!isFlipped && !IsMatched && OnCardSelected != null)
        {
            OnCardSelected.Invoke(this);
        }
    }

    public void FlipCard()
    {
        isFlipped = !isFlipped;
        // Kart� �evirme animasyonu veya g�rsel de�i�imi burada yap�lacak
        Debug.Log("Card flipped: " + CardID);
    }

    public void SetMatched()
    {
        IsMatched = true;
        // Kart�n e�le�me durumu animasyonu veya efekti burada yap�lacak
        Debug.Log("Card matched: " + CardID);
    }
}
