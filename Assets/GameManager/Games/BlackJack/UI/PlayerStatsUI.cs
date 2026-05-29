using TMPro;
using UnityEngine;

public class PlayerStatsUI
{
    public TMP_Text scoreText;
    public TMP_Text burnText;

    public PlayerStatsUI(TMP_Text scoreText, TMP_Text burnText)
    {
        this.scoreText = scoreText;
        this.burnText = burnText;
    }

    public PlayerStatsUI(TMP_Text scoreText)
    {
        this.scoreText = scoreText;
        burnText = null;
    }

    public void SetScore(int score, bool containsFaceDownCard = false)
    {
        string text = $"Score: {score}";

        if (containsFaceDownCard) text += $" + ?";

        scoreText?.SetText(text);
    }

    public void SetBurn(int burn)
    {
        if (burnText) burnText.SetText($"Burn: {burn}");
    }
}