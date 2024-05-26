using System.Collections;
using System.Collections.Generic;
using ChuongCustom;
using UnityEngine;
using UnityEngine.EventSystems;

public class TapToStart : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        InGameManager.Instance.TapToStart();
        gameObject.SetActive(false);
    }
}
