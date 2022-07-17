using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private Ability[] _abilities;
    [SerializeField] private float _abilityDelay = 0.5f;
    [SerializeField] private Button _endTurnButton;

    public void EndTurn()
    {
        _endTurnButton.interactable = false;
        var i = 0;

        _abilities[i++].CastAbility(); // Player Attack
        FunctionTimer.Create(() => _abilities[i++].CastAbility(), _abilityDelay * 1f); // Armor Up
        FunctionTimer.Create(() => _abilities[i++].CastAbility(), _abilityDelay * 2f); // Invest

        if (!GameManager.Instance.CurrentEnemy.IsDead())
        {
            FunctionTimer.Create(() => _abilities[i].CastAbility(), _abilityDelay * 3f); // Enemy Turn
            FunctionTimer.Create(() => GameManager.Instance.NewTurn(), _abilityDelay * 4f); // New Turn
        }

        FunctionTimer.Create(() => _endTurnButton.interactable = true, _abilityDelay * 5f);
    }
}