using UnityEngine;
using UnityEngine.EventSystems;

public class DiceDropSlot : MonoBehaviour, IDropHandler
{
    private Ability _parentAbility;

    private void Awake()
    {
        _parentAbility = GetComponentInParent<Ability>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = SwitchToRectTransform(GetComponent<RectTransform>(), eventData.pointerDrag.GetComponent<RectTransform>());

            Dice dice = eventData.pointerDrag.GetComponent<Dice>();

            dice.GetComponent<DragDrop>().enabled = false;
            dice.GetComponent<CanvasGroup>().alpha = 0.75f;

            _parentAbility.SetDiceInAbility(dice);
        }
    }

    /// <summary>
    /// Converts the anchoredPosition of the first RectTransform to the second RectTransform,
    /// taking into consideration offset, anchors and pivot, and returns the new anchoredPosition
    /// </summary>
    private Vector2 SwitchToRectTransform(RectTransform from, RectTransform to)
    {
       Vector2 localPoint;
       Vector2 fromPivotDerivedOffset = new Vector2(from.rect.width * 0.5f + from.rect.xMin, from.rect.height * 0.5f + from.rect.yMin);
       Vector2 screenP = RectTransformUtility.WorldToScreenPoint(null, from.position);
       screenP += fromPivotDerivedOffset;
       RectTransformUtility.ScreenPointToLocalPointInRectangle(to, screenP, null, out localPoint);
       Vector2 pivotDerivedOffset = new Vector2(to.rect.width * 0.5f + to.rect.xMin, to.rect.height * 0.5f + to.rect.yMin);
       return to.anchoredPosition + localPoint - pivotDerivedOffset;
    }
}