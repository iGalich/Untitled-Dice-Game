using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
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
        _investText.text = $"Amount Invested:\n{_invested}";
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

        if (_health <= 0)
            Death();
    }

    public void ArmorUp(int value)
    {
        _armor += value;
        UpdateArmorText();
    }

    public void Invest(int value)
    {
        _invested += value;
        UpdateInvestText();
    }

    private void Death()
    {
        Debug.Log("Player has died");
    }
}