using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class CardData
{
    public int id;
    public bool isActive;
    public string spriteName;
    public int childIndex;
    public bool isHide;

    public CardData(GameObject card, int index)
    {
        var cardController = card.GetComponent<CardController>();
        id = cardController.id;
        isActive = card.activeSelf;
        spriteName = cardController.cardImage.sprite.name;
        childIndex = index;
        //isHide = card.transform.GetChild(0).gameObject.activeSelf;
        isHide = cardController.isHide;
    }
}
