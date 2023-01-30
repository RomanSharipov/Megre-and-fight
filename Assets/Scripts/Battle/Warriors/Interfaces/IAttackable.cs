namespace MergeAndFight.Fight
{
    public interface IAttackable
    {
        public void TakeDamageWithAnimation(int damage);
        public void Die();
    }
}