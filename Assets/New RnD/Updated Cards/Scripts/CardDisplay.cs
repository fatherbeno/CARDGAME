using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    private CardCore cardCore;
    private Renderer cardRenderer;
    private GameObject cardTextParent;
    private GameObject cardArt;

    private void Awake()
    {
        cardCore = GetComponent<CardCore>();
        cardTextParent = transform.Find("CardDisplay/CardText").gameObject;
        cardArt = transform.Find("CardDisplay/CardArt").gameObject;
        cardRenderer = cardArt.GetComponent<Renderer>();

        SetText("Name", cardCore.cardName);
        SetText("Tier", cardCore.cardTier.ToString());
        SetText("Cost", cardCore.cardCost.ToString());
        SetText("Description", cardCore.cardDescription);
        SetText("Attack", cardCore.cardAttack.ToString());
        SetText("Health", cardCore.cardHealth.ToString());
        SetText("Armour", cardCore.cardArmour.ToString());

        cardRenderer.material.SetTexture("_CardArt", cardCore.cardArt);
    }

    private void SetText(string textObject, string newText)
    {
        cardTextParent.transform.Find(textObject).gameObject.GetComponent<TextMeshPro>().text = newText;
    }

    private Transform RecursiveFindChild(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
            {
                return child;
            }
            else
            {
                Transform found = RecursiveFindChild(child, childName);
                if (found != null)
                {
                    return found;
                }
            }
        }
        return null;
    }
}
