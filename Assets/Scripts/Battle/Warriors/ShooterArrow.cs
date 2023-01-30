using MergeAndFight.Fight;
using UnityEngine;

public class ShooterArrow : MonoBehaviour
{
    [SerializeField] private Archer _archer;

    public void Shoot()
    {
        _archer.Attack();
    }
}
