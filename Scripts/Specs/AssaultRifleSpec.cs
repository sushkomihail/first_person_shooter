using UnityEngine;

namespace Specs
{
    [CreateAssetMenu(fileName = "_Spec", menuName = "Specs/AssaultRifle")]
    public class AssaultRifleSpec : WeaponSpec
    {        
        public enum ShootingType
        {
            Auto,
            Burst
        }
        public ShootingType Type;
        public int BurstBulletsCount;
        public int FireRate;
    }
}
