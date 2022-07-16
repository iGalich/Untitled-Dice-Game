using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Canvas _canvas;
    [SerializeField] private int _currentDiceAmount = 2;
    [SerializeField] private Dice[] _dices = new Dice[6];
    [SerializeField] private Dice _extraDice;
    [SerializeField] private GameObject _lootCanvas;
    [SerializeField] private EnemyInfo[] _enemies;
    private int _enemyIndex = 0;

    [Header("Battle")]
    [SerializeField] private Player _player;
    [SerializeField] private Enemy _currentEnemy;

    public Canvas Canvas => _canvas;
    public GameObject LootCanvas { get => _lootCanvas; set => _lootCanvas = value; }
    public int CurrentDiceAmount { get => _currentDiceAmount; set => _currentDiceAmount = value; }
    public Dice[] Dices { get => _dices; set => _dices = value; }
    public Dice ExtraDice => _extraDice;
    public Player Player { get => _player; set => _player = value; }
    public Enemy CurrentEnemy { get => _currentEnemy; set => _currentEnemy = value; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != null && Instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        NewTurn();
    }

    public void NewTurn()
    {
        RollDices();
        _currentEnemy.ChooseAbility();
    }

    public void NextBattle()
    {
        _enemyIndex++;
        _currentEnemy.GetComponent<Enemy>().Image.sprite = _enemies[_enemyIndex].enemySprite;
        _currentEnemy.GetComponent<Enemy>().MaxHealth = _enemies[_enemyIndex].maxHealth;
        _currentEnemy.Reset();
    }

    public void NewDice(ItemValue value)
    {
        _dices[_currentDiceAmount].enabled = true;

        if (value != ItemValue.Regular)
        {
            _dices[_currentDiceAmount].CustomDice = true;
            _dices[_currentDiceAmount].DiceType = value;
        }
        else
        {
            _dices[_currentDiceAmount].CustomDice = false;
            _dices[_currentDiceAmount].DiceType = ItemValue.Regular;
        }
        _currentDiceAmount++;
    }

    // Reset dice positions and roll
    private void RollDices()
    {
        for (int i = 0; i < _currentDiceAmount; i++)
        {
            _dices[i].ResetPosition();
            _dices[i].RollDice();
        }
    }
}