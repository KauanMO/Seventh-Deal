using System.Collections.Generic;
using UnityEngine;
public class JackBlackPlayer
{
    public readonly List<CommonCard> hand = new();
    public int score = 0;
    public int burn = 0;
    private readonly Player player;
    private readonly PlayerStatsUI playerStatsManager;

    public JackBlackPlayer(Player player, PlayerStatsUI playerStatsManager)
    {
        this.playerStatsManager = playerStatsManager;
        this.playerStatsManager?.SetScore(score);
        this.playerStatsManager?.SetBurn(burn);
        this.player = player;
    }

    public void DrawCard(CommonCard card)
    {
        hand.Add(card);

        CalculateHand();
    }

    public void TurnCardsFaceUp()
    {
        foreach (CommonCard card in hand)
        {
            if (card.IsFaceDown) card.IsFaceDown = false;
        }

        CalculateHand();
    }

    private void CalculateHand()
    {
        int score = 0;
        int burn = 0;

        int aceCount = 0;

        int faceDownScore = 0;

        foreach (CommonCard card in hand)
        {
            if (card.IsFaceDown) faceDownScore += card.GetBlackJackValue();

            if (card.Rank == Rank.Ace)
            {
                aceCount++;
            }

            if (card.positive) score += card.GetBlackJackValue();
            else
            {
                int blackJackValue = card.GetBlackJackValue();
                score -= blackJackValue;
                burn += blackJackValue;

                playerStatsManager?.SetBurn(burn);
            }
        }

        while (score > 21 && aceCount > 0)
        {
            score -= 10;

            aceCount--;
        }

        this.score = score;
        this.burn = burn;

        playerStatsManager?.SetScore(score - faceDownScore,
            containsFaceDownCard:
            player.Equals(Player.Greed)
            && faceDownScore > 0);
    }
}