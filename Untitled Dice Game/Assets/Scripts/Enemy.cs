using UnityEngine;
using UnityEngine.UI;

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

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        _health = _maxHealth;
    }

    public void TakeDamage(int value)
    {
        _health -= value;

        if (_health <= 0)
            Death();
    }

    public void Heal(int value)
    {
        _health += value;

        if (_health > _maxHealth)
            _health = _maxHealth;
    }

    private void Death()
    {
        Debug.Log("Enemy has been defeated");
    }
}