using UnityEngine;
using UnityEngine.EventSystems;

public class CardHover : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    private Vector3 targetScale;
    private Vector3 originalScale;
    private Quaternion originalRotation;

    [SerializeField]
    private float hoverMultiplier = 1.2f;

    [SerializeField]
    private float speed = 10f;

    private void Start()
    {
        originalRotation = transform.localRotation;
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.deltaTime * speed
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * hoverMultiplier;

        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;

        transform.localRotation = originalRotation;
    }
}