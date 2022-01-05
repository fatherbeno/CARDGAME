using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [SerializeField]
    private CardInformation card; //Init card information from editor

    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text descriptionText;

    [SerializeField]
    private Image artworkImage;

    // Start is called before the first frame update
    void Start()
    {
        if(card != null)
        {
            card.Print();
            nameText.text = card.name;
            descriptionText.text = card.description;
            artworkImage.sprite = card.artwork;
        }
        else
        {
            Debug.LogError("Card information has not been supplied for " + this.name);
        }
    }
}
