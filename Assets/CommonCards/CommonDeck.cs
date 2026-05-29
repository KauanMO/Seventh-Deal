using System.Collections.Generic;
using UnityEngine;

public class CommonDeck
{
    public List<CommonCard> Cards = new();
    private static readonly System.Random rng = new();

    public CommonDeck()
    {
        CreateDeck();
    }

    private void CreateDeck()
    {
        foreach (Suit suit in System.Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank rank in System.Enum.GetValues(typeof(Rank)))
            {
                Cards.Add(new CommonCard(suit, rank));
            }
        }
    }

    public void Shuffle()
    {
        int n = Cards.Count;

        while (n > 1)
        {
            n--;

            int k = rng.Next(n + 1);

            (Cards[n], Cards[k]) = (Cards[k], Cards[n]);
        }
    }

    public CommonCard Draw(bool isFaceDown = false, bool positive = true)
    {
        CommonCard card = Cards[0];

        card.IsFaceDown = isFaceDown;
        card.positive = positive;

        Cards.RemoveAt(0);

        return card;
    }
}