using UnityEngine;
using Animation;
using Character.Avatar;
using Initializator;

namespace Character
{
    public abstract class Human : Initializable
    {
        [Header("Animation")]
        [SerializeField] protected HumanAnimator HumanAnimator;

        protected abstract void OnDeath();
    }
}