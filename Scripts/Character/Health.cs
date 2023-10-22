using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

namespace Character
{
    public abstract class Health : MonoBehaviour
    {
        [SerializeField] protected int MaxHealth;
        
        protected int CurrentHealth;
        
        public bool IsDied { get; protected set; }

        protected virtual void Awake()
        {
            CurrentHealth = MaxHealth;
        }
        
        public virtual void Recover(int amount) {}
        
        public abstract void ApplyDamage(int amount);

        protected void DecreaseHealth(ref int current, int max, int decreaseValue)
        {
            current -= Mathf.Abs(decreaseValue);
            current = Mathf.Clamp(current, 0, max);
        }
    }
}