using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewRoundOnClick : MonoBehaviour,
    IPointerClickHandler
{
    public event Action OnNewRoundRequest;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnNewRoundRequest.Invoke();
    }
}