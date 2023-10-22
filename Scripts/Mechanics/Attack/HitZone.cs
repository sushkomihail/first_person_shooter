using UnityEngine;

namespace Mechanics.Attack
{
    public class HitZone : MonoBehaviour, IDamageModifier
    {
        [SerializeField] private float _damageMultiplier;
        
        public int ModifyDamage(int damage)
        {
            return Mathf.RoundToInt(damage * _damageMultiplier);
        }
    }
}