using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugChangeCard : MonoBehaviour
{
    private CardFlipper cardFlipper;
    private CardModel cardModel;
    private int cardIndex = 0;

    [SerializeField]
    private GameObject card;

    // Start is called before the first frame update
    void Awake()
    {
        cardModel = card.GetComponent<CardModel>();
        cardFlipper = card.GetComponent<CardFlipper>();
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 28), "HIT ME!"))
        {
            cardModel.cardIndex = cardIndex;


            if (cardIndex > cardModel.cardIndexLast)
            {
                cardIndex = 0;
                cardFlipper.FlipCard(cardModel.GetFace(cardModel.cardIndexLast), cardModel.GetCardBack(), -1);
            }
            else
            {
                if(cardIndex > 0)
                {
                    cardFlipper.FlipCard(cardModel.GetFace(cardIndex - 1), cardModel.GetFace(cardIndex), cardIndex);
                }
                else
                {
                    cardFlipper.FlipCard(cardModel.GetCardBack(), cardModel.GetFace(cardIndex), cardIndex);
                }

                cardIndex++;
            }
        }
    }
}
