using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class MobileJoystick : MonoBehaviour, IDragHandler, IEndDragHandler
{
    RectTransform rt;
    Vector2 originalAnchored;

    public Vector2 axisValue;
    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        originalAnchored = rt.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var parent = rt.parent.GetComponent<RectTransform>();
        var parentSize = parent.rect.size;
        var parentPoint = eventData.position - parentSize;

        Vector2 localPoint = parent.InverseTransformPoint(parentPoint);

        Vector2 newAnchorPoint = localPoint - originalAnchored;
        newAnchorPoint = Vector2.ClampMagnitude(newAnchorPoint, parentSize.x / 2);

        rt.anchoredPosition = newAnchorPoint;
        axisValue = newAnchorPoint / (parentSize.x / 2);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        rt.anchoredPosition = Vector3.zero;
        axisValue = Vector2.zero;
    }
}
