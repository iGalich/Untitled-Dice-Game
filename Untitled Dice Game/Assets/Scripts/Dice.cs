using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    [SerializeField] private Sprite[] _diceSprites;
    private Image _image;
    private int _diceValue;
    private AbilityType _setOnAbility;

    public Image Image { get => _image; set => _image = value; }
    public int DiceValue => _diceValue;
    public AbilityType SetOnAbility { get => _setOnAbility; set => _setOnAbility = value; }

    private void Start()
    {
        _image = GetComponent<Image>();
        _image.sprite = _diceSprites[5];
    }

    public void RollDice()
    {
        int _diceThrow = Random.Range(0, 6);
        _image.sprite = _diceSprites[_diceThrow];
        _diceValue = _diceThrow + 1;
    }
}