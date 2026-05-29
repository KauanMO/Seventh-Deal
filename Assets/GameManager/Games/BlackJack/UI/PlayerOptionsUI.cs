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
        round.Play(play);
    }

    public void StandPlay()
    {
        PlayerPlay(TurnPlay.Stand);
    }
}