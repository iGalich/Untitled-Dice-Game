using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public UnityEvent newTurnStarted;

    [SerializeField] private Canvas _canvas;
    [SerializeField] private int _currentDiceAmount = 2;
    [SerializeField] private Dice[] _dices = new Dice[6];
    [SerializeField] private Dice _extraDice;
    [SerializeField] private GameObject _lootCanvas;
    [SerializeField] private EnemyInfo[] _enemies;
    private Color _powerfulDiceColor = Color.yellow;
    private Color _weakDiceColor = Color.red;
    private int _enemyIndex = 0;

    [Header("Battle")]
    [SerializeField] private Player _player;
    [SerializeField] private Enemy _currentEnemy;
    [SerializeField] private AudioClip[] _healSFX;

    public Canvas Canvas => _canvas;
    public GameObject LootCanvas { get => _lootCanvas; set => _lootCanvas = value; }
    public int CurrentDiceAmount { get => _currentDiceAmount; set => _currentDiceAmount = value; }
    public Dice[] Dices { get => _dices; set => _dices = value; }
    public Dice ExtraDice => _extraDice;
    public Player Player { get => _player; set => _player = value; }
    public Enemy CurrentEnemy { get => _currentEnemy; set => _currentEnemy = value; }
    public int EnemyIndex => _enemyIndex;
    public AudioClip[] HealSFX => _healSFX;

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
        if (_player.IsDead) return;

        RollDices();
        _currentEnemy.ChooseAbility();
        newTurnStarted.Invoke();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void NextBattle()
    {
        _enemyIndex++;

        if (_enemyIndex > 2)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        //_currentEnemy.GetComponent<Enemy>().Image.sprite = _enemies[_enemyIndex].enemySprite;
        _currentEnemy.GetComponent<Enemy>().MaxHealth = _enemies[_enemyIndex].maxHealth;
        _currentEnemy.GetComponent<Enemy>().Anim.runtimeAnimatorController = _enemies[_enemyIndex]._animController;
        _currentEnemy.GetComponent<Enemy>().DiceCount = _enemies[_enemyIndex].diceCount;
        _currentEnemy.Reset();

        _player.Invested = 0;
        _player.UpdateInvestText();

        NewTurn();
    }

    public void NewDice(ItemValue value)
    {
        _dices[_currentDiceAmount].gameObject.SetActive(true);
        print(_dices[_currentDiceAmount] + " has been activated");

        if (value != ItemValue.Regular)
        {
            _dices[_currentDiceAmount].CustomDice = true;
            _dices[_currentDiceAmount].DiceType = value;

            _dices[_currentDiceAmount].gameObject.GetComponent<Image>().color = (value == ItemValue.Powerful) ? _powerfulDiceColor : _weakDiceColor;
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