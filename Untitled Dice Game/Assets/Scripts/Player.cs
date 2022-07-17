using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    private bool _inAnimation = false;
    private bool _isDead = false;

    [Header("Stats")]
    [SerializeField] private int _maxHealth = 10;
    [SerializeField] private int _armor = 0;
    [SerializeField] private int _invested = 0;
    private int _health = 10;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private Image _healthBarBack;
    [SerializeField] private Image _healthBarFront;
    [SerializeField] private TextMeshProUGUI _armorText;
    [SerializeField] private TextMeshProUGUI _investText;

    public int Health { get => _health; set => _health = value; }
    public int MaxHealth => _maxHealth;
    public int Invested => _invested;

    private void Start()
    {
        _health = _maxHealth;

        UpdateArmorText();
        UpdateHealthText();
        UpdateInvestText();
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

    private void UpdateArmorText()
    {
        _armorText.text = $"Armor : {_armor}";
    }

    private void UpdateInvestText()
    {
        if (GameManager.Instance.EnemyIndex == 2)
            _investText.text = "Investing now heals";
        else
        {
            var type = InvestCalculator.Instance.InvestToItem();
            string color = "";

            switch (type)
            {
                case ItemValue.Powerful:
                    color = "yellow";
                    break;

                case ItemValue.Regular:
                    color = "green";
                    break;

                case ItemValue.Weak:
                    color = "red";
                    break;
            }

            _investText.text = $"Amount Invested:\n<color={color}>{_invested}</color>";
        }
    }

    public void Heal(int value)
    {
        _health += value;

        if (_health > _maxHealth)
            _health = _maxHealth;

        UpdateHealthText();
    }

    public void TakeDamage(int value)
    {
        if (_armor > 0)
        {
            if (_armor >= value)
            {
                _armor -= value;
                value = 0;
            }
            else
            {
                value -= _armor;
                _armor = 0;
            }

            UpdateArmorText();
        }

        _health -= value;

        UpdateHealthText();

        _inAnimation = true;
        _anim.CrossFade(Hurt, 0f, 0);

        if (_health <= 0)
            PlayerDeath();
    }

    public void ArmorUp(int value)
    {
        _armor += value;
        UpdateArmorText();
    }

    public void Invest(int value)
    {
        if (GameManager.Instance.EnemyIndex == 2)
            Heal(value);
        else
        {
            _invested += value;
            UpdateInvestText();
        }
    }

    private void PlayerDeath()
    {
        _isDead = true;
        _inAnimation = true;
        _anim.CrossFade(Death, 0f, 0);
    }

    public void AttackAnimation()
    {
        _inAnimation = true;
        _anim.CrossFade(Attack, 0f, 0);
    }

    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Hurt = Animator.StringToHash("Hurt");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Death = Animator.StringToHash("Death");
}