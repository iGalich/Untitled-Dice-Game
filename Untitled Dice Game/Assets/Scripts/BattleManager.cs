using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private Ability[] _abilities;
    [SerializeField] private float _abilityDelay = 0.5f;

    public void EndTurn()
    {
        var i = 0;

        _abilities[i++].CastAbility(); // Player Attack
        FunctionTimer.Create(() => _abilities[i++].CastAbility(), _abilityDelay * 1); // Armor Up
        FunctionTimer.Create(() => _abilities[i++].CastAbility(), _abilityDelay * 2); // Invest

        if (!GameManager.Instance.CurrentEnemy.IsDead())
        {
            FunctionTimer.Create(() => _abilities[i].CastAbility(), _abilityDelay * 3); // Enemy Turn
            FunctionTimer.Create(() => GameManager.Instance.NewTurn(), _abilityDelay * 4); // New Turn
        }
    }
}