using System.Collections;
using UnityEngine;

namespace Mechanics.Attack
{
    public abstract class Trail : MonoBehaviour
    {
        protected abstract IEnumerator PlayTrail(Vector3 origin, Vector3 hitPosition);
        
        public abstract void ShowTrail(Vector3 origin, Vector3 hitPosition);
    }
}