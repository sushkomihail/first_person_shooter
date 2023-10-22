using UnityEngine;

namespace Initializator
{
    public class Initializator : MonoBehaviour
    {
        [SerializeField] private Initializable[] _components;

        private void Awake()
        {
            foreach (Initializable component in _components)
            {
                component.Initialize();
            }
        }
    }
}