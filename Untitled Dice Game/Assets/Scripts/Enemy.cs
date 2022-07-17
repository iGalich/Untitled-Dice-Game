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
    [SerializeField] private Animator _anim;
    [SerializeField] private int _maxHealth = 10;
    [SerializeField] private int _health;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _enemyAbilityText;
    private int _diceCount = 1;
    private EnemyAbility _nextAbility;
    private int _diceRoll;
    private bool _inAnimation = false;
    private bool _isDead = false;

    [Header("HealthUI")]
    [SerializeField] private Image _healthBarBack;
    [SerializeField] private Image _healthBarFront;
    [SerializeField] private TextMeshProUGUI _healthText;

    [Header("SFX")]
    [SerializeField] private AudioClip[] _hurtSFX;

    public Image Image { get => _image; set => _image = value; }
    public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public int DiceCount { get => _diceCount; set => _diceCount = value; }
    public Animator Anim { get => _anim; set => _anim = value; }

    private void Awake()
    {
        _image = GetComponent<Image>();
        _health = _maxHealth;
    }

    private void Start()
    {
        UpdateHealthText();
    }

    private void Update()
    {
        var delta = Time.deltaTime;

        SyncHealthBar(delta);

        if (_inAnimation && !_isDead)
        {
            _inAnimation = false;
            FunctionTimer.Create(() => _anim.CrossFade(Idle, 0f, 0), 0.5f);
        }
    }

    public void Reset()
    {
        _isDead = false;
        _health = _maxHealth;
        _healthBarBack.fillAmount = 1f;
        _healthBarFront.fillAmount = 1f;
        UpdateHealthText();
    }

    public bool IsDead()
    {
        return _health <= 0;
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

    private void ShakeBar()
    {
        iTween.ShakePosition(_healthBarFront.rectTransform.parent.GetComponent<RectTransform>().gameObject, Vector3.one * 50f, 0.5f);
    }

    public void ChooseAbility()
    {
        _diceRoll = 0;
        for (int i = 0; i < _diceCount; i++)
        {
            _diceRoll += Random.Range(1, 7);
        }
        //_diceRoll = Random.Range(1, 7);

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
                _enemyAbilityText.text = $"Attack and Heal for {Mathf.Ceil(_diceRoll / 2f)}";
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
            case EnemyAbility.Attack:
                GameManager.Instance.Player.TakeDamage(_diceRoll);
                _inAnimation = true;
                _anim.CrossFade(Attack, 0f, 0);
                break;

            case EnemyAbility.Heal:
                Heal(_diceRoll);
                break;

            case EnemyAbility.AttackAndHeal:
                _diceRoll = (int)Mathf.Ceil(_diceRoll / 2f);
                var temp = _diceRoll;
                GameManager.Instance.Player.TakeDamage(_diceRoll);
                _inAnimation = true;
                _anim.CrossFade(Attack, 0f, 0);
                FunctionTimer.Create(() => Heal(temp), 1f);
                break;
        }
    }

    public void TakeDamage(int value)
    {
        if (value <= 0) return;

        AudioManager.Instance.PlaySound(_hurtSFX[Random.Range(0, _hurtSFX.Length)]);
        ShakeBar();
        _health -= value;
        _anim.CrossFade(Hurt, 0f, 0);
        _inAnimation = true;
        UpdateHealthText();

        if (_health <= 0)
            EnemyDeath();
    }

    public void Heal(int value)
    {
        if (value <= 0) return;

        AudioManager.Instance.PlaySound(GameManager.Instance.HealSFX[Random.Range(0, GameManager.Instance.HealSFX.Length)]);
        _health += value;

        if (_health > _maxHealth)
            _health = _maxHealth;

        UpdateHealthText();
    }

    private void EnemyDeath()
    {
        _anim.CrossFade(Death, 0f, 0);
        _inAnimation = true;
        _isDead = true;

        if (GameManager.Instance.EnemyIndex < 2)
            FunctionTimer.Create(() => GameManager.Instance.LootCanvas.SetActive(true), 2.5f);
        else
            FunctionTimer.Create(() => GameManager.Instance.NextBattle(), 2.5f);
    }

    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Hurt = Animator.StringToHash("Hurt");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Death = Animator.StringToHash("Death");
}