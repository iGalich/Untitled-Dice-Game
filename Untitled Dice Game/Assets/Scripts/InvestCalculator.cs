using UnityEngine;

public enum ItemValue
{
    Powerful,
    Regular,
    Weak
}

public class InvestCalculator : MonoBehaviour
{
    public static InvestCalculator Instance { get; private set; }

    [SerializeField] private int _regularThreshhold = 8;
    [SerializeField] private int _powerfulThreshhold = 16;
    private int _investedCount;

    public int InvestedCount { get => _investedCount; set => _investedCount = value; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != null && Instance != this)
            Destroy(gameObject);
    }

    public int InvestToHeal()
    {
        _investedCount = GameManager.Instance.Player.Invested;

        return _investedCount / 2;
    }

    public ItemValue InvestToItem()
    {
        _investedCount = GameManager.Instance.Player.Invested;

        if (_investedCount < _regularThreshhold)
            return ItemValue.Weak;
        if (_investedCount < _powerfulThreshhold)
            return ItemValue.Regular;
        return ItemValue.Powerful;
    }
}