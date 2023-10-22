using UnityEngine;

namespace Character
{
    public sealed class EnemyHealth : Health
    {
        public override void ApplyDamage(int amount)
        {
            DecreaseHealth(ref CurrentHealth, MaxHealth, amount);

            if (CurrentHealth == 0)
            {
                IsDied = true;
                EventManager.OnEnemyDeath.Invoke();
            }
        }
    }
}