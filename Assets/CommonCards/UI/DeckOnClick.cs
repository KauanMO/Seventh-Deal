using UnityEngine;
using UnityEngine.EventSystems;

public class DeckOnClick : MonoBehaviour,
    IPointerClickHandler
{
    [SerializeField]
    private TurnPlay play;

    [SerializeField]
    private PlayerOptionsUI playerOptionsUI;

    public void OnPointerClick(PointerEventData eventData)
    {
        playerOptionsUI.PlayerPlay(play);
    }
}