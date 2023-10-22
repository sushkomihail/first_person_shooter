using UnityEngine;

namespace Character
{
    public sealed class PlayerHealth : Health
    {
        [SerializeField] private int MaxArmor;

        private int _currentArmor;

        protected override void Awake()
        {
            base.Awake();

            _currentArmor = MaxArmor;
        }

        public override void ApplyDamage(int amount)
        {
            _currentArmor -= Mathf.Abs(amount);

            if (_currentArmor < 0)
            {
                DecreaseHealth(ref CurrentHealth, MaxHealth, _currentArmor);

                if (CurrentHealth == 0)
                {
                    IsDied = true;
                    EventManager.OnPlayerDeath.Invoke();
                }
            }
        }
    }
}