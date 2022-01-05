using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardInformation : ScriptableObject
{
    public int id;

    public new string name;
    public string description;

    public Sprite artwork;

    public void Print()
    {
        Debug.Log("Card Info: " + "ID: " + id + " Name: " + name);
    }
}