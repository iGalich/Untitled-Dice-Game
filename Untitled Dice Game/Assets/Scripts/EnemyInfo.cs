using UnityEngine;

[CreateAssetMenu]
public class EnemyInfo : ScriptableObject
{
    public int maxHealth;
    public RuntimeAnimatorController _animController;
    public int diceCount;
}