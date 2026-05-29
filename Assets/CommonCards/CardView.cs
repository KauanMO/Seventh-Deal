using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    public CommonCard card;

    [SerializeField]
    private Image cardImage;

    [SerializeField]
    private Image cardBackground;

    public void Setup(CommonCard card)
    {
        this.card = card;
        cardImage.sprite = card.GetCardSprite();

        if (card.positive)
        {
            cardBackground.color = Color.white;
        }
        else
        {
            cardBackground.color = Color.mediumPurple;
        }
    }

    public void TurnCard()
    {
        cardImage.sprite = card.GetCardSprite();
    }
}
