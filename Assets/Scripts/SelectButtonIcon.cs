using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelectButtonIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    
    [SerializeField] private GameObject icon; // Highlight Icon
    private bool isHighlighted;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        icon.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        icon.SetActive(false);
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        icon.SetActive(true);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        icon.SetActive(false);
    }
}
