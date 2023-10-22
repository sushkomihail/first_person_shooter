using System;
using Mechanics.ObjectsSelection;
using UnityEngine;

namespace Weapon
{
    public class WeaponController : MonoBehaviour
    {
        private Weapon _weapon;

        public void Initialize(Weapon weapon)
        {
            _weapon = weapon;
        }
    }
}
