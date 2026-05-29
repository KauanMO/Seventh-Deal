using UnityEngine;
using TMPro;

public class BlackJackManager : MonoBehaviour
{
    [SerializeField]
    private GameObject cardPrefab;

    [SerializeField]
    private GameObject newRoundButtonPrefab;

    [SerializeField]
    private Transform playerArea;

    [SerializeField]
    private Transform greedArea;

    [SerializeField]
    private Transform newRoundButtonArea;

    [SerializeField]
    private TMP_Text playerScoreText;

    [SerializeField]
    private TMP_Text greedScoreText;

    [SerializeField]
    private TMP_Text playerBurnText;

    [SerializeField]
    private TMP_Text gameResultText;

    [SerializeField]
    private TMP_Text targetScoreText;

    [SerializeField]
    private TMP_Text matchPointsText;

    [SerializeField]
    private PlayerOptionsUI playerOptionsUI;

    [SerializeField]
    private int seed;

    [SerializeField]
    private int maxBurn;

    [SerializeField]
    private int targetScore;

    [SerializeField]
    private int matchRoundsCount;

    private MatchManager matchManager;

    private GameObject newRoundButton;

    private void Start()
    {
        matchManager = new(matchRoundsCount, matchPointsText);

        StartNewRound();
    }

    private void StartNewRound()
    {
        if (newRoundButton != null) Object.Destroy(newRoundButton);
        if (matchManager.HasMatchWinner()) return;

        RandomHelper random = new(seed);

        RoundManager currentRound = new(
                cardPrefab,
                playerArea,
                greedArea,
                playerScoreText,
                greedScoreText,
                playerBurnText,
                gameResultText,
                targetScoreText,
                random,
                maxBurn,
                targetScore);

        currentRound.OnRoundEnded += HandleRoundEnd;

        currentRound.StartRound();

        playerOptionsUI.Setup(currentRound);
    }

    private void HandleRoundEnd(GameResult result)
    {
        matchManager.RegisterRoundWinner(result);

        if (matchManager.HasMatchWinner()) return;

        newRoundButton = GameObject.Instantiate(newRoundButtonPrefab, newRoundButtonArea, false);

        newRoundButton.GetComponent<NewRoundOnClick>().OnNewRoundRequest += StartNewRound;
    }
}