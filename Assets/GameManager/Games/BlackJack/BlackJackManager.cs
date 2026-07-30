using UnityEngine;
using TMPro;
using Yarn.Unity;

public class BlackJackManager : MonoBehaviour
{
    [SerializeField]
    private DialogueRunner dialogueRunner;

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
    private TMP_Text targetScoreText;

    [SerializeField]
    private TMP_Text matchPointsText;

    [SerializeField]
    private PlayerOptionsUI playerOptionsUI;

    [SerializeField]
    private readonly int seed;

    [SerializeField]
    private int maxBurn;

    [SerializeField]
    private int targetScore;

    [SerializeField]
    private int matchRoundsCount;

    private void Start()
    {
        DialogueManager dialogueManager = new(dialogueRunner);

        MatchManager matchManager = new(matchRoundsCount,
            matchPointsText,
            cardPrefab,
            matchRoundsCount,
            targetScore,
            maxBurn,
            seed,
            playerOptionsUI,
            targetScoreText,
            playerBurnText,
            greedScoreText,
            newRoundButtonPrefab,
            playerArea,
            greedArea,
            newRoundButtonArea,
            playerScoreText,
            dialogueManager);

        dialogueManager.TriggerDialogue("GreedIntro", next: () => matchManager.StartMatch());
    }
}