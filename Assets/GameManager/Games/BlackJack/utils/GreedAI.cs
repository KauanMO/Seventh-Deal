using UnityEngine;

public class GreedAI
{
    private readonly RandomHelper random;

    public GreedAI(RandomHelper random)
    {
        this.random = random;
    }

    public GreedDecision TakeDecision(JackBlackPlayer player, JackBlackPlayer greed, int targetScore)
    {
        bool stand = IsStand(greed.score, targetScore);

        if (stand) return GreedDecision.Stand;

        bool cardFaceDown = IsCardFaceDown(player.score, targetScore);
        bool positive = IsPositive(greed.score, targetScore);

        if (cardFaceDown)
        {
            if (positive)
            {
                return GreedDecision.HitFaceDownPositive;
            }
            else
            {
                return GreedDecision.HitFaceDownNegative;
            }
        }
        else
        {
            if (positive)
            {
                return GreedDecision.HitPositive;
            }
            else
            {
                return GreedDecision.HitNegative;
            }
        }
    }

    private bool IsStand(int greedScore, int targetScore)
    {
        if (greedScore.Equals(targetScore)) return true;

        if (greedScore >= MathHelper.Percentage(targetScore, 80) && greedScore < targetScore)
            if (random.Chance(70f)) return true;

        return false;
    }

    private bool IsCardFaceDown(int playerScore, int targetScore)
    {
        if (playerScore >= MathHelper.Percentage(targetScore, 80))
        {
            if (random.Chance(40f)) return true;
        }

        return false;
    }

    private bool IsPositive(int greedScore, int targetScore)
    {
        if (greedScore > MathHelper.Percentage(targetScore, 80))
        {
            if (random.Chance(85f)) return false;
        }

        if (greedScore > targetScore)
        {
            if (random.Chance(90f)) return false;
        }

        return true;
    }
}