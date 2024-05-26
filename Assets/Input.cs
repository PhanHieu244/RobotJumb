using ChuongCustom;
using UnityEngine;
using UnityEngine.EventSystems;

public class Input : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        InGameManager.Instance.Stop();
    }
}
