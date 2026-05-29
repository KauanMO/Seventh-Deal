using UnityEngine;

public enum Suit
{
    Hearts,
    Diamonds,
    Clubs,
    Spades
}

public enum Rank
{
    Two = 2,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King,
    Ace
}

public class CommonCard
{
    public Suit Suit;
    public Rank Rank;
    public bool positive;
    public bool IsFaceDown;

    public CommonCard(Suit suit, Rank rank)
    {
        Suit = suit;
        Rank = rank;
    }

    public Sprite GetCardSprite()
    {
        if (IsFaceDown)
        {
            return Resources.Load<Sprite>($"CommonCards/back");
        }

        return Resources.Load<Sprite>($"CommonCards/{GetCardSpriteCode()}");
    }

    public int GetBlackJackValue()
    {
        return Rank switch
        {
            Rank.Jack => 10,
            Rank.Queen => 10,
            Rank.King => 10,
            Rank.Ace => 11,
            _ => (int)Rank
        };
    }

    private string GetCardSpriteCode()
    {
        string rankCode = Rank switch
        {
            Rank.Two => "2",
            Rank.Three => "3",
            Rank.Four => "4",
            Rank.Five => "5",
            Rank.Six => "6",
            Rank.Seven => "7",
            Rank.Eight => "8",
            Rank.Nine => "9",
            Rank.Ten => "10",
            Rank.Jack => "jack",
            Rank.Queen => "queen",
            Rank.King => "king",
            Rank.Ace => "ace",
            _ => ""
        };

        return $"{rankCode}_of_{Suit.ToString().ToLower()}";
    }

    public override string ToString()
    {
        return $"{Rank} of {Suit} positive {positive} facedown {IsFaceDown}";
    }
}
