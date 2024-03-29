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

    [Header("SFX")]
    [SerializeField] private AudioClip[] _armorUpSFX;
    [SerializeField] private AudioClip[] _investSFX;
    [SerializeField] private AudioClip[] _hurtSFX;
    [SerializeField] private AudioClip _deathJingle;

    public int Health { get => _health; set => _health = value; }
    public int MaxHealth => _maxHealth;
    public int Invested { get => _invested; set => _invested = value; }
    public bool IsDead => _isDead;

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

    private void ShakeBar()
    {
        iTween.ShakePosition(_healthBarFront.rectTransform.parent.GetComponent<RectTransform>().gameObject, Vector3.one * 50f, 0.5f);
    }

    private void UpdateArmorText()
    {
        _armorText.text = $"Armor : {_armor}";
    }

    public void UpdateInvestText()
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
        if (value <= 0) return;

        AudioManager.Instance.PlaySound(GameManager.Instance.HealSFX[Random.Range(0, GameManager.Instance.HealSFX.Length)]);
        _health += value;

        if (_health > _maxHealth)
            _health = _maxHealth;

        UpdateHealthText();
    }

    public void TakeDamage(int value)
    {
        AudioManager.Instance.PlaySound(_hurtSFX[Random.Range(0, _hurtSFX.Length)]);

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

        if (value > 0)
        {
            ShakeBar();
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
        if (value <= 0) return;
        _armor += value;
        UpdateArmorText();
        AudioManager.Instance.PlaySound(_armorUpSFX[Random.Range(0, _armorUpSFX.Length)]);
    }

    public void Invest(int value)
    {
        if (value <= 0) return;

        if (GameManager.Instance.EnemyIndex == 2)
            Heal(value);
        else
        {
            _invested += value;
            UpdateInvestText();
            AudioManager.Instance.PlaySound(_investSFX[Random.Range(0, _investSFX.Length)]);
        }
    }

    private void PlayerDeath()
    {
        AudioManager.Instance.SwitchMusic(_deathJingle);
        _isDead = true;
        _inAnimation = true;
        _anim.CrossFade(Death, 0f, 0);
        FunctionTimer.Create(() => GameManager.Instance.LoadMainMenu(), _deathJingle.length);
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