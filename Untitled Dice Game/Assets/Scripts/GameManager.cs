using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Canvas _canvas;
    [SerializeField] private int _currentDiceAmount = 3;
    [SerializeField] private Dice[] _dices = new Dice[6];

    public Canvas Canvas => _canvas;
    public int CurrentDiceAmount => _currentDiceAmount;
    public Dice[] Dices => _dices;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != null && Instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        RollDices();
    }

    private void RollDices()
    {
        for (int i = 0; i < _currentDiceAmount; i++)
        {
            _dices[i].RollDice();
        }
    }
}