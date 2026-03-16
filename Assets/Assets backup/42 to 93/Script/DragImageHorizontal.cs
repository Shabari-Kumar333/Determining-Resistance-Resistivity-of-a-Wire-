using UnityEngine;
using UnityEngine.EventSystems;

public class DragImageHorizontal : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [Header("Drag Target")]
    [Tooltip("Only this object will move when this image is dragged")]
    public RectTransform dragTarget;

    private Vector2 startPointerPos;
    private Vector2 startTargetPos;
    private Canvas canvas;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (dragTarget == null) return;

        startPointerPos = eventData.position;
        startTargetPos = dragTarget.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragTarget == null) return;

        float deltaX = (eventData.position.x - startPointerPos.x) / canvas.scaleFactor;

        dragTarget.anchoredPosition = new Vector2(
            startTargetPos.x + deltaX,
            startTargetPos.y
        );
    }
}
