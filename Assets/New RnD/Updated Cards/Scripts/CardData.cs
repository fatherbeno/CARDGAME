using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardData : ScriptableObject
{
    public int cardId;

    public string cardName;

    public Texture2D cardArt;

    public string cardDescription;

    public int cardTier;
    public int cardCost;
    public int cardHealth;
    public int cardAttack;
    public int cardArmour;

    public enum CardType {BodyPart, Accessory, Event, Dismantle, Espionage};
    public enum BodyPartType {False, Head, Torso, Arm, Leg};
    public enum AccessoryType {False, Head, Torso, Arm, Leg};

    public CardType cardType;
    public BodyPartType bodyPartType;
    public AccessoryType accessoryType;

    public void Debugging()
    {
        Debug.Log(this.name + ": " + cardId + ", " + cardName + ", " + cardCost + ", " + cardHealth + ", " + cardAttack + ", " + cardArmour + ", " + cardType + ", " + bodyPartType + ", " + accessoryType);
    }
}
