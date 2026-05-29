using TMPro;

public class MatchManager
{
    private int playerWins;
    private int greedWins;

    private readonly int roundsToWin;

    private readonly TMP_Text matchPointsText;

    public MatchManager(int roundsToWin, TMP_Text matchPointsText)
    {
        this.roundsToWin = roundsToWin;
        this.matchPointsText = matchPointsText;
    }

    public void RegisterRoundWinner(GameResult result)
    {
        switch (result)
        {
            case GameResult.PlayerWon:
                playerWins += 1;
                break;
            case GameResult.GreedWon:
                greedWins += 1;
                break;
            default:
                break;
        }

        matchPointsText.SetText($"{playerWins} - {greedWins}");
    }

    public bool HasMatchWinner()
    {
        return playerWins >= roundsToWin
            || greedWins >= roundsToWin;
    }
}