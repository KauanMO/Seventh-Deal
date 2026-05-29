using UnityEngine;
using UnityEngine.EventSystems;

public class DeckHover : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [SerializeField]
    private Texture2D hoverCursor;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(
            hoverCursor,
            Vector2.zero,
            CursorMode.Auto
        );
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(
           null,
           Vector2.zero,
           CursorMode.Auto
       );
    }
}