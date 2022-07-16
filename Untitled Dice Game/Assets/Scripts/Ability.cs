using UnityEngine;
using UnityEngine.UI;

public enum AbilityType
{ 
    None,
    Attack,
    Armor,
    Invest
}

public class Ability : MonoBehaviour
{
    [SerializeField] private AbilityType _abilityType;
    [SerializeField] private Image _diceImage;
    [SerializeField] private Sprite _emptyDice;
    private int _abilityValue = 0;

    public void SetDiceInAbility(Dice dice)
    {
        _abilityValue = dice.DiceValue;
    }

    private void CastAbility()
    {
        switch (_abilityType)
        {
            case AbilityType.Attack:
                Attack();
                break;

            case AbilityType.Armor:
                Armor();
                break;

            case AbilityType.Invest:
                Invest();
                break;

            default:
                Debug.LogError("No ability type chosen");
                break;
        }
    }

    private void Attack()
    {
        Debug.Log("Attacking");
    }

    private void Armor()
    {
        Debug.Log("Armoring up");
    }

    private void Invest()
    {
        Debug.Log("Investing");
    }
}