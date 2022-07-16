using UnityEngine;
using UnityEngine.UI;

public enum AbilityType
{ 
    Attack,
    Armor,
    Invest
}

public class Ability : MonoBehaviour
{
    [SerializeField] private AbilityType _abilityType;
    private int _abilityValue = 0;

    public void SetDiceInAbility(Dice dice)
    {
        _abilityValue = dice.DiceValue;
    }

    public void CastAbility()
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
        GameManager.Instance.CurrentEnemy.TakeDamage(_abilityValue);
    }

    private void Armor()
    {
        GameManager.Instance.Player.ArmorUp(_abilityValue);
    }

    private void Invest()
    {
        GameManager.Instance.Player.Invest(_abilityValue);
    }
}