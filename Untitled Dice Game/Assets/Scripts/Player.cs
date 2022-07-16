using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _health = 10;
    [SerializeField] private int _armor = 0;
    [SerializeField] private int _invested = 0;

    public void ArmorUp(int value)
    {
        _armor += value;
    }

    public void Invest(int value)
    {
        _invested += value;
    }
}