using UnityEngine;

[CreateAssetMenu(fileName = "Warrior Data", menuName = "Warriors/Warrior Data", order = 54)]
public class WarriorData : ScriptableObject
{
    [SerializeField, Min(0)] private int _health;
    [SerializeField, Min(0)] private int _attackPower;

    public int Health => _health;
    public int AttackPower => _attackPower;
}
