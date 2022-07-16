using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LootCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healText;
    [SerializeField] private TextMeshProUGUI _itemText;

    private void OnEnable()
    {
        _healText.text = $"Heal for {InvestCalculator.Instance.InvestToHeal()}";
        _itemText.text = $"Get {InvestCalculator.Instance.InvestToItem().ToString()} Dice";
    }

    public void Heal()
    {
        GameManager.Instance.Player.Heal(InvestCalculator.Instance.InvestToHeal());
        GameManager.Instance.NextBattle();
    }

    public void AddNewDice()
    {
        GameManager.Instance.NewDice(InvestCalculator.Instance.InvestToItem());
        GameManager.Instance.NextBattle();
    }
}