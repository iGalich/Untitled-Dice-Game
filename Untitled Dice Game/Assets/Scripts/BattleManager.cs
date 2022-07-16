using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private Ability[] _abilities;

    public void EndTurn()
    {
        for (int i = 0; i < _abilities.Length; i++)
        {
            _abilities[i].CastAbility();
        }
    }
}