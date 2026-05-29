using TMPro;
using UnityEngine;
using System;

public class RoundManager
{
    private readonly CommonDeck deck;
    private readonly JackBlackPlayer player;
    private readonly JackBlackPlayer greed;

    private readonly GameObject cardPrefab;

    private readonly Transform playerArea;
    private readonly Transform greedArea;

    private readonly TMP_Text gameResultText;

    private readonly int maxBurn;
    private readonly int targetScore;

    private bool gameIsOn = true;

    private readonly GreedAI greedAI;

    public event Action<GameResult> OnRoundEnded;

    public RoundManager(GameObject cardPrefab,
        Transform playerArea,
        Transform greedArea,
        TMP_Text playerScore,
        TMP_Text greedScore,
        TMP_Text playerBurn,
        TMP_Text gameResultText,
        TMP_Text targetScoreText,
        RandomHelper random,
        int maxBurn,
        int targetScore)
    {

        player = new JackBlackPlayer(Player.Player, new(playerScore, playerBurn));
        greed = new JackBlackPlayer(Player.Greed, new(greedScore));
        deck = new CommonDeck();

        this.cardPrefab = cardPrefab;
        this.playerArea = playerArea;
        this.greedArea = greedArea;

        this.gameResultText = gameResultText;

        this.maxBurn = maxBurn;
        this.targetScore = targetScore;
        targetScoreText.SetText(targetScore.ToString());

        greedAI = new(random);

        gameResultText.SetText("");
        CleanTable();
    }

    public void StartRound()
    {
        deck.Shuffle();

        DrawInitialCards();
        ShowCards();
    }

    public void Play(TurnPlay turnPlay)
    {
        switch (turnPlay)
        {
            case TurnPlay.HitPositive:
                Hit(true);
                GreedPlay();
                break;
            case TurnPlay.HitNegative:
                Hit(false);
                GreedPlay();
                break;
            case TurnPlay.Stand:
                GreedPlay();
                Stand();
                break;
            default:
                Debug.Log("Error");
                break;
        }
    }

    private void GreedPlay()
    {
        GreedDecision greedDecision = greedAI.TakeDecision(player,
            greed,
            targetScore);

        switch (greedDecision)
        {
            case GreedDecision.HitPositive:
                Hit(true, Player.Greed);
                break;
            case GreedDecision.HitNegative:
                Hit(false, Player.Greed);
                break;
            case GreedDecision.HitFaceDownPositive:
                Hit(true, Player.Greed, true);
                break;
            case GreedDecision.HitFaceDownNegative:
                Hit(false, Player.Greed, true);
                break;
            case GreedDecision.Stand:
                break;
            default:
                Debug.Log("Error");
                break;
        }
    }

    private void Hit(bool positive, Player player = Player.Player, bool isFaceDown = false)
    {
        if (!gameIsOn) return;
        CommonCard card;

        switch (player)
        {
            case Player.Player:
                card = deck.Draw(positive: positive);
                this.player.DrawCard(card);
                InstantiateCard(card, Player.Player);
                if (!card.positive) CheckBurn();
                break;

            case Player.Greed:
                card = deck.Draw(positive: positive, isFaceDown: isFaceDown);
                greed.DrawCard(card);
                InstantiateCard(card, Player.Greed);
                break;
        }
    }

    private void Stand()
    {
        GameResult result = GetResult();

        EndGame(result);
    }

    private void DrawInitialCards()
    {
        player.DrawCard(deck.Draw());
        greed.DrawCard(deck.Draw());

        player.DrawCard(deck.Draw());
        greed.DrawCard(deck.Draw(isFaceDown: true));
    }

    private void ShowCards()
    {
        foreach (CommonCard card in player.hand)
        {
            CardView cardView = GetCardView(playerArea, card);

            if (cardView == null)
            {
                InstantiateCard(card, Player.Player);
            }
            else
            {
                TurnCardFaceUp(cardView);
            }
        }

        foreach (CommonCard card in greed.hand)
        {
            CardView cardView = GetCardView(greedArea, card);

            if (cardView == null)
            {
                InstantiateCard(card, Player.Greed);
            }
            else
            {
                TurnCardFaceUp(cardView);
            }
        }
    }

    private void TurnCardFaceUp(CardView cardView)
    {
        cardView.TurnCard();
    }

    private void InstantiateCard(CommonCard card, Player area)
    {
        Transform cardArea = area switch
        {
            Player.Player => playerArea,
            Player.Greed => greedArea,
            _ => throw new System.ArgumentOutOfRangeException(nameof(area))
        };

        GameObject obj = UnityEngine.Object.Instantiate(cardPrefab, cardArea);

        obj.GetComponent<CardView>().Setup(card);
    }

    private CardView GetCardView(Transform area, CommonCard card)
    {
        foreach (Transform child in area)
        {
            CardView view = child.GetComponent<CardView>();

            if (view != null && view.card == card)
            {
                return view;
            }
        }

        return null;
    }

    private void CheckBurn()
    {
        if (player.burn >= maxBurn) EndGame(GameResult.GreedWon);
    }

    private GameResult GetResult()
    {
        if (player.score.Equals(greed.score)) return GameResult.Tie;

        if (greed.score > targetScore) return GameResult.PlayerWon;

        if (targetScore - player.score > targetScore - greed.score || player.score > targetScore) return GameResult.GreedWon;

        return GameResult.PlayerWon;
    }

    private void EndGame(GameResult result)
    {
        if (!gameIsOn) return;

        TurnCardsFaceUp();

        gameIsOn = false;

        gameResultText.SetText(result switch
        {
            GameResult.GreedWon => "You lose",
            GameResult.PlayerWon => "You won",
            GameResult.Tie => "Tie",
            _ => "error"
        });

        OnRoundEnded.Invoke(result);
    }

    private void TurnCardsFaceUp()
    {
        player.TurnCardsFaceUp();
        greed.TurnCardsFaceUp();

        ShowCards();
    }

    private void CleanTable()
    {
        foreach (Transform child in playerArea)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Transform child in greedArea)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}