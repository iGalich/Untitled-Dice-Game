using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum EnemyAbility
{
    Attack,
    Heal,
    AttackAndHeal
}

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 10;
    [SerializeField] private int _health;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _enemyAbilityText;
    private EnemyAbility _nextAbility;
    private int _diceRoll;

    [Header("HealthUI")]
    [SerializeField] private Image _healthBarBack;
    [SerializeField] private Image _healthBarFront;
    [SerializeField] private TextMeshProUGUI _healthText;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        _health = _maxHealth;
        UpdateHealthText();
    }

    private void Update()
    {
        var delta = Time.deltaTime;

        SyncHealthBar(delta);
    }

    private void SyncHealthBar(float delta)
    {
        _healthBarFront.fillAmount = (float)_health / (float)_maxHealth;

        if (_healthBarBack.fillAmount > _healthBarFront.fillAmount)
        {
            _healthBarBack.fillAmount = Mathf.Lerp(_healthBarBack.fillAmount, _healthBarFront.fillAmount, delta);
        }
    }

    private void UpdateHealthText()
    {
        _healthText.text = $"{_health} / {_maxHealth}";
    }

    public void ChooseAbility()
    {
        _diceRoll = Random.Range(1, 7);

        if (_health == _maxHealth)
            _nextAbility = EnemyAbility.Attack;
        else
            _nextAbility = (EnemyAbility)Random.Range(0, 3);

        switch (_nextAbility)
        {
            case EnemyAbility.Attack:
                _enemyAbilityText.text = $"Attack for {_diceRoll}";
                break;

            case EnemyAbility.Heal:
                _enemyAbilityText.text = $"Heal for {_diceRoll}";
                break;

            case EnemyAbility.AttackAndHeal:
                _enemyAbilityText.text = $"Attack and Heal for {_diceRoll}";
                break;

            default:
                Debug.Log("Enemy ability wasn't chosen");
                break;
        }
    }

    public void CastAbility(int value)
    {
        _diceRoll -= value;
        if (_diceRoll <= 0)
            return;

        switch (_nextAbility)
        {
            case EnemyAbility.AttackAndHeal:

            case EnemyAbility.Attack:
                GameManager.Instance.Player.TakeDamage(_diceRoll);
                break;

            case EnemyAbility.Heal:
                Heal(_diceRoll);
                break;

        }
    }

    public void TakeDamage(int value)
    {
        _health -= value;

        UpdateHealthText();

        if (_health <= 0)
            Death();
    }

    public void Heal(int value)
    {
        _health += value;

        if (_health > _maxHealth)
            _health = _maxHealth;

        UpdateHealthText();
    }

    private void Death()
    {
        GameManager.Instance.LootCanvas.SetActive(true);
    }
}