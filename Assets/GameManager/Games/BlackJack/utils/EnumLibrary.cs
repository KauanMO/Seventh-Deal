public enum Player
{
    Player,
    Greed
}

public enum GameResult
{
    PlayerWon_BetterCards,
    PlayerWon_GreedBurn,
    GreedWon_BetterCards,
    GreedWon_PlayerBurn,
    Tie,
    Tie_TargetScore
}

public enum TurnPlay
{
    HitPositive,
    HitNegative,
    Stand
}

public enum GreedDecision
{
    HitPositive,
    HitNegative,
    HitFaceDownPositive,
    HitFaceDownNegative,
    Stand
}