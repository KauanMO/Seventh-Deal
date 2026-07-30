using UnityEngine;

public class PlayerOptionsUI : MonoBehaviour
{
    private RoundManager round;

    public void Setup(RoundManager round)
    {
        this.round = round;
    }

    public void PlayerPlay(TurnPlay play)
    {
        if (round.gameIsOn) round.Play(play);
    }

    public void StandPlay()
    {
        if (round.gameIsOn) PlayerPlay(TurnPlay.Stand);
    }
}