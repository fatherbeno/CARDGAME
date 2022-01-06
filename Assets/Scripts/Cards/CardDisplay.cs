using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    private CardCore cardCore;

    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text descriptionText;
    [SerializeField]
    private Image artImage;
    [SerializeField]
    private Image cardBackImage;

    private void Awake()
    {
        cardCore = GetComponent<CardCore>();

        cardBackImage.gameObject.SetActive(false);

        if (cardCore != null)
        {
            nameText.text = cardCore.cardName;
            descriptionText.text = cardCore.cardDescription;
            artImage.sprite = cardCore.cardArt;
            cardBackImage.sprite = cardCore.cardBackArt;
        }
        else
        {
            Debug.LogError("Card Display: Card Core not found on " + this.name);
            nameText.text = "ERROR";
            descriptionText.text = "ERROR";
        }
    }

    public void FlipCard(bool showFace)
    {
        if(showFace == true)
        {
            cardBackImage.gameObject.SetActive(false);
            artImage.gameObject.SetActive(true);
            nameText.gameObject.SetActive(true);
            descriptionText.gameObject.SetActive(true);
        }
        else
        {
            cardBackImage.gameObject.SetActive(true);
            artImage.gameObject.SetActive(false);
            nameText.gameObject.SetActive(false);
            descriptionText.gameObject.SetActive(false);
        }
    }
}
