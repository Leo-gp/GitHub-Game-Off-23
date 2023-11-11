using System;
using main.entity.Card_Management.Card_Data;
using UnityEngine;

namespace main.entity.Card_Management.Deck_Definition
{
    [Serializable]
    public struct CardCopies
    {
        [SerializeField] private Card card;
        [SerializeField] private int copies;
    }
}