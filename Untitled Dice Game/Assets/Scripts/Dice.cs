using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    [SerializeField] private Sprite[] _diceSprites;
    [SerializeField] private Sprite[] _weakDiceSprites;
    [SerializeField] private Sprite[] _powerfulDiceSprites;
    private Vector2 _originalPosition;
    private Image _image;
    private int _diceValue;
    private AbilityType _setOnAbility;
    private bool _customDice = false;
    private ItemValue _diceType = ItemValue.Regular;

    public Image Image { get => _image; set => _image = value; }
    public int DiceValue => _diceValue;
    public AbilityType SetOnAbility { get => _setOnAbility; set => _setOnAbility = value; }
    public bool CustomDice { get => _customDice; set => _customDice = value; }
    public ItemValue DiceType { get => _diceType; set => _diceType = value; }

    private void Awake()
    {
        _image = GetComponent<Image>();
        _image.sprite = _diceSprites[5];
        FunctionTimer.Create(() => _originalPosition = GetComponent<RectTransform>().anchoredPosition, 0.5f);
    }

    public void RollDice()
    {
        int _diceThrow = Random.Range(0, 6);

        if (_customDice)
        {
            switch (_diceType)
            {
                case ItemValue.Powerful:
                    _image.sprite = _powerfulDiceSprites[_diceThrow];
                    if (_diceThrow < 3)
                        _diceValue = 5;
                    else
                        _diceValue = 6;
                    break;

                case ItemValue.Weak:
                    _image.sprite = _weakDiceSprites[_diceThrow];
                    if (_diceThrow < 3)
                        _diceValue = 1;
                    else
                        _diceValue = 2;
                    break;

                default:
                    Debug.Log("Should be unreachable");
                    break;
            }
        }
        else
        {
            _image.sprite = _diceSprites[_diceThrow];
            _diceValue = _diceThrow + 1;
        }
    }

    public void ResetPosition()
    {
        GetComponent<RectTransform>().anchoredPosition = SwitchToRectTransform(GetComponent<RectTransform>(), GameManager.Instance.ExtraDice.GetComponent<RectTransform>());
        GetComponent<RectTransform>().anchoredPosition = _originalPosition;
        GetComponent<DragDrop>().enabled = true;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        GetComponent<CanvasGroup>().alpha = 1f;
    }

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