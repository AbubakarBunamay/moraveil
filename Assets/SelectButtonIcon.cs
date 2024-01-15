using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelectButtonIcon : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField]
    private GameObject icon;

    public void OnPointerEnter(PointerEventData eventData)
    {
        icon.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        icon.SetActive(false);
    }

   
}
