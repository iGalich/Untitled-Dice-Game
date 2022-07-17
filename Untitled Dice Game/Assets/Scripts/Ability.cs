using UnityEngine;
using UnityEngine.UI;

public enum AbilityType
{ 
    Attack,
    Armor,
    Invest,
    EnemyMove
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

            case AbilityType.EnemyMove:
                GameManager.Instance.CurrentEnemy.CastAbility(_abilityValue);
                break;

            default:
                Debug.LogError("No ability type chosen");
                break;
        }
    }

    private void Attack()
    {
        if (_abilityValue <= 0) return;
        GameManager.Instance.Player.AttackAnimation();
        GameManager.Instance.CurrentEnemy.TakeDamage(_abilityValue);
    }

    private void Armor()
    {
        Debug.Log("Armor up");
        GameManager.Instance.Player.ArmorUp(_abilityValue);
    }

    private void Invest()
    {
        Debug.Log("Invested");
        GameManager.Instance.Player.Invest(_abilityValue);
    }
}