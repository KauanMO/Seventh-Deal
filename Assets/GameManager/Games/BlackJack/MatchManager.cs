using TMPro;
using UnityEngine;
using Yarn.Unity;

public class MatchManager
{
    private int playerWins;
    private int greedWins;

    private readonly int roundsToWin;
    private readonly TMP_Text matchPointsText;
    private readonly GameObject cardPrefab;
    private GameObject newRoundButton;
    private readonly int targetScore;
    private readonly int maxBurn;
    private readonly int seed;
    private readonly PlayerOptionsUI playerOptionsUI;
    private readonly TMP_Text targetScoreText;
    private readonly TMP_Text playerBurnText;
    private readonly TMP_Text greedScoreText;
    private readonly GameObject newRoundButtonPrefab;
    private readonly Transform playerArea;
    private readonly Transform greedArea;
    private readonly Transform newRoundButtonArea;
    private readonly TMP_Text playerScoreText;
    private DialogueManager dialogueManager;

    public MatchManager(int roundsToWin,
        TMP_Text matchPointsText,
        GameObject cardPrefab,
        int roundsCount,
        int targetScore,
        int maxBurn,
        int seed,
        PlayerOptionsUI playerOptionsUI,
        TMP_Text targetScoreText,
        TMP_Text playerBurnText,
        TMP_Text greedScoreText,
        GameObject newRoundButtonPrefab,
        Transform playerArea,
        Transform greedArea,
        Transform newRoundButtonArea,
        TMP_Text playerScoreText,
        DialogueManager dialogueManager)
    {
        this.roundsToWin = roundsToWin;
        this.matchPointsText = matchPointsText;
        this.cardPrefab = cardPrefab;
        this.targetScore = targetScore;
        this.maxBurn = maxBurn;
        this.seed = seed;
        this.playerOptionsUI = playerOptionsUI;
        this.targetScoreText = targetScoreText;
        this.playerBurnText = playerBurnText;
        this.greedScoreText = greedScoreText;
        this.newRoundButtonPrefab = newRoundButtonPrefab;
        this.playerArea = playerArea;
        this.greedArea = greedArea;
        this.newRoundButtonArea = newRoundButtonArea;
        this.playerScoreText = playerScoreText;
        this.dialogueManager = dialogueManager;
    }

    public void StartMatch()
    {
        StartNewRound();
    }

    private void RegisterRoundWinner(GameResult result)
    {
        switch (result)
        {
            case GameResult.PlayerWon_BetterCards:
            case GameResult.PlayerWon_GreedBurn:
                playerWins += 1;
                break;
            case GameResult.GreedWon_BetterCards:
            case GameResult.GreedWon_PlayerBurn:
                greedWins += 1;
                break;
            default:
                break;
        }

        matchPointsText.SetText($"{playerWins} - {greedWins}");
    }

    private Player? GetMatchWinner()
    {
        if (playerWins >= roundsToWin) return Player.Player;

        if (greedWins >= roundsToWin) return Player.Greed;

        return null;
    }

    private void StartNewRound()
    {
        if (newRoundButton != null) Object.Destroy(newRoundButton);
        if (GetMatchWinner() != null) return;

        RandomHelper random = new(seed);

        RoundManager currentRound = new(
                cardPrefab,
                playerArea,
                greedArea,
                playerScoreText,
                greedScoreText,
                playerBurnText,
                targetScoreText,
                random,
                dialogueManager,
                maxBurn,
                targetScore);

        currentRound.OnRoundEnded += HandleRoundEnd;

        currentRound.StartRound();

        playerOptionsUI.Setup(currentRound);
    }

    private void HandleRoundEnd(GameResult result)
    {
        RegisterRoundWinner(result);

        Player? winner = GetMatchWinner();

        if (winner != null)
        {
            string dialogueName = winner.Equals(Player.Greed) ? "GreedWonReaction" : "GreedLostReaction";
            dialogueManager.QueueDialogue(dialogueName);
            return;
        }

        newRoundButton = GameObject.Instantiate(newRoundButtonPrefab, newRoundButtonArea, false);
        newRoundButton.GetComponent<NewRoundOnClick>().OnNewRoundRequest += StartNewRound;
    }
}